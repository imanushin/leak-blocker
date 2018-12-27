
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Libraries.Storage.InternalTools;

/*
 * */

//
using System;
using LeakBlocker.Libraries.Common.IoC;

namespace LeakBlocker.Libraries.Storage
{
    /// <summary>
    /// Набор реализаций Singleton-объектов.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class StorageObjects
    {

        #region ICredentialsManager

        ///////////////////////// SINGLETON ICredentialsManager

        private static readonly Lazy<CredentialsManager> credentialsManagerLazy = new Lazy<CredentialsManager>(() => new CredentialsManager());
        private static readonly Singleton<ICredentialsManager> credentialsManager = new Singleton<ICredentialsManager>(() => credentialsManagerLazy.Value );

        /// <summary>
        /// Реализация типа ICredentialsManager
        /// </summary>
        public static ICredentialsManager CredentialsManager
        {
            get
            {
                return Singletons.CredentialsManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа ICredentialsManager
            /// </summary>
            public static Singleton<ICredentialsManager> CredentialsManager
            {
                get
                {
                    return credentialsManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IConfigurationManager

        ///////////////////////// SINGLETON IConfigurationManager

        private static readonly Lazy<ConfigurationManager> configurationManagerLazy = new Lazy<ConfigurationManager>(() => new ConfigurationManager());
        private static readonly Singleton<IConfigurationManager> configurationManager = new Singleton<IConfigurationManager>(() => configurationManagerLazy.Value );

        /// <summary>
        /// Реализация типа IConfigurationManager
        /// </summary>
        public static IConfigurationManager ConfigurationManager
        {
            get
            {
                return Singletons.ConfigurationManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IConfigurationManager
            /// </summary>
            public static Singleton<IConfigurationManager> ConfigurationManager
            {
                get
                {
                    return configurationManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IDatabaseModel

        ///////////////////////// FACTORY IDatabaseModel

        private static readonly Factory<IDatabaseModel> databaseModel = new Factory<IDatabaseModel>(() => new DatabaseModel());

        /// <summary>
        /// Реализация типа IDatabaseModel
        /// </summary>
        internal static IDatabaseModel CreateDatabaseModel()
        {
            return Factories.DatabaseModel.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IDatabaseModel
            /// </summary>
            internal static Factory<IDatabaseModel> DatabaseModel
            {
                get
                {
                    return databaseModel;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IDatabaseConstants

        ///////////////////////// SINGLETON IDatabaseConstants

        private static readonly Lazy<DatabaseConstants> databaseConstantsLazy = new Lazy<DatabaseConstants>(() => new DatabaseConstants());
        private static readonly Singleton<IDatabaseConstants> databaseConstants = new Singleton<IDatabaseConstants>(() => databaseConstantsLazy.Value );

        /// <summary>
        /// Реализация типа IDatabaseConstants
        /// </summary>
        internal static IDatabaseConstants DatabaseConstants
        {
            get
            {
                return Singletons.DatabaseConstants.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IDatabaseConstants
            /// </summary>
            internal static Singleton<IDatabaseConstants> DatabaseConstants
            {
                get
                {
                    return databaseConstants;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IIndexInitializer

        ///////////////////////// SINGLETON IIndexInitializer

        private static readonly Lazy<IndexInitializer> indexInitializerLazy = new Lazy<IndexInitializer>(() => new IndexInitializer());
        private static readonly Singleton<IIndexInitializer> indexInitializer = new Singleton<IIndexInitializer>(() => indexInitializerLazy.Value );

        /// <summary>
        /// Реализация типа IIndexInitializer
        /// </summary>
        internal static IIndexInitializer IndexInitializer
        {
            get
            {
                return Singletons.IndexInitializer.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IIndexInitializer
            /// </summary>
            internal static Singleton<IIndexInitializer> IndexInitializer
            {
                get
                {
                    return indexInitializer;
                }
            }
        }

        /////////////////////////

        #endregion


        #region ICrossRequestCache

        ///////////////////////// SINGLETON ICrossRequestCache

        private static readonly Lazy<CrossRequestCache> crossRequestCacheLazy = new Lazy<CrossRequestCache>(() => new CrossRequestCache());
        private static readonly Singleton<ICrossRequestCache> crossRequestCache = new Singleton<ICrossRequestCache>(() => crossRequestCacheLazy.Value );

        /// <summary>
        /// Реализация типа ICrossRequestCache
        /// </summary>
        internal static ICrossRequestCache CrossRequestCache
        {
            get
            {
                return Singletons.CrossRequestCache.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа ICrossRequestCache
            /// </summary>
            internal static Singleton<ICrossRequestCache> CrossRequestCache
            {
                get
                {
                    return crossRequestCache;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAuditItemsManager

        ///////////////////////// SINGLETON IAuditItemsManager

        private static readonly Lazy<AuditItemsManager> auditItemsManagerLazy = new Lazy<AuditItemsManager>(() => new AuditItemsManager());
        private static readonly Singleton<IAuditItemsManager> auditItemsManager = new Singleton<IAuditItemsManager>(() => auditItemsManagerLazy.Value );

        /// <summary>
        /// Реализация типа IAuditItemsManager
        /// </summary>
        public static IAuditItemsManager AuditItemsManager
        {
            get
            {
                return Singletons.AuditItemsManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAuditItemsManager
            /// </summary>
            public static Singleton<IAuditItemsManager> AuditItemsManager
            {
                get
                {
                    return auditItemsManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IDevicesManager

        ///////////////////////// SINGLETON IDevicesManager

        private static readonly Lazy<DevicesManager> devicesManagerLazy = new Lazy<DevicesManager>(() => new DevicesManager());
        private static readonly Singleton<IDevicesManager> devicesManager = new Singleton<IDevicesManager>(() => devicesManagerLazy.Value );

        /// <summary>
        /// Реализация типа IDevicesManager
        /// </summary>
        public static IDevicesManager DevicesManager
        {
            get
            {
                return Singletons.DevicesManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IDevicesManager
            /// </summary>
            public static Singleton<IDevicesManager> DevicesManager
            {
                get
                {
                    return devicesManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAccountManager

        ///////////////////////// SINGLETON IAccountManager

        private static readonly Lazy<AccountManager> accountManagerLazy = new Lazy<AccountManager>(() => new AccountManager());
        private static readonly Singleton<IAccountManager> accountManager = new Singleton<IAccountManager>(() => accountManagerLazy.Value );

        /// <summary>
        /// Реализация типа IAccountManager
        /// </summary>
        public static IAccountManager AccountManager
        {
            get
            {
                return Singletons.AccountManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAccountManager
            /// </summary>
            public static Singleton<IAccountManager> AccountManager
            {
                get
                {
                    return accountManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentEncryptionDataManager

        ///////////////////////// SINGLETON IAgentEncryptionDataManager

        private static readonly Lazy<AgentEncryptionDataManager> agentEncryptionDataManagerLazy = new Lazy<AgentEncryptionDataManager>(() => new AgentEncryptionDataManager());
        private static readonly Singleton<IAgentEncryptionDataManager> agentEncryptionDataManager = new Singleton<IAgentEncryptionDataManager>(() => agentEncryptionDataManagerLazy.Value );

        /// <summary>
        /// Реализация типа IAgentEncryptionDataManager
        /// </summary>
        public static IAgentEncryptionDataManager AgentEncryptionDataManager
        {
            get
            {
                return Singletons.AgentEncryptionDataManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAgentEncryptionDataManager
            /// </summary>
            public static Singleton<IAgentEncryptionDataManager> AgentEncryptionDataManager
            {
                get
                {
                    return agentEncryptionDataManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IDatabaseInitializer

        ///////////////////////// SINGLETON IDatabaseInitializer

        private static readonly Lazy<DatabaseInitializer> databaseInitializerLazy = new Lazy<DatabaseInitializer>(() => new DatabaseInitializer());
        private static readonly Singleton<IDatabaseInitializer> databaseInitializer = new Singleton<IDatabaseInitializer>(() => databaseInitializerLazy.Value );

        /// <summary>
        /// Реализация типа IDatabaseInitializer
        /// </summary>
        public static IDatabaseInitializer DatabaseInitializer
        {
            get
            {
                return Singletons.DatabaseInitializer.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IDatabaseInitializer
            /// </summary>
            public static Singleton<IDatabaseInitializer> DatabaseInitializer
            {
                get
                {
                    return databaseInitializer;
                }
            }
        }

        /////////////////////////

        #endregion

    }                                                                                         
}                                                                                             
                                                                                               

