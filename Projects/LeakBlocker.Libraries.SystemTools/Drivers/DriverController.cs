using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Devices;
using LeakBlocker.Libraries.SystemTools.ProcessTools;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;
using Microsoft.Win32;

namespace LeakBlocker.Libraries.SystemTools.Drivers
{
    internal sealed class DriverController : Disposable, IDriverController
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        private struct BaseMessage
        {
            private readonly NativeMethods.OVERLAPPED Overlapped;
            private readonly NativeMethods.FILTER_MESSAGE_HEADER MessageHeader;
            internal DriverStructures.MESSAGE_HEADER Header;
        }

        private const uint processingThreadsCount = 2;
        private const uint bufferSize = 1024 * 1024;

        private static readonly string driverName = "LeakBlockerFsDrv_" + SharedObjects.Constants.VersionString;
        private static readonly string path = SharedObjects.Constants.CurrentVersionProgramFilesFolder + driverName + ".sys";

        private readonly IntPtr communicationPortHandle;
        private readonly IntPtr completionPortHandle;

        private readonly List<Thread> processingThreads = new List<Thread>();
        private readonly ConcurrentBag<UnmanagedMemory> buffers = new ConcurrentBag<UnmanagedMemory>();

        private readonly IDriverControllerHandler eventHandler;

        private ReadOnlySet<VolumeName> lastVolumeNames;

        bool IDriverController.Available
        {
            get
            {
                using (var handle = new UnmanagedPointer())
                {
                    uint error = NativeMethods.FilterCreate(driverName, +handle);
                    if (error != NativeMethods.S_OK)
                    {
                        Log.Write(NativeErrors.GetMessage("FilterCreate", error, driverName));
                        return false;
                    }

                    error = NativeMethods.FilterClose(handle);
                    if (error != NativeMethods.S_OK)
                        Log.Write(NativeErrors.GetMessage("FilterClose", error));

                    return true;
                }
            }
        }

        public DriverController(IDriverControllerHandler eventHandler = null)
        {
            if (eventHandler == null)
                return;

            this.eventHandler = eventHandler;

            ProcessPrivileges.AddForCurrentProcess(ProcessPrivilege.LoadDriver);

            uint error = NativeMethods.FilterLoad(driverName);
            if ((NativeMethods.HRESULT_CODE(error) == NativeMethods.ERROR_ALREADY_EXISTS) || (NativeMethods.HRESULT_CODE(error) == NativeMethods.ERROR_SERVICE_ALREADY_RUNNING))
            {
                error = NativeMethods.FilterUnload(driverName);
                if (error != NativeMethods.S_OK)
                    NativeErrors.ThrowException("FilterUnload", error, driverName);
                error = NativeMethods.FilterLoad(driverName);
            }

            if (error != NativeMethods.S_OK)
                NativeErrors.ThrowException("FilterLoad", error, driverName);

            using (var handleBuffer = new UnmanagedPointer())
            {
                Log.Write("Initializing communication port.");
                string portName = "\\" + driverName;
                error = NativeMethods.FilterConnectCommunicationPort(portName, 0, IntPtr.Zero, 0, IntPtr.Zero, +handleBuffer);
                if (error != NativeMethods.S_OK)
                    NativeErrors.ThrowException("FilterConnectCommunicationPort", error, portName);
                communicationPortHandle = handleBuffer;

                if (communicationPortHandle == NativeMethods.INVALID_HANDLE_VALUE)
                    Exceptions.Throw(ErrorMessage.InvalidHandleState);

                Log.Write("Initializing completion port.");
                completionPortHandle = NativeMethods.CreateIoCompletionPort(communicationPortHandle, IntPtr.Zero, IntPtr.Zero, processingThreadsCount);
                if (completionPortHandle == IntPtr.Zero)
                {
                    error = NativeMethods.GetLastError();
                    Dispose();
                    NativeErrors.ThrowException("CreateIoCompletionPort", error);
                }

                Log.Write("Starting processing threads.");
                for (int i = 0; i < processingThreadsCount; i++)
                {
                    var buffer = new UnmanagedMemory(bufferSize);
                    buffers.Add(buffer);

                    var newThread = new Thread(MainThread)
                    {
                        IsBackground = true
                    };
                    processingThreads.Add(newThread);
                    newThread.Start(buffer);
                }
            }
        }

        public void SetManagedVolumes(IReadOnlyCollection<VolumeName> volumes)
        {
            Check.CollectionHasNoDefaultItems(volumes, "volumes");
            ThrowIfDisposed();

            var oldVolumes = new List<VolumeName>();

            using (var requiredSize = new UnmanagedInteger())
            using (var searchHandle = new UnmanagedPointer())
            {
                uint error = NativeMethods.FilterInstanceFindFirst(driverName, NativeMethods.INSTANCE_INFORMATION_CLASS.InstanceFullInformation, IntPtr.Zero, 0, +requiredSize, +searchHandle);
                if (error != NativeMethods.HRESULT_FROM_WIN32(NativeMethods.ERROR_NO_MORE_ITEMS))
                {
                    if (error != NativeMethods.HRESULT_FROM_WIN32(NativeMethods.ERROR_INSUFFICIENT_BUFFER))
                        NativeErrors.ThrowException("FilterInstanceFindFirst", error);

                    using (var firstEntryBuffer = new UnmanagedMemory(requiredSize))
                    {
                        error = NativeMethods.FilterInstanceFindFirst(driverName, NativeMethods.INSTANCE_INFORMATION_CLASS.InstanceFullInformation, +firstEntryBuffer, firstEntryBuffer.USize, +requiredSize, +searchHandle);
                        if (error != NativeMethods.S_OK)
                            NativeErrors.ThrowException("FilterInstanceFindFirst", error);

                        using (new FilterInstanceSearchHandleWrapper(searchHandle))
                        {
                            IntPtr currentBuffer = +firstEntryBuffer;

                            while (true)
                            {
                                using (var data = new UnmanagedStructure<NativeMethods.INSTANCE_FULL_INFORMATION>(currentBuffer))
                                {
                                    oldVolumes.Add(new VolumeName(data));
                                }

                                error = NativeMethods.FilterInstanceFindNext(searchHandle, NativeMethods.INSTANCE_INFORMATION_CLASS.InstanceFullInformation, IntPtr.Zero, 0, +requiredSize);
                                if (error == NativeMethods.HRESULT_FROM_WIN32(NativeMethods.ERROR_NO_MORE_ITEMS))
                                    break;

                                if (error != NativeMethods.HRESULT_FROM_WIN32(NativeMethods.ERROR_INSUFFICIENT_BUFFER))
                                    NativeErrors.ThrowException("FilterInstanceFindNext", error);

                                using (var nextEntryBuffer = new UnmanagedMemory(requiredSize))
                                {
                                    error = NativeMethods.FilterInstanceFindNext(searchHandle, NativeMethods.INSTANCE_INFORMATION_CLASS.InstanceFullInformation, +nextEntryBuffer, nextEntryBuffer.USize, +requiredSize);
                                    if (error != NativeMethods.S_OK)
                                        NativeErrors.ThrowException("FilterInstanceFindNext", error);

                                    currentBuffer = +nextEntryBuffer;
                                }
                            }
                        }
                    }
                }
            }

            IEnumerable<VolumeName> detachFromVolumes = oldVolumes.Without(volumes);
            IEnumerable<VolumeName> attachToVolumes = volumes.Without(oldVolumes);

            foreach(VolumeName volume in detachFromVolumes)
            {
                uint error = NativeMethods.FilterDetach(driverName, volume);
                if(error != NativeMethods.S_OK)
                    Log.Write(NativeErrors.GetMessage("FilterDetach", error, volume));
            }

            foreach (VolumeName volume in attachToVolumes)
            {
                uint error = NativeMethods.FilterAttach(driverName, volume, null, 0, IntPtr.Zero);
                if (error != NativeMethods.S_OK)
                    Log.Write(NativeErrors.GetMessage("FilterAttach", error, volume));
            }
        }

        public void SetInstanceConfiguration(long instanceIdentifier, ReadOnlyDictionary<AccountSecurityIdentifier, FileAccessType> rules)
        {
            SendMessage(new ConfigurationMessage(instanceIdentifier, rules));
        }
        
        protected override void DisposeUnmanaged()
        {
            if (eventHandler == null)
                return;

            Log.Write("Releasing driver controller.");

            if ((completionPortHandle != IntPtr.Zero) && !NativeMethods.CloseHandle(completionPortHandle))
                Log.Write(NativeErrors.GetLastErrorMessage("CloseHandle"));
            if (!NativeMethods.CloseHandle(communicationPortHandle))
                Log.Write(NativeErrors.GetLastErrorMessage("CloseHandle"));

            uint error = NativeMethods.FilterUnload(driverName);
            if (error != NativeMethods.S_OK)
                Log.Write(NativeErrors.GetMessage("FilterUnload", error, driverName));

            foreach (Thread thread in processingThreads)
                thread.Join();

            foreach (UnmanagedMemory currentItem in buffers)
                currentItem.Dispose();
        }
        
        private void SendMessage(IOutgoingMessage message)
        {
            Check.ObjectIsNotNull(message, "message");

            int type;

            if (message is ConfigurationMessage)
                type = DriverStructures.CONFIGURATION_MESSAGE_TYPE;
            else
            {
                Exceptions.Throw(ErrorMessage.UnsupportedType, "Unknown message type.");
                return;
            }

            byte[] data = message.GetBinaryData();

            using (var buffer = new UnmanagedMemory(data.Length))
            using (var header = new UnmanagedStructure<DriverStructures.MESSAGE_HEADER>(+buffer))
            {
                buffer.BinaryData = data;
                header.Value = new DriverStructures.MESSAGE_HEADER
                {
                    Protocol = DriverStructures.PROTOCOL_VERSION,
                    TotalSize = buffer.SSize,
                    Type = type
                };

                using (var requiredSizeBuffer = new UnmanagedInteger())
                {
                    uint error = NativeMethods.FilterSendMessage(communicationPortHandle, +buffer, buffer.USize, IntPtr.Zero, 0, +requiredSizeBuffer);
                    if (error != NativeMethods.S_OK)
                        NativeErrors.ThrowException("FilterGetMessage", error);
                }
            }
        }

        private void MainThread(object parameter)
        {
            try
            {
                var buffer = (UnmanagedMemory)parameter;

                IntPtr dataPointer = +buffer + (int)UnmanagedStructure<NativeMethods.OVERLAPPED>.GetSize();

                uint error = NativeMethods.FilterGetMessage(communicationPortHandle, dataPointer, bufferSize, +buffer);
                if ((error != NativeMethods.S_OK) && (NativeMethods.HRESULT_CODE(error) != NativeMethods.ERROR_IO_PENDING))
                {
                    Log.Write(NativeErrors.GetMessage("FilterGetMessage", error));
                    return;
                }

                using (var sizeBuffer = new UnmanagedInteger())
                using (var keyBuffer = new UnmanagedPointer())
                using (var messageBuffer = new UnmanagedPointer())
                {
                    while (true)
                    {
                        if (!NativeMethods.GetQueuedCompletionStatus(completionPortHandle, +sizeBuffer, +keyBuffer, +messageBuffer, NativeMethods.INFINITE))
                        {
                            if (!Disposed)
                                Log.Write(NativeErrors.GetLastErrorMessage("GetQueuedCompletionStatus"));
                            break;
                        }

                        using (var currentBuffer = new UnmanagedMemory(messageBuffer, bufferSize))
                        using (var baseMessage = new UnmanagedStructure<BaseMessage>(messageBuffer))
                        using (var message = new UnmanagedStructure<DriverStructures.MESSAGE_HEADER>(baseMessage.GetFieldAddress("Header")))
                        {
                            int headerOffset = UnmanagedStructure<BaseMessage>.GetFieldOffset("Header");
                            if (message.Value.TotalSize > (bufferSize - headerOffset))
                            {
                                Log.Write("Too much data.");
                                continue;
                            }

                            int version = message.Value.Protocol;
                            if (version != DriverStructures.PROTOCOL_VERSION)
                            {
                                Log.Write("Incorrect version. {0} {1}", version, DriverStructures.PROTOCOL_VERSION);
                                continue;
                            }

                            try
                            {
                                IntPtr address = +baseMessage + headerOffset;

                                using (var data = new UnmanagedMemory(address, message.Value.TotalSize))
                                {
                                    switch (baseMessage.Value.Header.Type)
                                    {
                                        case DriverStructures.FILE_ACCESS_NOTIFICATION_TYPE:
                                            eventHandler.FileNotificationMessageReceived(new FileNotificationMessage(data));
                                            break;
                                        case DriverStructures.VOLUME_DETACH_NOTIFICATION_TYPE:
                                            eventHandler.VolumeDetachMessageReceived(new VolumeDetachMessage(data));
                                            break;
                                        case DriverStructures.VOLUME_IDENTIFIER_NOTIFICATION_TYPE:
                                            eventHandler.VolumeAttachMessageReceived(new VolumeAttachMessage(data));
                                            break;
                                        case DriverStructures.VOLUME_LIST_UPDATE_NOTIFICATION_TYPE:
                                            ReadOnlySet<VolumeName> newVolumeNames = VolumeName.Enumerate();
                                            if (newVolumeNames != lastVolumeNames)
                                            {
                                                lastVolumeNames = newVolumeNames;
                                                eventHandler.VolumeListUpdateMessageReceived();
                                            }
                                            break;
                                        default:
                                            Log.Write("Unknown message type.");
                                            break;
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                Log.Write(exception);
                                continue;
                            }

                            currentBuffer.Clear();

                            dataPointer = +currentBuffer + (int)UnmanagedStructure<NativeMethods.OVERLAPPED>.GetSize();

                            error = NativeMethods.FilterGetMessage(communicationPortHandle, dataPointer, bufferSize, +currentBuffer);
                            if ((error != NativeMethods.S_OK) && (NativeMethods.HRESULT_CODE(error) != NativeMethods.ERROR_IO_PENDING))
                            {
                                Log.Write(NativeErrors.GetMessage("FilterGetMessage", error));
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex.ToString());
            }
        }

        void IDriverController.Install()
        {
            IList<byte> binary = Environment.Is64BitOperatingSystem ? Binaries.DriverBinary64 : Binaries.DriverBinary32;

            if((Environment.OSVersion.Version.Major * 10 + Environment.OSVersion.Version.Minor) < (Environment.Is64BitOperatingSystem ? 52 : 51))
                Exceptions.Throw(ErrorMessage.OldSystemVersion, "System version {0} is not supported.".Combine(Environment.OSVersion.Version));
                
            if (!SystemObjects.FileTools.Exists(path))
                SystemObjects.FileTools.WriteFile(path, binary.ToReadOnlyList());

            Log.Write("Creating driver service.");

            IWindowsService service = SystemObjects.CreateWindowsService(driverName);
            if (!service.Exists)
                service.Create(path, SystemServiceType.FileSystemDriver);
            
            service.SetDependencies(new[] { "FltMgr" }.ToReadOnlySet());

            Log.Write("Adding registry keys.");

            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\{0}\Instances".Combine(driverName), "DefaultInstance", "{0} Instance".Combine(driverName));
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\{0}\Instances\{0} Instance".Combine(driverName), "Altitude", "173692");
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\{0}\Instances\{0} Instance".Combine(driverName), "Flags", 0);
        }

        void IDriverController.Remove()
        {
            uint error = NativeMethods.FilterUnload(driverName);
            if (error != NativeMethods.S_OK)
                Log.Write(NativeErrors.GetMessage("FilterUnload", error));
            
            IWindowsService service = SystemObjects.CreateWindowsService(driverName);
            SharedObjects.ExceptionSuppressor.Run(service.Delete);
            SharedObjects.ExceptionSuppressor.Run(() => SystemObjects.FileTools.Delete(path));
        }
    }
}
