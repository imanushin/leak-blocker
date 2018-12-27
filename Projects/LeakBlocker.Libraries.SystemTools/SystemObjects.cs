
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Libraries.SystemTools.Implementations;
using LeakBlocker.Libraries.SystemTools.Entities.Implementations;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools.Devices;
using LeakBlocker.Libraries.SystemTools.Devices.Implementations;
using LeakBlocker.Libraries.SystemTools.ProcessTools;
using LeakBlocker.Libraries.SystemTools.ProcessTools.Implementations;
using LeakBlocker.Libraries.SystemTools.Drivers;  
using LeakBlocker.Libraries.SystemTools.Network;  
using System.Collections.Generic;         

/*
 * */

//
using System;
using LeakBlocker.Libraries.Common.IoC;

namespace LeakBlocker.Libraries.SystemTools
{
    /// <summary>
    /// Набор реализаций Singleton-объектов.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class SystemObjects
    {

        #region ITimeProvider

        ///////////////////////// SINGLETON ITimeProvider

        private static readonly Lazy<TimeProvider> timeProviderLazy = new Lazy<TimeProvider>(() => new TimeProvider());
        private static readonly Singleton<ITimeProvider> timeProvider = new Singleton<ITimeProvider>(() => timeProviderLazy.Value );

        /// <summary>
        /// Реализация типа ITimeProvider
        /// </summary>
        public static ITimeProvider TimeProvider
        {
            get
            {
                return Singletons.TimeProvider.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа ITimeProvider
            /// </summary>
            public static Singleton<ITimeProvider> TimeProvider
            {
                get
                {
                    return timeProvider;
                }
            }
        }

        /////////////////////////

        #endregion


        #region ISystemAccountTools

        ///////////////////////// SINGLETON ISystemAccountTools

        private static readonly Lazy<SystemAccountTools> systemAccountToolsLazy = new Lazy<SystemAccountTools>(() => new SystemAccountTools());
        private static readonly Singleton<ISystemAccountTools> systemAccountTools = new Singleton<ISystemAccountTools>(() => systemAccountToolsLazy.Value );

        /// <summary>
        /// Реализация типа ISystemAccountTools
        /// </summary>
        public static ISystemAccountTools SystemAccountTools
        {
            get
            {
                return Singletons.SystemAccountTools.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа ISystemAccountTools
            /// </summary>
            public static Singleton<ISystemAccountTools> SystemAccountTools
            {
                get
                {
                    return systemAccountTools;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IFileTools

        ///////////////////////// SINGLETON IFileTools

        private static readonly Lazy<FileTools> fileToolsLazy = new Lazy<FileTools>(() => new FileTools());
        private static readonly Singleton<IFileTools> fileTools = new Singleton<IFileTools>(() => fileToolsLazy.Value );

        /// <summary>
        /// Реализация типа IFileTools
        /// </summary>
        public static IFileTools FileTools
        {
            get
            {
                return Singletons.FileTools.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IFileTools
            /// </summary>
            public static Singleton<IFileTools> FileTools
            {
                get
                {
                    return fileTools;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IPrerequisites

        ///////////////////////// SINGLETON IPrerequisites

        private static readonly Lazy<Prerequisites> prerequisitesLazy = new Lazy<Prerequisites>(() => new Prerequisites());
        private static readonly Singleton<IPrerequisites> prerequisites = new Singleton<IPrerequisites>(() => prerequisitesLazy.Value );

        /// <summary>
        /// Реализация типа IPrerequisites
        /// </summary>
        public static IPrerequisites Prerequisites
        {
            get
            {
                return Singletons.Prerequisites.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IPrerequisites
            /// </summary>
            public static Singleton<IPrerequisites> Prerequisites
            {
                get
                {
                    return prerequisites;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IDeviceProvider

        ///////////////////////// SINGLETON IDeviceProvider

        private static readonly Lazy<DeviceProvider> deviceProviderLazy = new Lazy<DeviceProvider>(() => new DeviceProvider());
        private static readonly Singleton<IDeviceProvider> deviceProvider = new Singleton<IDeviceProvider>(() => deviceProviderLazy.Value );

        /// <summary>
        /// Реализация типа IDeviceProvider
        /// </summary>
        public static IDeviceProvider DeviceProvider
        {
            get
            {
                return Singletons.DeviceProvider.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IDeviceProvider
            /// </summary>
            public static Singleton<IDeviceProvider> DeviceProvider
            {
                get
                {
                    return deviceProvider;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IDomainSnapshot

        ///////////////////////// FACTORY IDomainSnapshot

        private static readonly Factory<IDomainSnapshot, Credentials> domainSnapshot = new Factory<IDomainSnapshot, Credentials>((credentials) => new DomainSnapshot(credentials));

        /// <summary>
        /// Реализация типа IDomainSnapshot
        /// </summary>
        public static IDomainSnapshot CreateDomainSnapshot(Credentials credentials)
        {
            return Factories.DomainSnapshot.CreateInstance(credentials);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IDomainSnapshot
            /// </summary>
            public static Factory<IDomainSnapshot, Credentials> DomainSnapshot
            {
                get
                {
                    return domainSnapshot;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IDomainSnapshot

        ///////////////////////// FACTORY IDomainSnapshot

        private static readonly Factory<IDomainSnapshot> emptyDomainSnapshot = new Factory<IDomainSnapshot>(() => new DomainSnapshot());

        /// <summary>
        /// Реализация типа IDomainSnapshot
        /// </summary>
        public static IDomainSnapshot CreateEmptyDomainSnapshot()
        {
            return Factories.EmptyDomainSnapshot.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IDomainSnapshot
            /// </summary>
            public static Factory<IDomainSnapshot> EmptyDomainSnapshot
            {
                get
                {
                    return emptyDomainSnapshot;
                }
            }
        }

        /////////////////////////

        #endregion


        #region ILocalDataCache

        ///////////////////////// FACTORY ILocalDataCache

        private static readonly Factory<ILocalDataCache> localDataCache = new Factory<ILocalDataCache>(() => new LocalDataCache());

        /// <summary>
        /// Реализация типа ILocalDataCache
        /// </summary>
        public static ILocalDataCache CreateLocalDataCache()
        {
            return Factories.LocalDataCache.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа ILocalDataCache
            /// </summary>
            public static Factory<ILocalDataCache> LocalDataCache
            {
                get
                {
                    return localDataCache;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IWindowsService

        ///////////////////////// FACTORY IWindowsService

        private static readonly Factory<IWindowsService, string, SystemAccessOptions> windowsService = new Factory<IWindowsService, string, SystemAccessOptions>((name, options) => new WindowsService(name, options));

        /// <summary>
        /// Реализация типа IWindowsService
        /// </summary>
        public static IWindowsService CreateWindowsService(string name, SystemAccessOptions options = default(SystemAccessOptions))
        {
            return Factories.WindowsService.CreateInstance(name, options);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IWindowsService
            /// </summary>
            public static Factory<IWindowsService, string, SystemAccessOptions> WindowsService
            {
                get
                {
                    return windowsService;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IPrivateRegistryStorage

        ///////////////////////// FACTORY IPrivateRegistryStorage

        private static readonly Factory<IPrivateRegistryStorage, Guid> privateRegistryStorage = new Factory<IPrivateRegistryStorage, Guid>((identifier) => new PrivateRegistryStorage(identifier));

        /// <summary>
        /// Реализация типа IPrivateRegistryStorage
        /// </summary>
        public static IPrivateRegistryStorage CreatePrivateRegistryStorage(Guid identifier)
        {
            return Factories.PrivateRegistryStorage.CreateInstance(identifier);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IPrivateRegistryStorage
            /// </summary>
            public static Factory<IPrivateRegistryStorage, Guid> PrivateRegistryStorage
            {
                get
                {
                    return privateRegistryStorage;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IGlobalFlag

        ///////////////////////// FACTORY IGlobalFlag

        private static readonly Factory<IGlobalFlag, string> globalFlag = new Factory<IGlobalFlag, string>((name) => new GlobalFlag(name));

        /// <summary>
        /// Реализация типа IGlobalFlag
        /// </summary>
        public static IGlobalFlag CreateGlobalFlag(string name)
        {
            return Factories.GlobalFlag.CreateInstance(name);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IGlobalFlag
            /// </summary>
            public static Factory<IGlobalFlag, string> GlobalFlag
            {
                get
                {
                    return globalFlag;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IPrivateFile

        ///////////////////////// FACTORY IPrivateFile

        private static readonly Factory<IPrivateFile, string> privateFile = new Factory<IPrivateFile, string>((path) => new PrivateFile(path));

        /// <summary>
        /// Реализация типа IPrivateFile
        /// </summary>
        public static IPrivateFile CreatePrivateFile(string path)
        {
            return Factories.PrivateFile.CreateInstance(path);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IPrivateFile
            /// </summary>
            public static Factory<IPrivateFile, string> PrivateFile
            {
                get
                {
                    return privateFile;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IMailslotClient

        ///////////////////////// FACTORY IMailslotClient

        private static readonly Factory<IMailslotClient, string, int> mailslotClient = new Factory<IMailslotClient, string, int>((name, messageSize) => new MailslotClient(name, messageSize));

        /// <summary>
        /// Реализация типа IMailslotClient
        /// </summary>
        public static IMailslotClient CreateMailslotClient(string name, int messageSize)
        {
            return Factories.MailslotClient.CreateInstance(name, messageSize);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IMailslotClient
            /// </summary>
            public static Factory<IMailslotClient, string, int> MailslotClient
            {
                get
                {
                    return mailslotClient;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IMailslotServer

        ///////////////////////// FACTORY IMailslotServer

        private static readonly Factory<IMailslotServer, string, int> mailslotServer = new Factory<IMailslotServer, string, int>((name, messageSize) => new MailslotServer(name, messageSize));

        /// <summary>
        /// Реализация типа IMailslotServer
        /// </summary>
        public static IMailslotServer CreateMailslotServer(string name, int messageSize)
        {
            return Factories.MailslotServer.CreateInstance(name, messageSize);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IMailslotServer
            /// </summary>
            public static Factory<IMailslotServer, string, int> MailslotServer
            {
                get
                {
                    return mailslotServer;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IDriverController

        ///////////////////////// FACTORY IDriverController

        private static readonly Factory<IDriverController, IDriverControllerHandler> driverController = new Factory<IDriverController, IDriverControllerHandler>((eventHandler) => new DriverController(eventHandler));

        /// <summary>
        /// Реализация типа IDriverController
        /// </summary>
        public static IDriverController CreateDriverController(IDriverControllerHandler eventHandler = null)
        {
            return Factories.DriverController.CreateInstance(eventHandler);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IDriverController
            /// </summary>
            public static Factory<IDriverController, IDriverControllerHandler> DriverController
            {
                get
                {
                    return driverController;
                }
            }
        }

        /////////////////////////

        #endregion

    }                                                                                         
}                                                                                             
                                                                                               

