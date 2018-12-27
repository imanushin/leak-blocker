using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools.Network;
using LeakBlocker.Libraries.SystemTools.Win32;

namespace LeakBlocker.Libraries.SystemTools.ProcessTools.Implementations
{
    internal sealed class WindowsService : IWindowsService
    {
        private readonly string name;
        private readonly SystemAccessOptions systemAccessOptions;

        ServiceState IWindowsService.State
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return (ServiceState)ServiceManagement.QueryServiceStatus(name, systemAccessOptions.SystemName).dwCurrentState;
                }
            }
        }

        string IWindowsService.CommandLine
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return ServiceManagement.GetServiceConfiguration(name, systemAccessOptions.SystemName).CommandLine;
                }
            }
            set
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    ServiceManagement.SetServiceConfiguration(name, new ServiceConfiguration { CommandLine = value }, systemAccessOptions.SystemName);
                }
            }
        }

        ServiceStartType IWindowsService.StartType
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return (ServiceStartType)ServiceManagement.GetServiceConfiguration(name, systemAccessOptions.SystemName).StartType;
                }
            }
            set
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    ServiceManagement.SetServiceConfiguration(name, new ServiceConfiguration { StartType = (uint)value }, systemAccessOptions.SystemName);
                }
            }
        }

        ServiceErrorControl IWindowsService.ErrorControl
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return (ServiceErrorControl)ServiceManagement.GetServiceConfiguration(name, systemAccessOptions.SystemName).ErrorControl;
                }
            }
            set
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    ServiceManagement.SetServiceConfiguration(name, new ServiceConfiguration { ErrorControl = (uint)value }, systemAccessOptions.SystemName);
                }
            }
        }

        string IWindowsService.DisplayedName
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return ServiceManagement.GetServiceConfiguration(name, systemAccessOptions.SystemName).DisplayedName;
                }
            }
            set
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    ServiceManagement.SetServiceConfiguration(name, new ServiceConfiguration { DisplayedName = value }, systemAccessOptions.SystemName);
                }
            }
        }

        string IWindowsService.Description
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return ServiceManagement.GetServiceConfiguration(name, systemAccessOptions.SystemName).Description;
                }
            }
            set
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    ServiceManagement.SetServiceConfiguration(name, new ServiceConfiguration { Description = value }, systemAccessOptions.SystemName);
                }
            }
        }

        string IWindowsService.LoadOrderGroup
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return ServiceManagement.GetServiceConfiguration(name, systemAccessOptions.SystemName).LoadOrderGroup;
                }
            }
            set
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    ServiceManagement.SetServiceConfiguration(name, new ServiceConfiguration { LoadOrderGroup = value }, systemAccessOptions.SystemName);
                }
            }
        }

        SystemServiceType IWindowsService.ServiceType
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return (SystemServiceType)ServiceManagement.GetServiceConfiguration(name, systemAccessOptions.SystemName).ServiceType;
                }
            }
            set
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    ServiceManagement.SetServiceConfiguration(name, new ServiceConfiguration { ServiceType = (uint)value }, systemAccessOptions.SystemName);
                }
            }
        }

        ReadOnlySet<string> IWindowsService.Dependencies
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return ServiceManagement.GetServiceConfiguration(name, systemAccessOptions.SystemName).Dependencies ?? (new string[0].ToReadOnlySet());
                }
            }
        }

        TimeSpan IWindowsService.FailureCounterResetInterval
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return TimeSpan.FromSeconds(ServiceManagement.GetServiceConfiguration(name, systemAccessOptions.SystemName).FailureCounterResetInterval);
                }
            }
            set
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    ServiceManagement.SetServiceConfiguration(name, new ServiceConfiguration { FailureCounterResetInterval = (uint)value.Milliseconds }, systemAccessOptions.SystemName);
                }
            }
        }

        string IWindowsService.FailureCommand
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return ServiceManagement.GetServiceConfiguration(name, systemAccessOptions.SystemName).FailureCommand;
                }
            }
            set
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    ServiceManagement.SetServiceConfiguration(name, new ServiceConfiguration { FailureCommand = value }, systemAccessOptions.SystemName);
                }
            }
        }

        string IWindowsService.FailureRebootMessage
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return ServiceManagement.GetServiceConfiguration(name, systemAccessOptions.SystemName).FailureRebootMessage;
                }
            }
            set
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    ServiceManagement.SetServiceConfiguration(name, new ServiceConfiguration { FailureRebootMessage = value }, systemAccessOptions.SystemName);
                }
            }
        }

        ReadOnlyList<ServiceFailureAction> IWindowsService.FailureActions
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    NativeMethods.SC_ACTION[] actions = ServiceManagement.GetServiceConfiguration(name, systemAccessOptions.SystemName).FailureActions;

                    return ((actions == null) ? new ServiceFailureAction[0] : actions.Select(item => (ServiceFailureAction)item.Type)).ToReadOnlyList();
                }
            }
        }

        string IWindowsService.UserName
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return ServiceManagement.GetServiceConfiguration(name, systemAccessOptions.SystemName).FailureRebootMessage;
                }
            }
            set
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    ServiceManagement.SetServiceConfiguration(name, new ServiceConfiguration { FailureRebootMessage = value }, systemAccessOptions.SystemName);
                }
            }
        }

        string IWindowsService.Password
        {
            get
            {
                Exceptions.Throw(ErrorMessage.Generic, "What the hell are you trying to do?");
                return null;
            }
            set
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    ServiceManagement.SetServiceConfiguration(name, new ServiceConfiguration { Password = value }, systemAccessOptions.SystemName);
                }
            }
        }

        bool IWindowsService.Exists
        {
            get
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return ServiceManagement.Exists(name, systemAccessOptions.SystemName);
                }
            }
        }

        public WindowsService(string name, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.StringIsMeaningful(name, "name");

            this.name = name;
            systemAccessOptions = options;
        }

        void IWindowsService.SetDependencies(IReadOnlyCollection<string> dependencies)
        {
            Check.CollectionHasNoDefaultItems(dependencies, "dependencies");

            using (new AuthenticatedConnection(systemAccessOptions))
            {
                ServiceManagement.SetServiceConfiguration(name, new ServiceConfiguration { Dependencies = dependencies.ToReadOnlySet() }, systemAccessOptions.SystemName);
            }
        }

        void IWindowsService.SetFailureActions(ReadOnlyList<ServiceFailureAction> actions)
        {
            Check.CollectionIsNotNullOrEmpty(actions, "actions");

            using (new AuthenticatedConnection(systemAccessOptions))
            {
                NativeMethods.SC_ACTION[] actionsArray = actions.Select(item => 
                    new NativeMethods.SC_ACTION { Type = (NativeMethods.SC_ACTION_TYPE)item }).ToArray();

                ServiceManagement.SetServiceConfiguration(name, new ServiceConfiguration { FailureActions = actionsArray }, systemAccessOptions.SystemName);
            }
        }

        void IWindowsService.Create(string commandLine, SystemServiceType serviceType)
        {
            Check.StringIsMeaningful(commandLine, "commandLine");

            using (new AuthenticatedConnection(systemAccessOptions))
            {
                var configuration = new ServiceConfiguration
                {
                    CommandLine = commandLine,
                    ServiceType = (uint)serviceType,
                    StartType = NativeMethods.SERVICE_DEMAND_START
                };

                ServiceManagement.CreateService(name, configuration, systemAccessOptions.SystemName);
            }
        }

        void IWindowsService.Delete()
        {
            using (new AuthenticatedConnection(systemAccessOptions))
            {
                ServiceManagement.DeleteService(name, systemAccessOptions.SystemName);
            }
        }

        void IWindowsService.Start(IList<string> arguments)
        {
            Check.CollectionHasNoDefaultItems(arguments, "arguments");

            using (new AuthenticatedConnection(systemAccessOptions))
            {
                ServiceManagement.Start(name, arguments, systemAccessOptions.SystemName);
            }
        }

        void IWindowsService.Start(params string[] arguments)
        {
            Check.CollectionHasNoDefaultItems(arguments, "arguments");

            using (new AuthenticatedConnection(systemAccessOptions))
            {
                ServiceManagement.Start(name, arguments, systemAccessOptions.SystemName);
            }
        }

        void IWindowsService.QueryStop()
        {
            using (new AuthenticatedConnection(systemAccessOptions))
            {
                ServiceManagement.ControlService(name, NativeMethods.SERVICE_CONTROL_STOP, systemAccessOptions.SystemName);
            }
        }

        void IWindowsService.SendCustomCommand(int controlCode)
        {
            if ((controlCode < 128) || (controlCode > 255))
                Exceptions.Throw(ErrorMessage.IncorrectControlCode, "Control code must be between 128 and 255.");

            using (new AuthenticatedConnection(systemAccessOptions))
            {
                ServiceManagement.ControlService(name, (uint)controlCode, systemAccessOptions.SystemName);
            }
        }

        bool IWindowsService.WaitForState(ServiceState state, TimeSpan timeout)
        {
            Check.EnumerationValueIsDefined(state, "state");

            return PeriodicCheck.WaitUntilSuccess(delegate
            {
                using (new AuthenticatedConnection(systemAccessOptions))
                {
                    return ((ServiceState)ServiceManagement.QueryServiceStatus(name, systemAccessOptions.SystemName).dwCurrentState == state);
                } 
            }, TimeSpan.FromSeconds(3), timeout);
        }
    }
}
