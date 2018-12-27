
using LeakBlocker.Agent.Core.Implementations;
using LeakBlocker.Agent.Core.Settings;
using LeakBlocker.Agent.Core.Settings.Implementations;
using LeakBlocker.ServerShared.AgentCommunication;   
using LeakBlocker.Libraries.Common.Network;       
using LeakBlocker.Libraries.SystemTools.Drivers;  
using LeakBlocker.Libraries.Common.Entities;  
using System.Collections.Generic;         
using LeakBlocker.Libraries.Common.Entities.Security; 
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Libraries.Common.Entities.Settings;   
using LeakBlocker.Libraries.Common.Collections;        

/*
 * */

//
using System;
using LeakBlocker.Libraries.Common.IoC;

namespace LeakBlocker.Agent.Core
{
    /// <summary>
    /// Набор реализаций Singleton-объектов.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class AgentObjects
    {

        #region IStackStorage

        ///////////////////////// FACTORY IStackStorage

        private static readonly Factory<IStackStorage, string> stackStorage = new Factory<IStackStorage, string>((file) => new StackStorage(file));

        /// <summary>
        /// Реализация типа IStackStorage
        /// </summary>
        internal static IStackStorage CreateStackStorage(string file)
        {
            return Factories.StackStorage.CreateInstance(file);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IStackStorage
            /// </summary>
            internal static Factory<IStackStorage, string> StackStorage
            {
                get
                {
                    return stackStorage;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IStackStorage

        ///////////////////////// FACTORY IStackStorage

        private static readonly Factory<IStackStorage> alternativeStackStorage = new Factory<IStackStorage>(() => new StackStorage());

        /// <summary>
        /// Реализация типа IStackStorage
        /// </summary>
        internal static IStackStorage CreateAlternativeStackStorage()
        {
            return Factories.AlternativeStackStorage.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IStackStorage
            /// </summary>
            internal static Factory<IStackStorage> AlternativeStackStorage
            {
                get
                {
                    return alternativeStackStorage;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentInstaller

        ///////////////////////// SINGLETON IAgentInstaller

        private static readonly Lazy<AgentInstaller> agentInstallerLazy = new Lazy<AgentInstaller>(() => new AgentInstaller());
        private static readonly Singleton<IAgentInstaller> agentInstaller = new Singleton<IAgentInstaller>(() => agentInstallerLazy.Value );

        /// <summary>
        /// Реализация типа IAgentInstaller
        /// </summary>
        internal static IAgentInstaller AgentInstaller
        {
            get
            {
                return Singletons.AgentInstaller.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAgentInstaller
            /// </summary>
            internal static Singleton<IAgentInstaller> AgentInstaller
            {
                get
                {
                    return agentInstaller;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentPrivateStorage

        ///////////////////////// SINGLETON IAgentPrivateStorage

        private static readonly Lazy<AgentPrivateStorage> agentPrivateStorageLazy = new Lazy<AgentPrivateStorage>(() => new AgentPrivateStorage());
        private static readonly Singleton<IAgentPrivateStorage> agentPrivateStorage = new Singleton<IAgentPrivateStorage>(() => agentPrivateStorageLazy.Value );

        /// <summary>
        /// Реализация типа IAgentPrivateStorage
        /// </summary>
        internal static IAgentPrivateStorage AgentPrivateStorage
        {
            get
            {
                return Singletons.AgentPrivateStorage.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAgentPrivateStorage
            /// </summary>
            internal static Singleton<IAgentPrivateStorage> AgentPrivateStorage
            {
                get
                {
                    return agentPrivateStorage;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentControlService

        ///////////////////////// SINGLETON IAgentControlService

        private static readonly Lazy<AgentControlServiceClient> agentControlServiceClientLazy = new Lazy<AgentControlServiceClient>(() => new AgentControlServiceClient());
        private static readonly Singleton<IAgentControlService> agentControlServiceClient = new Singleton<IAgentControlService>(() => agentControlServiceClientLazy.Value );

        /// <summary>
        /// Реализация типа IAgentControlService
        /// </summary>
        internal static IAgentControlService AgentControlServiceClient
        {
            get
            {
                return Singletons.AgentControlServiceClient.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAgentControlService
            /// </summary>
            internal static Singleton<IAgentControlService> AgentControlServiceClient
            {
                get
                {
                    return agentControlServiceClient;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentConstants

        ///////////////////////// SINGLETON IAgentConstants

        private static readonly Lazy<AgentConstants> agentConstantsLazy = new Lazy<AgentConstants>(() => new AgentConstants());
        private static readonly Singleton<IAgentConstants> agentConstants = new Singleton<IAgentConstants>(() => agentConstantsLazy.Value );

        /// <summary>
        /// Реализация типа IAgentConstants
        /// </summary>
        internal static IAgentConstants AgentConstants
        {
            get
            {
                return Singletons.AgentConstants.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAgentConstants
            /// </summary>
            internal static Singleton<IAgentConstants> AgentConstants
            {
                get
                {
                    return agentConstants;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IVersionIndependentPrivateStorage

        ///////////////////////// SINGLETON IVersionIndependentPrivateStorage

        private static readonly Lazy<VersionIndependentPrivateStorage> versionIndependentPrivateStorageLazy = new Lazy<VersionIndependentPrivateStorage>(() => new VersionIndependentPrivateStorage());
        private static readonly Singleton<IVersionIndependentPrivateStorage> versionIndependentPrivateStorage = new Singleton<IVersionIndependentPrivateStorage>(() => versionIndependentPrivateStorageLazy.Value );

        /// <summary>
        /// Реализация типа IVersionIndependentPrivateStorage
        /// </summary>
        internal static IVersionIndependentPrivateStorage VersionIndependentPrivateStorage
        {
            get
            {
                return Singletons.VersionIndependentPrivateStorage.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IVersionIndependentPrivateStorage
            /// </summary>
            internal static Singleton<IVersionIndependentPrivateStorage> VersionIndependentPrivateStorage
            {
                get
                {
                    return versionIndependentPrivateStorage;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IRuleConditionChecker

        ///////////////////////// SINGLETON IRuleConditionChecker

        private static readonly Lazy<RuleConditionChecker> ruleConditionCheckerLazy = new Lazy<RuleConditionChecker>(() => new RuleConditionChecker());
        private static readonly Singleton<IRuleConditionChecker> ruleConditionChecker = new Singleton<IRuleConditionChecker>(() => ruleConditionCheckerLazy.Value );

        /// <summary>
        /// Реализация типа IRuleConditionChecker
        /// </summary>
        internal static IRuleConditionChecker RuleConditionChecker
        {
            get
            {
                return Singletons.RuleConditionChecker.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IRuleConditionChecker
            /// </summary>
            internal static Singleton<IRuleConditionChecker> RuleConditionChecker
            {
                get
                {
                    return ruleConditionChecker;
                }
            }
        }

        /////////////////////////

        #endregion


        #region ITemporaryAccessConditionsChecker

        ///////////////////////// SINGLETON ITemporaryAccessConditionsChecker

        private static readonly Lazy<TemporaryAccessConditionsChecker> temporaryAccessConditionsCheckerLazy = new Lazy<TemporaryAccessConditionsChecker>(() => new TemporaryAccessConditionsChecker());
        private static readonly Singleton<ITemporaryAccessConditionsChecker> temporaryAccessConditionsChecker = new Singleton<ITemporaryAccessConditionsChecker>(() => temporaryAccessConditionsCheckerLazy.Value );

        /// <summary>
        /// Реализация типа ITemporaryAccessConditionsChecker
        /// </summary>
        internal static ITemporaryAccessConditionsChecker TemporaryAccessConditionsChecker
        {
            get
            {
                return Singletons.TemporaryAccessConditionsChecker.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа ITemporaryAccessConditionsChecker
            /// </summary>
            internal static Singleton<ITemporaryAccessConditionsChecker> TemporaryAccessConditionsChecker
            {
                get
                {
                    return temporaryAccessConditionsChecker;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IProgramConfigurationChecker

        ///////////////////////// SINGLETON IProgramConfigurationChecker

        private static readonly Lazy<ProgramConfigurationChecker> programConfigurationCheckerLazy = new Lazy<ProgramConfigurationChecker>(() => new ProgramConfigurationChecker());
        private static readonly Singleton<IProgramConfigurationChecker> programConfigurationChecker = new Singleton<IProgramConfigurationChecker>(() => programConfigurationCheckerLazy.Value );

        /// <summary>
        /// Реализация типа IProgramConfigurationChecker
        /// </summary>
        internal static IProgramConfigurationChecker ProgramConfigurationChecker
        {
            get
            {
                return Singletons.ProgramConfigurationChecker.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IProgramConfigurationChecker
            /// </summary>
            internal static Singleton<IProgramConfigurationChecker> ProgramConfigurationChecker
            {
                get
                {
                    return programConfigurationChecker;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentDataStorage

        ///////////////////////// FACTORY IAgentDataStorage

        private static readonly Factory<IAgentDataStorage, string> agentDataStorage = new Factory<IAgentDataStorage, string>((file) => new AgentDataStorage(file));

        /// <summary>
        /// Реализация типа IAgentDataStorage
        /// </summary>
        internal static IAgentDataStorage CreateAgentDataStorage(string file)
        {
            return Factories.AgentDataStorage.CreateInstance(file);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IAgentDataStorage
            /// </summary>
            internal static Factory<IAgentDataStorage, string> AgentDataStorage
            {
                get
                {
                    return agentDataStorage;
                }
            }
        }

        /////////////////////////

        #endregion


        #region ILocalControlClient

        ///////////////////////// FACTORY ILocalControlClient

        private static readonly Factory<ILocalControlClient> localControlClient = new Factory<ILocalControlClient>(() => new LocalControlClient());

        /// <summary>
        /// Реализация типа ILocalControlClient
        /// </summary>
        internal static ILocalControlClient CreateLocalControlClient()
        {
            return Factories.LocalControlClient.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа ILocalControlClient
            /// </summary>
            internal static Factory<ILocalControlClient> LocalControlClient
            {
                get
                {
                    return localControlClient;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentTaskManager

        ///////////////////////// FACTORY IAgentTaskManager

        private static readonly Factory<IAgentTaskManager, IAuditStorage, IDriverController, IAgentDataStorage, IAgentServiceController> agentTaskManager = new Factory<IAgentTaskManager, IAuditStorage, IDriverController, IAgentDataStorage, IAgentServiceController>((auditStorage, driverController, dataStorage, serviceController) => new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController));

        /// <summary>
        /// Реализация типа IAgentTaskManager
        /// </summary>
        internal static IAgentTaskManager CreateAgentTaskManager(IAuditStorage auditStorage, IDriverController driverController, IAgentDataStorage dataStorage, IAgentServiceController serviceController)
        {
            return Factories.AgentTaskManager.CreateInstance(auditStorage, driverController, dataStorage, serviceController);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IAgentTaskManager
            /// </summary>
            internal static Factory<IAgentTaskManager, IAuditStorage, IDriverController, IAgentDataStorage, IAgentServiceController> AgentTaskManager
            {
                get
                {
                    return agentTaskManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAuditStorage

        ///////////////////////// FACTORY IAuditStorage

        private static readonly Factory<IAuditStorage, IStackStorage, IAgentDataStorage> auditStorage = new Factory<IAuditStorage, IStackStorage, IAgentDataStorage>((stackStorage, dataStorage) => new AuditStorage(stackStorage, dataStorage));

        /// <summary>
        /// Реализация типа IAuditStorage
        /// </summary>
        internal static IAuditStorage CreateAuditStorage(IStackStorage stackStorage, IAgentDataStorage dataStorage)
        {
            return Factories.AuditStorage.CreateInstance(stackStorage, dataStorage);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IAuditStorage
            /// </summary>
            internal static Factory<IAuditStorage, IStackStorage, IAgentDataStorage> AuditStorage
            {
                get
                {
                    return auditStorage;
                }
            }
        }

        /////////////////////////

        #endregion


        #region ILocalControlServer

        ///////////////////////// FACTORY ILocalControlServer

        private static readonly Factory<ILocalControlServer, ILocalControlServerHandler> localControlServer = new Factory<ILocalControlServer, ILocalControlServerHandler>((eventHandler) => new LocalControlServer(eventHandler));

        /// <summary>
        /// Реализация типа ILocalControlServer
        /// </summary>
        internal static ILocalControlServer CreateLocalControlServer(ILocalControlServerHandler eventHandler)
        {
            return Factories.LocalControlServer.CreateInstance(eventHandler);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа ILocalControlServer
            /// </summary>
            internal static Factory<ILocalControlServer, ILocalControlServerHandler> LocalControlServer
            {
                get
                {
                    return localControlServer;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IStateNotifier

        ///////////////////////// FACTORY IStateNotifier

        private static readonly Factory<IStateNotifier, IStateNotifierHandler, IReadOnlyCollection<BaseUserAccount>, IReadOnlyCollection<DeviceDescription>, ReadOnlyDictionary<DeviceDescription, DeviceAccessType>> stateNotifier = new Factory<IStateNotifier, IStateNotifierHandler, IReadOnlyCollection<BaseUserAccount>, IReadOnlyCollection<DeviceDescription>, ReadOnlyDictionary<DeviceDescription, DeviceAccessType>>((handler, users, devices, deviceStates) => new StateNotifier(handler, users, devices, deviceStates));

        /// <summary>
        /// Реализация типа IStateNotifier
        /// </summary>
        internal static IStateNotifier CreateStateNotifier(IStateNotifierHandler handler, IReadOnlyCollection<BaseUserAccount> users, IReadOnlyCollection<DeviceDescription> devices, ReadOnlyDictionary<DeviceDescription, DeviceAccessType> deviceStates)
        {
            return Factories.StateNotifier.CreateInstance(handler, users, devices, deviceStates);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IStateNotifier
            /// </summary>
            internal static Factory<IStateNotifier, IStateNotifierHandler, IReadOnlyCollection<BaseUserAccount>, IReadOnlyCollection<DeviceDescription>, ReadOnlyDictionary<DeviceDescription, DeviceAccessType>> StateNotifier
            {
                get
                {
                    return stateNotifier;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IFileSystemAccessController

        ///////////////////////// FACTORY IFileSystemAccessController

        private static readonly Factory<IFileSystemAccessController> fileSystemAccessController = new Factory<IFileSystemAccessController>(() => new FileSystemAccessController());

        /// <summary>
        /// Реализация типа IFileSystemAccessController
        /// </summary>
        internal static IFileSystemAccessController CreateFileSystemAccessController()
        {
            return Factories.FileSystemAccessController.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IFileSystemAccessController
            /// </summary>
            internal static Factory<IFileSystemAccessController> FileSystemAccessController
            {
                get
                {
                    return fileSystemAccessController;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IHardwareProfile

        ///////////////////////// FACTORY IHardwareProfile

        private static readonly Factory<IHardwareProfile, CachedComputerData, IReadOnlyCollection<CachedUserData>, IReadOnlyCollection<DeviceDescription>, ProgramConfiguration> hardwareProfile = new Factory<IHardwareProfile, CachedComputerData, IReadOnlyCollection<CachedUserData>, IReadOnlyCollection<DeviceDescription>, ProgramConfiguration>((computer, users, devices, settings) => new HardwareProfile(computer, users, devices, settings));

        /// <summary>
        /// Реализация типа IHardwareProfile
        /// </summary>
        internal static IHardwareProfile CreateHardwareProfile(CachedComputerData computer, IReadOnlyCollection<CachedUserData> users, IReadOnlyCollection<DeviceDescription> devices, ProgramConfiguration settings)
        {
            return Factories.HardwareProfile.CreateInstance(computer, users, devices, settings);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IHardwareProfile
            /// </summary>
            internal static Factory<IHardwareProfile, CachedComputerData, IReadOnlyCollection<CachedUserData>, IReadOnlyCollection<DeviceDescription>, ProgramConfiguration> HardwareProfile
            {
                get
                {
                    return hardwareProfile;
                }
            }
        }

        /////////////////////////

        #endregion

    }                                                                                         
}                                                                                             
                                                                                               

