using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.ProcessTools.Implementations
{
    #region ServiceConfiguration

    internal struct ServiceConfiguration
    {
        public uint ServiceType
        {
            get;
            set;
        }

        public uint StartType
        {
            get;
            set;
        }

        public uint ErrorControl
        {
            get;
            set;
        }

        public string CommandLine
        {
            get;
            set;
        }

        public string LoadOrderGroup
        {
            get;
            set;
        }

        public ReadOnlySet<string> Dependencies
        {
            get;
            set;
        }

        public string DisplayedName
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public uint FailureCounterResetInterval
        {
            get;
            set;
        }

        public string FailureCommand
        {
            get;
            set;
        }

        public string FailureRebootMessage
        {
            get;
            set;
        }

        public NativeMethods.SC_ACTION[] FailureActions
        {
            get;
            set;
        }
    }

    #endregion

    internal static class ServiceManagement
    {
        #region ServiceControlManagerHandle

        private sealed class ServiceControlManagerHandle : Disposable
        {
            private readonly IntPtr handle;

            public ServiceControlManagerHandle(uint access, string machine = null)
            {
                handle = NativeMethods.OpenSCManager(machine, null, access);
                if (handle == IntPtr.Zero)
                    NativeErrors.ThrowLastErrorException("OpenSCManager", machine);
            }

            protected override void DisposeUnmanaged()
            {
                if ((handle != IntPtr.Zero) && !NativeMethods.CloseServiceHandle(handle))
                    Log.Write(NativeErrors.GetLastErrorMessage("CloseServiceHandle"));
            }

            public static implicit operator IntPtr(ServiceControlManagerHandle value)
            {
                Check.ObjectIsNotNull(value, "value");

                value.ThrowIfDisposed();
                return value.handle;
            }
        }

        #endregion

        #region ServiceHandle

        private sealed class ServiceHandle : Disposable
        {
            private readonly IntPtr handle;

            public ServiceHandle(ServiceControlManagerHandle manager, uint access, string serviceName)
            {
                Check.ObjectIsNotNull(manager, "manager");
                Check.StringIsMeaningful(serviceName, "serviceName");

                handle = NativeMethods.OpenService(manager, serviceName, access);
                if (handle == IntPtr.Zero)
                    NativeErrors.ThrowLastErrorException("OpenSCManager", serviceName);
            }

            public ServiceHandle(ServiceControlManagerHandle manager, uint access, string serviceName, ServiceConfiguration configuration)
            {
                Check.ObjectIsNotNull(manager, "manager");
                Check.StringIsMeaningful(serviceName, "serviceName");

                handle = NativeMethods.CreateService(manager, serviceName, configuration.DisplayedName, access, configuration.ServiceType,
                    configuration.StartType, configuration.ErrorControl, configuration.CommandLine, configuration.LoadOrderGroup, IntPtr.Zero,
                    (configuration.Dependencies == null) ? null : configuration.Dependencies.ToDoubleNullTerminated(), configuration.UserName, configuration.Password);

                if (handle == IntPtr.Zero)
                    NativeErrors.ThrowLastErrorException("CreateService", serviceName);
            }

            public static bool Exists(ServiceControlManagerHandle manager, string serviceName)
            {
                IntPtr handle = NativeMethods.OpenService(manager, serviceName, NativeMethods.SERVICE_QUERY_STATUS);

                if (handle != IntPtr.Zero)
                {
                    if (!NativeMethods.CloseServiceHandle(handle))
                        Log.Write(NativeErrors.GetLastErrorMessage("CloseServiceHandle"));
                    return true;
                }

                uint error = NativeMethods.GetLastError();
                if (error == NativeMethods.ERROR_SERVICE_DOES_NOT_EXIST)
                    return false;

                NativeErrors.ThrowLastErrorException("OpenService", serviceName);
                return false;
            }

            protected override void DisposeUnmanaged()
            {
                if ((handle != IntPtr.Zero) && !NativeMethods.CloseServiceHandle(handle))
                    Log.Write(NativeErrors.GetLastErrorMessage("CloseServiceHandle"));
            }

            public static implicit operator IntPtr(ServiceHandle value)
            {
                Check.ObjectIsNotNull(value, "value");

                value.ThrowIfDisposed();
                return value.handle;
            }
        }

        #endregion

        internal static bool Exists(string name, string machine = null)
        {
            Check.StringIsMeaningful(name, "name");

            using (var serviceControlManager = new ServiceControlManagerHandle(NativeMethods.SC_MANAGER_CONNECT, machine))
            {
                return ServiceHandle.Exists(serviceControlManager, name);
            }
        }

        internal static void CreateService(string name, ServiceConfiguration configuration, string machine = null)
        {
            Check.StringIsMeaningful(name, "name");

            using (var serviceControlManager = new ServiceControlManagerHandle(NativeMethods.SC_MANAGER_CONNECT | NativeMethods.SC_MANAGER_CREATE_SERVICE, machine))
            using (new ServiceHandle(serviceControlManager, 0, name, configuration))
            {
            }
            SetServiceConfiguration(name, configuration, machine);
        }

        internal static ServiceConfiguration GetServiceConfiguration(string name, string machine = null)
        {
            Check.StringIsMeaningful(name, "name");

            using (var serviceControlManager = new ServiceControlManagerHandle(NativeMethods.SC_MANAGER_CONNECT, machine))
            using (var serviceHandle = new ServiceHandle(serviceControlManager, NativeMethods.SERVICE_QUERY_CONFIG, name))
            using (var dataBuffer = new UnmanagedMemory(8192))
            using (var configuration = new UnmanagedStructure<NativeMethods.QUERY_SERVICE_CONFIG>(+dataBuffer))
            using (var requiredSize = new UnmanagedInteger())
            using (var descriptionStructure = new UnmanagedStructure<NativeMethods.SERVICE_DESCRIPTION>(+dataBuffer))
            using (var failureActions = new UnmanagedStructure<NativeMethods.SERVICE_FAILURE_ACTIONS>(+dataBuffer))
            {
                if (!NativeMethods.QueryServiceConfig(serviceHandle, +dataBuffer, dataBuffer.USize, +requiredSize))
                    NativeErrors.ThrowLastErrorException("QueryServiceConfig", name, machine);

                dataBuffer.Clear();
                requiredSize.Clear();

                if (!NativeMethods.QueryServiceConfig2(serviceHandle, NativeMethods.SERVICE_CONFIG_DESCRIPTION, +dataBuffer, dataBuffer.USize, +requiredSize))
                    NativeErrors.ThrowLastErrorException("QueryServiceConfig2", name, machine);

                var result = new ServiceConfiguration
                {
                    ErrorControl = configuration.Value.dwErrorControl,
                    ServiceType = configuration.Value.dwServiceType,
                    StartType = configuration.Value.dwStartType
                };

                if (configuration.Value.lpBinaryPathName != IntPtr.Zero)
                    result.CommandLine = StringTools.FromPointer(configuration.Value.lpBinaryPathName);

                if (configuration.Value.lpDependencies != IntPtr.Zero)
                    result.Dependencies = StringTools.FromPointerDoubleNullTerminated(configuration.Value.lpDependencies).ToReadOnlySet();

                if (configuration.Value.lpDisplayName != IntPtr.Zero)
                    result.DisplayedName = StringTools.FromPointer(configuration.Value.lpDisplayName);

                if (configuration.Value.lpLoadOrderGroup != IntPtr.Zero)
                    result.LoadOrderGroup = StringTools.FromPointer(configuration.Value.lpLoadOrderGroup);

                if (configuration.Value.lpServiceStartName != IntPtr.Zero)
                    result.UserName = StringTools.FromPointer(configuration.Value.lpServiceStartName);

                if (descriptionStructure.Value.lpDescription != IntPtr.Zero)
                    result.Description = StringTools.FromPointer(descriptionStructure.Value.lpDescription);

                dataBuffer.Clear();
                requiredSize.Clear();

                if (!NativeMethods.QueryServiceConfig2(serviceHandle, NativeMethods.SERVICE_CONFIG_FAILURE_ACTIONS, +dataBuffer, dataBuffer.USize, +requiredSize))
                    NativeErrors.ThrowLastErrorException("QueryServiceConfig2", name, machine);

                if (failureActions.Value.lpCommand != IntPtr.Zero)
                    result.FailureCommand = StringTools.FromPointer(failureActions.Value.lpCommand) ?? string.Empty;

                if (failureActions.Value.lpRebootMsg != IntPtr.Zero)
                    result.FailureRebootMessage = StringTools.FromPointer(failureActions.Value.lpRebootMsg) ?? string.Empty;

                result.FailureActions = new NativeMethods.SC_ACTION[0];
                if (failureActions.Value.cActions > 0)
                {
                    using (var actionsArray = new UnmanagedArray<NativeMethods.SC_ACTION>(failureActions.Value.lpsaActions, failureActions.Value.cActions))
                    {
                        result.FailureActions = actionsArray.Value;
                    }
                }

                return result;
            }
        }

        internal static void SetServiceConfiguration(string name, ServiceConfiguration configuration, string machine = null)
        {
            Check.StringIsMeaningful(name, "name");

            bool emptyActions = ((configuration.FailureActions == null) || (configuration.FailureActions.Length == 0));

            NativeMethods.SC_ACTION[] checkedActions = configuration.FailureActions;

            if (emptyActions)
                checkedActions = new[] { new NativeMethods.SC_ACTION { Type = NativeMethods.SC_ACTION_TYPE.SC_ACTION_NONE } };

            using (var actionsArray = new UnmanagedArray<NativeMethods.SC_ACTION>(checkedActions))
            using (var commandBuffer = new UnmanagedUnicodeString(configuration.FailureCommand ?? string.Empty))
            using (var messageBuffer = new UnmanagedUnicodeString(configuration.FailureRebootMessage ?? string.Empty))
            using (var serviceControlManager = new ServiceControlManagerHandle(NativeMethods.SC_MANAGER_CONNECT, machine))
            using (var serviceHandle = new ServiceHandle(serviceControlManager, NativeMethods.SERVICE_CHANGE_CONFIG | NativeMethods.SERVICE_START, name))
            using (var failureActions = new UnmanagedStructure<NativeMethods.SERVICE_FAILURE_ACTIONS>())
            {
                if (!NativeMethods.ChangeServiceConfig(serviceHandle,
                    (configuration.ServiceType == 0) ? NativeMethods.SERVICE_NO_CHANGE : configuration.ServiceType,
                    (configuration.StartType == 0) ? NativeMethods.SERVICE_NO_CHANGE : configuration.StartType,
                    (configuration.ErrorControl == 0) ? NativeMethods.SERVICE_NO_CHANGE : configuration.ErrorControl,
                    configuration.CommandLine, configuration.LoadOrderGroup, IntPtr.Zero,
                    (configuration.Dependencies == null) ? null : configuration.Dependencies.ToDoubleNullTerminated(),
                    configuration.UserName, configuration.Password, configuration.DisplayedName))
                {
                    NativeErrors.ThrowLastErrorException("ChangeServiceConfig", name, machine);
                }

                if (configuration.Description != null)
                {
                    using (var description = new UnmanagedUnicodeString(configuration.Description))
                    using (var descriptionStructure = new UnmanagedStructure<NativeMethods.SERVICE_DESCRIPTION>())
                    {
                        descriptionStructure.Value = new NativeMethods.SERVICE_DESCRIPTION
                        {
                            lpDescription = +description
                        };

                        if (!NativeMethods.ChangeServiceConfig2(serviceHandle, NativeMethods.SERVICE_CONFIG_DESCRIPTION, +descriptionStructure))
                            NativeErrors.ThrowLastErrorException("ChangeServiceConfig2", name, machine);
                    }
                }

                failureActions.Value = new NativeMethods.SERVICE_FAILURE_ACTIONS
                                           {
                    cActions = emptyActions ? 0 : actionsArray.ULength,
                    lpsaActions = (configuration.FailureActions == null) ? IntPtr.Zero : +actionsArray,
                    dwResetPeriod = configuration.FailureCounterResetInterval,
                    lpRebootMsg = (configuration.FailureCommand == null) ? IntPtr.Zero : +messageBuffer,
                    lpCommand = (configuration.FailureCommand == null) ? IntPtr.Zero : +commandBuffer,
                };

                if (!NativeMethods.ChangeServiceConfig2(serviceHandle, NativeMethods.SERVICE_CONFIG_FAILURE_ACTIONS, +failureActions))
                    NativeErrors.ThrowLastErrorException("ChangeServiceConfig2", name, machine);
            }
        }

        internal static void Start(string name, IList<string> arguments = null, string machine = null)
        {
            Check.StringIsMeaningful(name, "name");

            using (var serviceControlManager = new ServiceControlManagerHandle(NativeMethods.SC_MANAGER_CONNECT, machine))
            using (var serviceHandle = new ServiceHandle(serviceControlManager, NativeMethods.SERVICE_START, name))
            {
                if ((arguments == null) || (arguments.Count == 0))
                {
                    if (!NativeMethods.StartService(serviceHandle, 0, IntPtr.Zero))
                        NativeErrors.ThrowLastErrorException("StartService", name, machine);
                }
                else
                {
                    Check.CollectionHasNoDefaultItems(arguments);

                    using (var argumentBuffer = new UnmanagedUnicodeString(string.Join("\0", arguments)))
                    {
                        var pointers = new List<IntPtr>();

                        int offset = 0;
                        foreach (string currentItem in arguments)
                        {
                            pointers.Add(+argumentBuffer + offset);

                            offset += 2 * (currentItem.Length + 1);
                        }

                        using (var argumentsArray = new UnmanagedArray<IntPtr>(pointers.ToArray()))
                        {
                            if (!NativeMethods.StartService(serviceHandle, argumentsArray.ULength, +argumentsArray))
                                NativeErrors.ThrowLastErrorException("StartService", name, machine);
                        }
                    }
                }
            }
        }

        internal static void ControlService(string name, uint controlCode = NativeMethods.SERVICE_CONTROL_INTERROGATE, string machine = null)
        {
            Check.StringIsMeaningful(name, "name");

            uint access;

            switch (controlCode)
            {
                case NativeMethods.SERVICE_CONTROL_NETBINDADD:
                case NativeMethods.SERVICE_CONTROL_NETBINDDISABLE:
                case NativeMethods.SERVICE_CONTROL_NETBINDENABLE:
                case NativeMethods.SERVICE_CONTROL_NETBINDREMOVE:
                case NativeMethods.SERVICE_CONTROL_PARAMCHANGE:
                case NativeMethods.SERVICE_CONTROL_PAUSE:
                case NativeMethods.SERVICE_CONTROL_CONTINUE:
                    access = NativeMethods.SERVICE_PAUSE_CONTINUE;
                    break;
                case NativeMethods.SERVICE_CONTROL_INTERROGATE:
                    access = NativeMethods.SERVICE_INTERROGATE;
                    break;
                case NativeMethods.SERVICE_CONTROL_STOP:
                    access = NativeMethods.SERVICE_STOP;
                    break;
                default:
                    access = NativeMethods.SERVICE_USER_DEFINED_CONTROL;
                    break;
            }

            using (var serviceControlManager = new ServiceControlManagerHandle(NativeMethods.SC_MANAGER_CONNECT, machine))
            using (var serviceHandle = new ServiceHandle(serviceControlManager, access, name))
            using (var serviceStatus = new UnmanagedStructure<NativeMethods.SERVICE_STATUS>())
            {
                if (!NativeMethods.ControlService(serviceHandle, controlCode, +serviceStatus))
                    NativeErrors.ThrowLastErrorException("ControlService", name, machine);
            }
        }

        internal static void DeleteService(string name, string machine = null)
        {
            Check.StringIsMeaningful(name, "name");

            using (var serviceControlManager = new ServiceControlManagerHandle(NativeMethods.SC_MANAGER_CONNECT, machine))
            using (var serviceHandle = new ServiceHandle(serviceControlManager, NativeMethods.DELETE, name))
            {
                if (!NativeMethods.DeleteService(serviceHandle))
                    NativeErrors.ThrowLastErrorException("DeleteService", name, machine);
            }
        }

        internal static NativeMethods.SERVICE_STATUS QueryServiceStatus(string name, string machine = null)
        {
            Check.StringIsMeaningful(name, "name");

            using (var serviceControlManager = new ServiceControlManagerHandle(NativeMethods.SC_MANAGER_CONNECT, machine))
            using (var serviceHandle = new ServiceHandle(serviceControlManager, NativeMethods.SERVICE_QUERY_STATUS, name))
            using (var status = new UnmanagedStructure<NativeMethods.SERVICE_STATUS>())
            {
                if (!NativeMethods.QueryServiceStatus(serviceHandle, +status))
                    NativeErrors.ThrowLastErrorException("QueryServiceConfig", name, machine);
                return status;
            }
        }
    }
}
