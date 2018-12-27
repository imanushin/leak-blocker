using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Devices.Management
{
    #region DeviceTreeNode

    internal sealed class DeviceTreeNode : BaseReadOnlyObject
    {
        internal NativeMethods.SP_DEVINFO_DATA Data
        {
            get;
            private set;
        }

        internal int Instance
        {
            get
            {
                return unchecked((int)Data.DevInst);
            }
        }

        internal Guid ClassIdentifier
        {
            get
            {
                return Data.ClassGuid;
            }
        }

        public ReadOnlySet<DeviceTreeNode> ChildDevices
        {
            get;
            private set;
        }

        public ReadOnlySet<DeviceTreeNode> AllDevices
        {
            get
            {
                return new [] { this }.Union(ChildDevices.SelectMany(item => item.AllDevices)).ToReadOnlySet();
            }
        }

        internal DeviceTreeNode(NativeMethods.SP_DEVINFO_DATA data, IReadOnlyCollection<DeviceTreeNode> childDevices)
        {
            Data = data;
            ChildDevices = childDevices.ToReadOnlySet();
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Instance;
        }

        #endregion
    }

    #endregion

    #region DeviceProperty

    internal enum DeviceProperty : uint
    {
        PhysicalDeviceObjectName = NativeMethods.SPDRP_PHYSICAL_DEVICE_OBJECT_NAME,
        EnumeratorName = NativeMethods.SPDRP_ENUMERATOR_NAME,
        ClassName = NativeMethods.SPDRP_CLASS,
        Manufacturer = NativeMethods.SPDRP_MFG,
        DeviceDescription = NativeMethods.SPDRP_DEVICEDESC,
        FriendlyName = NativeMethods.SPDRP_FRIENDLYNAME,
        Capabilities = NativeMethods.SPDRP_CAPABILITIES,
        HardwareIdentifier = NativeMethods.SPDRP_HARDWAREID,
        Service = NativeMethods.SPDRP_SERVICE 
    }

    #endregion

    internal static class DeviceManagement
    {
        private static readonly Dictionary<string, SystemDeviceType> classTypes = new Dictionary<string, SystemDeviceType>
        {
            #region Values

            { "E0CBF06C-CD8B-4647-BB8A-263B43F0F974", SystemDeviceType.Bluetooth },
            { "4D36E965-E325-11CE-BFC1-08002BE10318", SystemDeviceType.OpticalDrive },
            { "4D36E967-E325-11CE-BFC1-08002BE10318", SystemDeviceType.DiskDrive },
            { "4D36E980-E325-11CE-BFC1-08002BE10318", SystemDeviceType.FloppyDisk },
            { "48721B56-6795-11D2-B1A8-0080C72E74A2", SystemDeviceType.Dot4 },
            { "49CE6AC8-6F86-11D2-B1E5-0080C72E74A2", SystemDeviceType.Dot4Print },
            { "6BDD1FC5-810F-11D0-BEC7-08002BE2092F", SystemDeviceType.Infrared },
            { "4D36E96D-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Modem },
            { "4D36E972-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Net },
            { "4D36E978-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Ports },
            { "4D36E979-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Printer },
            { "4658EE7E-F050-11D1-B6BD-00C04FA372A7", SystemDeviceType.PlugAndPlayPrinters },
            { "6D807884-7D21-11CF-801C-08002BE10318", SystemDeviceType.TapeDrive },
            { "36FC9E60-C465-11CF-8056-444553540000", SystemDeviceType.UniversalSerialBus },
            { "88BAE032-5A81-49F0-BC3D-A4FF138216D6", SystemDeviceType.UniversalSerialBusDevice },
            { "25DBCE51-6C8F-4A72-8A6D-B54C2B4FC835", SystemDeviceType.WindowsCeActiveSync },
            { "EEC5AD98-8080-425F-922A-DABF3DE3F69A", SystemDeviceType.WindowsPortableDevice },
            { "6BDD1FC6-810F-11D0-BEC7-08002BE2092F", SystemDeviceType.Image },
            { "4D36E96C-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Media },
            { "7EBEFBC0-3200-11D2-B4C2-00A0C9697D07", SystemDeviceType.Iec61883 },
            { "C06FF265-AE09-48F0-812C-16753D7CBA83", SystemDeviceType.AdvancedVideoCoding },
            { "D48179BE-EC20-11D1-B6B8-00C04FA372A7", SystemDeviceType.SerialBusProtocol2 },
            { "4D36E971-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Multifunction },
            { "72631E54-78A4-11D0-BCF7-00AA00B7B32A", SystemDeviceType.Battery },
            { "53D29EF7-377C-4D14-864B-EB3A85769359", SystemDeviceType.Biometric },
            { "4D36E968-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Display },
            { "4D36E969-E325-11CE-BFC1-08002BE10318", SystemDeviceType.FloppyDiskController },
            { "4D36E96A-E325-11CE-BFC1-08002BE10318", SystemDeviceType.HardDiskController },
            { "745A17A0-74D3-11D0-B6FE-00A0C90F57DA", SystemDeviceType.HidClass },
            { "6BDD1FC1-810F-11D0-BEC7-08002BE2092F", SystemDeviceType.Ieee1394 },
            { "4D36E96B-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Keyboard },
            { "CE5939AE-EBDE-11D0-B181-0000F8753EC4", SystemDeviceType.MediumChanger },
            { "4D36E970-E325-11CE-BFC1-08002BE10318", SystemDeviceType.MemoryTechnologyDevice },
            { "4D36E96E-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Monitor },
            { "4D36E96F-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Mouse },
            { "50906CB8-BA12-11D1-BF5D-0000F805F530", SystemDeviceType.MultiportSerial },
            { "4D36E973-E325-11CE-BFC1-08002BE10318", SystemDeviceType.NetworkClient },
            { "4D36E974-E325-11CE-BFC1-08002BE10318", SystemDeviceType.NetworkService },
            { "4D36E975-E325-11CE-BFC1-08002BE10318", SystemDeviceType.NetworkTransport },
            { "268C95A1-EDFE-11D3-95C3-0010DC4050A5", SystemDeviceType.SecurityAccelerator },
            { "4D36E977-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Pcmcia },
            { "50127DC3-0F36-415E-A6CC-4CB3BE910B65", SystemDeviceType.Processor },
            { "4D36E97B-E325-11CE-BFC1-08002BE10318", SystemDeviceType.ScsiAdapter },
            { "5175D334-C371-4806-B3BA-71FD53C9258D", SystemDeviceType.Sensor },
            { "50DD5230-BA8A-11D1-BF5D-0000F805F530", SystemDeviceType.SmartcardReader },
            { "71A27CDD-812A-11D0-BEC7-08002BE2092F", SystemDeviceType.Volume },
            { "4D36E97D-E325-11CE-BFC1-08002BE10318", SystemDeviceType.System },
            { "997B5D8D-C442-4F2E-BAF3-9C8E671E9E21", SystemDeviceType.SideShow },
            { "4D36E964-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Adapter },
            { "D45B1C18-C8FA-11D1-9F77-0000F805F530", SystemDeviceType.AdvancedPowerManagement },
            { "4D36E966-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Computer },
            { "6BDD1FC2-810F-11D0-BEC7-08002BE2092F", SystemDeviceType.Decoders },
            { "6BDD1FC3-810F-11D0-BEC7-08002BE2092F", SystemDeviceType.GlobalPositioningSystem },
            { "66F250D6-7801-4A64-B139-EEA80A450B24", SystemDeviceType.Ieee1394Debugger },
            { "C459DF55-DB08-11D1-B009-00A0C9081FF6", SystemDeviceType.Ieee1394NetworkEnumerator },
            { "4D36E976-E325-11CE-BFC1-08002BE10318", SystemDeviceType.NoDriver },
            { "8ECC055D-047F-11D1-A537-0000F8753ED1", SystemDeviceType.LegacyDriver },
            { "4D36E97E-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Unknown },
            { "4D36E97A-E325-11CE-BFC1-08002BE10318", SystemDeviceType.PrinterUpgrade },
            { "4D36E97C-E325-11CE-BFC1-08002BE10318", SystemDeviceType.Sound },
            { "533C5B84-EC70-11D2-9505-00C04F79DEAF", SystemDeviceType.VolumeSnapshot }

            #endregion
        };

        internal static SystemDeviceType GetDeviceClassType(DeviceTreeNode node)
        {
            SystemDeviceType result;
            if (!classTypes.TryGetValue(node.ClassIdentifier.ToString().ToUpperInvariant(), out result))
                result = SystemDeviceType.Unsupported;
            return result;
        }

        internal static DeviceOptions GetDeviceOptions(DeviceManagementHandle handle, DeviceTreeNode node)
        {
            var capabilities = GetRegistryProperty<uint>(handle, node, DeviceProperty.Capabilities);

            using (var status = new UnmanagedInteger())
            using (var problem = new UnmanagedInteger())
            {
                uint returnCode = NativeMethods.CM_Get_DevNode_Status_Ex(+status, +problem, node.Data.DevInst, 0, handle.MachineHandle);
                if (returnCode == NativeMethods.CR_NO_SUCH_DEVNODE)
                {
                    //Log.Add("Cannot find device node.");
                    status.UValue = 0;
                }
                else if (returnCode != NativeMethods.CR_SUCCESS)
                    Exceptions.Throw(ErrorMessage.DeviceNotAvailable, "CM_Get_DevNode_Status_Ex failed with error {0}.".Combine(returnCode));

                return (DeviceOptions)(status | ((ulong)capabilities << 32));
            }
        }

        internal static DeviceError GetDeviceError(DeviceManagementHandle handle, DeviceTreeNode node)
        {
            using (var status = new UnmanagedInteger())
            using (var problem = new UnmanagedInteger())
            {
                uint returnCode = NativeMethods.CM_Get_DevNode_Status_Ex(+status, +problem, node.Data.DevInst, 0, handle.MachineHandle);
                if (returnCode == NativeMethods.CR_NO_SUCH_DEVNODE)
                {
                    //Log.Add("Cannot find device node.");
                    return DeviceError.None;
                }
                if (returnCode != NativeMethods.CR_SUCCESS)
                    Exceptions.Throw(ErrorMessage.DeviceNotAvailable, "CM_Get_DevNode_Status_Ex failed with error {0}.".Combine(returnCode));

                return (DeviceError)(status.HasFlag(NativeMethods.DN_HAS_PROBLEM) ? (uint)problem : 0);
            }
        }

        internal static string GetDeviceInstanceIdentifier(DeviceManagementHandle handle, DeviceTreeNode node)
        {
            using (var deviceInformationDataBuffer = new UnmanagedStructure<NativeMethods.SP_DEVINFO_DATA>(node.Data))
            using (var requiredSize = new UnmanagedInteger())
            {
                uint error = 0;
                if (!NativeMethods.SetupDiGetDeviceInstanceId(handle.DeviceInformationSet, +deviceInformationDataBuffer, IntPtr.Zero, 0, +requiredSize))
                    error = NativeMethods.GetLastError();

                if ((error != NativeMethods.ERROR_SUCCESS) && (error != NativeMethods.ERROR_INSUFFICIENT_BUFFER))
                    NativeErrors.ThrowException("SetupDiGetDeviceInstanceId", error, node.Data.DevInst, node.Data.ClassGuid);

                using (var buffer = new UnmanagedUnicodeString(requiredSize))
                {
                    if (!NativeMethods.SetupDiGetDeviceInstanceId(handle.DeviceInformationSet, +deviceInformationDataBuffer, +buffer, buffer.ULength, IntPtr.Zero))
                        NativeErrors.ThrowLastErrorException("SetupDiGetDeviceInstanceId", node.Data.DevInst, node.Data.ClassGuid);

                    return buffer;
                }
            }
        }

        internal static T GetRegistryProperty<T>(DeviceManagementHandle handle, DeviceTreeNode node, DeviceProperty property, bool ignoreOffline = false)
        {
            object result;

            using (var requiredSize = new UnmanagedInteger())
            using (var deviceInformationDataBuffer = new UnmanagedStructure<NativeMethods.SP_DEVINFO_DATA>(node.Data))
            using (var dataType = new UnmanagedInteger())
            {
                uint error = 0;
                if (!NativeMethods.SetupDiGetDeviceRegistryProperty(handle.DeviceInformationSet, +deviceInformationDataBuffer, (uint)property, +dataType, IntPtr.Zero, 0, +requiredSize))
                    error = NativeMethods.GetLastError();

                if ((error != 0) && (error != NativeMethods.ERROR_INSUFFICIENT_BUFFER))
                {
                    if (error == NativeMethods.ERROR_INVALID_DATA)
                        return default(T);
                    if (ignoreOffline && (error == NativeMethods.ERROR_NO_SUCH_DEVINST))
                        return default(T);

                    NativeErrors.ThrowLastErrorException("SetupDiGetDeviceRegistryProperty", node.Data.DevInst, node.Data.ClassGuid);
                }
                
                using (var buffer = new UnmanagedMemory(requiredSize))
                {
                    if (!NativeMethods.SetupDiGetDeviceRegistryProperty(handle.DeviceInformationSet, +deviceInformationDataBuffer,
                        (uint)property, +dataType, +buffer, buffer.USize, +requiredSize))
                    {
                        NativeErrors.ThrowLastErrorException("SetupDiGetDeviceRegistryProperty", node.Data.DevInst, node.Data.ClassGuid);
                    }

                    switch ((uint)dataType)
                    {
                        case NativeMethods.REG_BINARY:
                            result = (requiredSize == 0) ? new byte[0] : buffer.BinaryData;
                            break;
                        case NativeMethods.REG_DWORD:
                            result = (requiredSize == 0) ? 0 : BitConverter.ToUInt32(buffer.BinaryData, 0);
                            break;
                        case NativeMethods.REG_SZ:
                        case NativeMethods.REG_EXPAND_SZ:
                            result = (requiredSize == 0) ? string.Empty : StringTools.FromPointer(+buffer);
                            break;
                        case NativeMethods.REG_MULTI_SZ:
                            result = (requiredSize == 0) ? new List<string>().AsReadOnly() : StringTools.FromPointerDoubleNullTerminated(+buffer);
                            break;
                        default:
                            Exceptions.Throw(ErrorMessage.UnsupportedType, "Unsupported registry data type ({0}).".Combine(dataType));
                            return default(T);
                    }
                }
            }

            if (!(result is T))
                Exceptions.Throw(ErrorMessage.UnsupportedType, "Property {1} type is {0}.".Combine(property.GetType().Name, property));

            return (T)result;
        }

        internal static ReadOnlySet<DeviceTreeNode> EnumerateDevices(DeviceManagementHandle handle)
        {
            using (var deviceInformationDataBuffer = new UnmanagedStructure<NativeMethods.SP_DEVINFO_DATA>())
            {
                var devices = new HashSet<NativeMethods.SP_DEVINFO_DATA>();

                uint deviceNumber = 0;
                while (true)
                {
                    deviceInformationDataBuffer.Value = new NativeMethods.SP_DEVINFO_DATA
                    {
                        cbSize = deviceInformationDataBuffer.USize
                    };

                    if (!NativeMethods.SetupDiEnumDeviceInfo(handle.DeviceInformationSet, deviceNumber, +deviceInformationDataBuffer))
                    {
                        uint error = NativeMethods.GetLastError();
                        if (error != NativeMethods.ERROR_NO_MORE_ITEMS)
                            NativeErrors.ThrowException("SetupDiEnumDeviceInfo", error);
                        else
                            break;
                    }

                    if (!devices.Add(deviceInformationDataBuffer.Value))
                        Log.Write("Device {0} was already added.".Combine(deviceInformationDataBuffer.Value.DevInst));

                    deviceNumber++;
                }

                var rootDevices = new HashSet<NativeMethods.SP_DEVINFO_DATA>();
                var childDeviceLists = new Dictionary<NativeMethods.SP_DEVINFO_DATA, HashSet<NativeMethods.SP_DEVINFO_DATA>>();

                foreach (NativeMethods.SP_DEVINFO_DATA currentItem in devices)
                {
                    using (var parentInstance = new UnmanagedInteger())
                    {
                        NativeMethods.SP_DEVINFO_DATA? parent = null;
                        
                        uint error = NativeMethods.CM_Get_Parent_Ex(+parentInstance, currentItem.DevInst, 0, handle.MachineHandle);
                        if (error != NativeMethods.CR_SUCCESS)
                        {
                            //if(error == NativeMethods.CR_INVALID_DEVNODE)
                            //    Log.Write("CM_Get_Parent_Ex failed with error {0}.".Combine(error));
                            //else 
                            if (error != NativeMethods.CR_NO_SUCH_DEVNODE)
                                Exceptions.Throw(ErrorMessage.DeviceNotAvailable, "CM_Get_Parent_Ex failed with error {0}.".Combine(error));
                        }
                        else
                        {
                            uint parentInstanceValue = parentInstance.UValue;
                            ReadOnlySet<NativeMethods.SP_DEVINFO_DATA> parentDevices = devices.Where(data => data.DevInst == parentInstanceValue).ToReadOnlySet();

                            if (parentDevices.Any())
                                parent = parentDevices.FirstOrDefault();
                        }

                        if (!parent.HasValue)
                            rootDevices.Add(currentItem);
                        else
                        {
                            if (!childDeviceLists.ContainsKey(parent.Value))
                                childDeviceLists[parent.Value] = new HashSet<NativeMethods.SP_DEVINFO_DATA>();

                            childDeviceLists[parent.Value].Add(currentItem);
                        }
                    }
                }

                Func<NativeMethods.SP_DEVINFO_DATA, DeviceTreeNode> createNode = null;
                createNode = data => new DeviceTreeNode(data, childDeviceLists.ContainsKey(data) ? childDeviceLists[data].Select(createNode).ToReadOnlySet() : ReadOnlySet<DeviceTreeNode>.Empty);

                return rootDevices.Select(data => createNode(data)).SelectMany(node => node.AllDevices).ToReadOnlySet();
            }
        }

        internal static ReadOnlySet<VolumeName> GetDeviceDisks(DeviceManagementHandle handle, DeviceTreeNode node)
        {
            Check.ObjectIsNotNull(handle, "handle");

            if (GetDeviceClassType(node) != SystemDeviceType.DiskDrive)
                return ReadOnlySet<VolumeName>.Empty;
            
            string physicalDeviceObjectName = GetRegistryProperty<string>(handle, node, DeviceProperty.PhysicalDeviceObjectName, true) ?? string.Empty;

            if (string.IsNullOrEmpty(physicalDeviceObjectName))
                return ReadOnlySet<VolumeName>.Empty;

            var result = new HashSet<VolumeName>();

            var deviceNumbers = new Dictionary<VolumeName, uint>();

            foreach (VolumeName currentVolume in VolumeName.Enumerate())
            {
                using (var objectName = new UnmanagedStructure<NativeMethods.UNICODE_STRING>())
                using (var volumeHandle = new UnmanagedPointer())
                using (var statusBlock = new UnmanagedStructure<NativeMethods.IO_STATUS_BLOCK>())
                using (var attributes = new UnmanagedStructure<NativeMethods.OBJECT_ATTRIBUTES>())
                {
                    NativeMethods.RtlInitUnicodeString(+objectName, currentVolume);
                    attributes.Value = new NativeMethods.OBJECT_ATTRIBUTES
                    {
                        Length = attributes.USize,
                        ObjectName = +objectName
                    };

                    uint status = NativeMethods.NtOpenFile(+volumeHandle, NativeMethods.GENERIC_ALL, +attributes, +statusBlock,
                        NativeMethods.FILE_SHARE_DELETE | NativeMethods.FILE_SHARE_READ | NativeMethods.FILE_SHARE_WRITE, 0);

                    if (status != NativeMethods.STATUS_SUCCESS)
                        continue;

                    using (new NtHandleWrapper(volumeHandle))
                    using (var storageDeviceNumber = new UnmanagedStructure<NativeMethods.STORAGE_DEVICE_NUMBER>())
                    using (var returnedSize = new UnmanagedInteger())
                    {
                        if (!NativeMethods.DeviceIoControl(volumeHandle, NativeMethods.IOCTL_STORAGE_GET_DEVICE_NUMBER, IntPtr.Zero, 0,
                            +storageDeviceNumber, storageDeviceNumber.USize, +returnedSize, IntPtr.Zero))
                        {
                            if (NativeMethods.GetLastError() != NativeMethods.ERROR_INVALID_FUNCTION)
                                Log.Write(NativeErrors.GetLastErrorMessage("DeviceIoControl", currentVolume));
                        }
                        else
                            deviceNumbers.Add(currentVolume, storageDeviceNumber.Value.DeviceNumber);
                    }
                }
            }

            using (var objectName = new UnmanagedStructure<NativeMethods.UNICODE_STRING>())
            using (var deviceHandle = new UnmanagedPointer())
            using (var statusBlock = new UnmanagedStructure<NativeMethods.IO_STATUS_BLOCK>())
            using (var attributes = new UnmanagedStructure<NativeMethods.OBJECT_ATTRIBUTES>())
            {
                NativeMethods.RtlInitUnicodeString(+objectName, physicalDeviceObjectName);
                attributes.Value = new NativeMethods.OBJECT_ATTRIBUTES
                {
                    Length = attributes.USize,
                    ObjectName = +objectName
                };

                uint status = NativeMethods.NtOpenFile(+deviceHandle, NativeMethods.GENERIC_ALL, +attributes, +statusBlock,
                    NativeMethods.FILE_SHARE_DELETE | NativeMethods.FILE_SHARE_READ | NativeMethods.FILE_SHARE_WRITE, 0);

                if (status != NativeMethods.STATUS_SUCCESS)
                {
                    Log.Write(NativeErrors.GetLastErrorMessage("NtOpenFile", physicalDeviceObjectName));
                    return ReadOnlySet<VolumeName>.Empty;
                }

                using (new NtHandleWrapper(deviceHandle))
                using (var returnedSize = new UnmanagedInteger())
                using (var storageDeviceNumber = new UnmanagedStructure<NativeMethods.STORAGE_DEVICE_NUMBER>())
                {
                    if (!NativeMethods.DeviceIoControl(deviceHandle, NativeMethods.IOCTL_STORAGE_GET_DEVICE_NUMBER, IntPtr.Zero, 0,
                        +storageDeviceNumber, storageDeviceNumber.USize, +returnedSize, IntPtr.Zero))
                    {
                        Log.Write(NativeErrors.GetLastErrorMessage("DeviceIoControl", physicalDeviceObjectName));
                    }
                    else
                    {
                        uint number = storageDeviceNumber.Value.DeviceNumber;
                        deviceNumbers.Where(currentItem => currentItem.Value == number).ForEach(currentItem => result.Add(currentItem.Key));
                    }
                }
            }

            return result.ToReadOnlySet();
        }

        internal static bool SetEnabledState(DeviceManagementHandle handle, string deviceInstanceIdentifier, bool enabled, bool offline)
        {
            Check.StringIsMeaningful(deviceInstanceIdentifier, "deviceInstanceIdentifier");

            using (var deviceInstallParametersBuffer = new UnmanagedStructure<NativeMethods.SP_DEVINSTALL_PARAMS>())
            using (var propertyChangeParametersBuffer = new UnmanagedStructure<NativeMethods.SP_PROPCHANGE_PARAMS>())
            using (var deviceInformationDataBuffer = new UnmanagedStructure<NativeMethods.SP_DEVINFO_DATA>())
            {
                deviceInformationDataBuffer.Value = new NativeMethods.SP_DEVINFO_DATA
                {
                    cbSize = deviceInformationDataBuffer.USize
                };

                if (!NativeMethods.SetupDiOpenDeviceInfo(handle.DeviceInformationSet, deviceInstanceIdentifier, IntPtr.Zero, 0, +deviceInformationDataBuffer))
                    NativeErrors.ThrowLastErrorException("SetupDiOpenDeviceInfo", deviceInstanceIdentifier);

                propertyChangeParametersBuffer.Value = new NativeMethods.SP_PROPCHANGE_PARAMS
                {
                    ClassInstallHeader = new NativeMethods.SP_CLASSINSTALL_HEADER
                    {
                        cbSize = UnmanagedStructure<NativeMethods.SP_CLASSINSTALL_HEADER>.GetSize(),
                        InstallFunction = NativeMethods.DI_FUNCTION.DIF_PROPERTYCHANGE
                    },
                    HwProfile = 0,
                    Scope = NativeMethods.DICS_FLAG_GLOBAL,
                    StateChange = (enabled ? NativeMethods.DICS_ENABLE : NativeMethods.DICS_DISABLE)
                };

                if (!NativeMethods.SetupDiSetClassInstallParams(handle.DeviceInformationSet, +deviceInformationDataBuffer,
                    +propertyChangeParametersBuffer, propertyChangeParametersBuffer.USize))
                {
                    NativeErrors.ThrowLastErrorException("SetupDiSetClassInstallParams", deviceInstanceIdentifier);
                }

                if (!NativeMethods.SetupDiCallClassInstaller(NativeMethods.DI_FUNCTION.DIF_PROPERTYCHANGE, handle.DeviceInformationSet, +deviceInformationDataBuffer))
                {
                    uint error = NativeMethods.GetLastError();
                    if (offline && (error == NativeMethods.ERROR_NO_SUCH_DEVINST))
                        return true;
                    Log.Write(NativeErrors.GetLastErrorMessage("SetupDiCallClassInstaller", deviceInstanceIdentifier));
                    return false;
                }

                deviceInstallParametersBuffer.Value = new NativeMethods.SP_DEVINSTALL_PARAMS
                {
                    cbSize = UnmanagedStructure<NativeMethods.SP_DEVINSTALL_PARAMS>.GetSize()
                };

                if (!NativeMethods.SetupDiGetDeviceInstallParams(handle.DeviceInformationSet, +deviceInformationDataBuffer, +deviceInstallParametersBuffer))
                    NativeErrors.ThrowLastErrorException("SetupDiGetDeviceInstallParams", deviceInstanceIdentifier);

                bool rebootRequired = ((deviceInstallParametersBuffer.Value.Flags & NativeMethods.DI_NEEDREBOOT) != 0);
                if (rebootRequired)
                    Log.Write("Device {0} requires reboot.", deviceInstanceIdentifier);
                return !rebootRequired;
            }
        }
    }
}
