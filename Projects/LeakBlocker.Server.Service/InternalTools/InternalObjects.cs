

using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.AdminUsersStorage;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation.LocalStorages;
using LeakBlocker.Server.Service.Network.AdminView;
using LeakBlocker.Server.Service.Network.Agent;

/*
 * */

//
using System;
using LeakBlocker.Libraries.Common.IoC;

namespace LeakBlocker.Server.Service.InternalTools
{
    /// <summary>
    /// Набор реализаций Singleton-объектов.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class InternalObjects
    {

        #region IAgentStatusStore

        ///////////////////////// SINGLETON IAgentStatusStore

        private static readonly Lazy<AgentStatusStore> agentStatusStoreLazy = new Lazy<AgentStatusStore>(() => new AgentStatusStore());
        private static readonly Singleton<IAgentStatusStore> agentStatusStore = new Singleton<IAgentStatusStore>(() => agentStatusStoreLazy.Value );

        /// <summary>
        /// Реализация типа IAgentStatusStore
        /// </summary>
        internal static IAgentStatusStore AgentStatusStore
        {
            get
            {
                return Singletons.AgentStatusStore.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAgentStatusStore
            /// </summary>
            internal static Singleton<IAgentStatusStore> AgentStatusStore
            {
                get
                {
                    return agentStatusStore;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentStatusObserver

        ///////////////////////// SINGLETON IAgentStatusObserver

        private static readonly Lazy<AgentStatusObserver> agentStatusObserverLazy = new Lazy<AgentStatusObserver>(() => new AgentStatusObserver());
        private static readonly Singleton<IAgentStatusObserver> agentStatusObserver = new Singleton<IAgentStatusObserver>(() => agentStatusObserverLazy.Value );

        /// <summary>
        /// Реализация типа IAgentStatusObserver
        /// </summary>
        internal static IAgentStatusObserver AgentStatusObserver
        {
            get
            {
                return Singletons.AgentStatusObserver.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAgentStatusObserver
            /// </summary>
            internal static Singleton<IAgentStatusObserver> AgentStatusObserver
            {
                get
                {
                    return agentStatusObserver;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAuditItemHelper

        ///////////////////////// SINGLETON IAuditItemHelper

        private static readonly Lazy<AuditItemHelper> auditItemHelperLazy = new Lazy<AuditItemHelper>(() => new AuditItemHelper());
        private static readonly Singleton<IAuditItemHelper> auditItemHelper = new Singleton<IAuditItemHelper>(() => auditItemHelperLazy.Value );

        /// <summary>
        /// Реализация типа IAuditItemHelper
        /// </summary>
        internal static IAuditItemHelper AuditItemHelper
        {
            get
            {
                return Singletons.AuditItemHelper.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAuditItemHelper
            /// </summary>
            internal static Singleton<IAuditItemHelper> AuditItemHelper
            {
                get
                {
                    return auditItemHelper;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentManager

        ///////////////////////// SINGLETON IAgentManager

        private static readonly Lazy<AgentManager> agentManagerLazy = new Lazy<AgentManager>(() => new AgentManager());
        private static readonly Singleton<IAgentManager> agentManager = new Singleton<IAgentManager>(() => agentManagerLazy.Value );

        /// <summary>
        /// Реализация типа IAgentManager
        /// </summary>
        internal static IAgentManager AgentManager
        {
            get
            {
                return Singletons.AgentManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAgentManager
            /// </summary>
            internal static Singleton<IAgentManager> AgentManager
            {
                get
                {
                    return agentManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IScopeManager

        ///////////////////////// SINGLETON IScopeManager

        private static readonly Lazy<ScopeManager> scopeManagerLazy = new Lazy<ScopeManager>(() => new ScopeManager());
        private static readonly Singleton<IScopeManager> scopeManager = new Singleton<IScopeManager>(() => scopeManagerLazy.Value );

        /// <summary>
        /// Реализация типа IScopeManager
        /// </summary>
        internal static IScopeManager ScopeManager
        {
            get
            {
                return Singletons.ScopeManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IScopeManager
            /// </summary>
            internal static Singleton<IScopeManager> ScopeManager
            {
                get
                {
                    return scopeManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IReportMailer

        ///////////////////////// SINGLETON IReportMailer

        private static readonly Lazy<ReportMailer> reportMailerLazy = new Lazy<ReportMailer>(() => new ReportMailer());
        private static readonly Singleton<IReportMailer> reportMailer = new Singleton<IReportMailer>(() => reportMailerLazy.Value );

        /// <summary>
        /// Реализация типа IReportMailer
        /// </summary>
        internal static IReportMailer ReportMailer
        {
            get
            {
                return Singletons.ReportMailer.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IReportMailer
            /// </summary>
            internal static Singleton<IReportMailer> ReportMailer
            {
                get
                {
                    return reportMailer;
                }
            }
        }

        /////////////////////////

        #endregion


        #region ILicenseStorage

        ///////////////////////// SINGLETON ILicenseStorage

        private static readonly Lazy<LicenseStorage> licenseStorageLazy = new Lazy<LicenseStorage>(() => new LicenseStorage());
        private static readonly Singleton<ILicenseStorage> licenseStorage = new Singleton<ILicenseStorage>(() => licenseStorageLazy.Value );

        /// <summary>
        /// Реализация типа ILicenseStorage
        /// </summary>
        internal static ILicenseStorage LicenseStorage
        {
            get
            {
                return Singletons.LicenseStorage.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа ILicenseStorage
            /// </summary>
            internal static Singleton<ILicenseStorage> LicenseStorage
            {
                get
                {
                    return licenseStorage;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IReportConfigurationStorage

        ///////////////////////// SINGLETON IReportConfigurationStorage

        private static readonly Lazy<ReportConfigurationStorage> reportConfigurationStorageLazy = new Lazy<ReportConfigurationStorage>(() => new ReportConfigurationStorage());
        private static readonly Singleton<IReportConfigurationStorage> reportConfigurationStorage = new Singleton<IReportConfigurationStorage>(() => reportConfigurationStorageLazy.Value );

        /// <summary>
        /// Реализация типа IReportConfigurationStorage
        /// </summary>
        internal static IReportConfigurationStorage ReportConfigurationStorage
        {
            get
            {
                return Singletons.ReportConfigurationStorage.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IReportConfigurationStorage
            /// </summary>
            internal static Singleton<IReportConfigurationStorage> ReportConfigurationStorage
            {
                get
                {
                    return reportConfigurationStorage;
                }
            }
        }

        /////////////////////////

        #endregion


        #region ISecurityObjectCache

        ///////////////////////// SINGLETON ISecurityObjectCache

        private static readonly Lazy<SecurityObjectCache> securityObjectCacheLazy = new Lazy<SecurityObjectCache>(() => new SecurityObjectCache());
        private static readonly Singleton<ISecurityObjectCache> securityObjectCache = new Singleton<ISecurityObjectCache>(() => securityObjectCacheLazy.Value );

        /// <summary>
        /// Реализация типа ISecurityObjectCache
        /// </summary>
        internal static ISecurityObjectCache SecurityObjectCache
        {
            get
            {
                return Singletons.SecurityObjectCache.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа ISecurityObjectCache
            /// </summary>
            internal static Singleton<ISecurityObjectCache> SecurityObjectCache
            {
                get
                {
                    return securityObjectCache;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentKeyManager

        ///////////////////////// SINGLETON IAgentKeyManager

        private static readonly Lazy<AgentKeyManager> agentKeyManagerLazy = new Lazy<AgentKeyManager>(() => new AgentKeyManager());
        private static readonly Singleton<IAgentKeyManager> agentKeyManager = new Singleton<IAgentKeyManager>(() => agentKeyManagerLazy.Value );

        /// <summary>
        /// Реализация типа IAgentKeyManager
        /// </summary>
        internal static IAgentKeyManager AgentKeyManager
        {
            get
            {
                return Singletons.AgentKeyManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAgentKeyManager
            /// </summary>
            internal static Singleton<IAgentKeyManager> AgentKeyManager
            {
                get
                {
                    return agentKeyManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentNetworkSession

        ///////////////////////// SINGLETON IAgentNetworkSession

        private static readonly Lazy<AgentSessionManager> agentSessionManagerLazy = new Lazy<AgentSessionManager>(() => new AgentSessionManager());
        private static readonly Singleton<IAgentNetworkSession> agentSessionManager = new Singleton<IAgentNetworkSession>(() => agentSessionManagerLazy.Value );

        /// <summary>
        /// Реализация типа IAgentNetworkSession
        /// </summary>
        internal static IAgentNetworkSession AgentSessionManager
        {
            get
            {
                return Singletons.AgentSessionManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAgentNetworkSession
            /// </summary>
            internal static Singleton<IAgentNetworkSession> AgentSessionManager
            {
                get
                {
                    return agentSessionManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentNetworkSession

        ///////////////////////// SINGLETON IAgentNetworkSession

        private static readonly Lazy<AgentInstallationSecuritySessionManager> agentInstallationSecuritySessionManagerLazy = new Lazy<AgentInstallationSecuritySessionManager>(() => new AgentInstallationSecuritySessionManager());
        private static readonly Singleton<IAgentNetworkSession> agentInstallationSecuritySessionManager = new Singleton<IAgentNetworkSession>(() => agentInstallationSecuritySessionManagerLazy.Value );

        /// <summary>
        /// Реализация типа IAgentNetworkSession
        /// </summary>
        internal static IAgentNetworkSession AgentInstallationSecuritySessionManager
        {
            get
            {
                return Singletons.AgentInstallationSecuritySessionManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAgentNetworkSession
            /// </summary>
            internal static Singleton<IAgentNetworkSession> AgentInstallationSecuritySessionManager
            {
                get
                {
                    return agentInstallationSecuritySessionManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAdminViewWcfSession

        ///////////////////////// SINGLETON IAdminViewWcfSession

        private static readonly Lazy<AdminViewSecuritySessionManager> adminViewSecuritySessionManagerLazy = new Lazy<AdminViewSecuritySessionManager>(() => new AdminViewSecuritySessionManager());
        private static readonly Singleton<IAdminViewWcfSession> adminViewSecuritySessionManager = new Singleton<IAdminViewWcfSession>(() => adminViewSecuritySessionManagerLazy.Value );

        /// <summary>
        /// Реализация типа IAdminViewWcfSession
        /// </summary>
        internal static IAdminViewWcfSession AdminViewSecuritySessionManager
        {
            get
            {
                return Singletons.AdminViewSecuritySessionManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAdminViewWcfSession
            /// </summary>
            internal static Singleton<IAdminViewWcfSession> AdminViewSecuritySessionManager
            {
                get
                {
                    return adminViewSecuritySessionManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IConfigurationStorage

        ///////////////////////// SINGLETON IConfigurationStorage

        private static readonly Lazy<ConfigurationStorage> configurationStorageLazy = new Lazy<ConfigurationStorage>(() => new ConfigurationStorage());
        private static readonly Singleton<IConfigurationStorage> configurationStorage = new Singleton<IConfigurationStorage>(() => configurationStorageLazy.Value );

        /// <summary>
        /// Реализация типа IConfigurationStorage
        /// </summary>
        internal static IConfigurationStorage ConfigurationStorage
        {
            get
            {
                return Singletons.ConfigurationStorage.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IConfigurationStorage
            /// </summary>
            internal static Singleton<IConfigurationStorage> ConfigurationStorage
            {
                get
                {
                    return configurationStorage;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAdminKeysStorage

        ///////////////////////// SINGLETON IAdminKeysStorage

        private static readonly Lazy<AdminKeysStorage> adminKeysStorageLazy = new Lazy<AdminKeysStorage>(() => new AdminKeysStorage());
        private static readonly Singleton<IAdminKeysStorage> adminKeysStorage = new Singleton<IAdminKeysStorage>(() => adminKeysStorageLazy.Value );

        /// <summary>
        /// Реализация типа IAdminKeysStorage
        /// </summary>
        internal static IAdminKeysStorage AdminKeysStorage
        {
            get
            {
                return Singletons.AdminKeysStorage.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAdminKeysStorage
            /// </summary>
            internal static Singleton<IAdminKeysStorage> AdminKeysStorage
            {
                get
                {
                    return adminKeysStorage;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentSetupPasswordManager

        ///////////////////////// SINGLETON IAgentSetupPasswordManager

        private static readonly Lazy<AgentSetupPasswordManager> agentSetupPasswordManagerLazy = new Lazy<AgentSetupPasswordManager>(() => new AgentSetupPasswordManager());
        private static readonly Singleton<IAgentSetupPasswordManager> agentSetupPasswordManager = new Singleton<IAgentSetupPasswordManager>(() => agentSetupPasswordManagerLazy.Value );

        /// <summary>
        /// Реализация типа IAgentSetupPasswordManager
        /// </summary>
        internal static IAgentSetupPasswordManager AgentSetupPasswordManager
        {
            get
            {
                return Singletons.AgentSetupPasswordManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAgentSetupPasswordManager
            /// </summary>
            internal static Singleton<IAgentSetupPasswordManager> AgentSetupPasswordManager
            {
                get
                {
                    return agentSetupPasswordManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IMailer

        ///////////////////////// SINGLETON IMailer

        private static readonly Lazy<Mailer> mailerLazy = new Lazy<Mailer>(() => new Mailer());
        private static readonly Singleton<IMailer> mailer = new Singleton<IMailer>(() => mailerLazy.Value );

        /// <summary>
        /// Реализация типа IMailer
        /// </summary>
        internal static IMailer Mailer
        {
            get
            {
                return Singletons.Mailer.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IMailer
            /// </summary>
            internal static Singleton<IMailer> Mailer
            {
                get
                {
                    return mailer;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAccountResolver

        ///////////////////////// SINGLETON IAccountResolver

        private static readonly Lazy<AccountResolver> accountResolverLazy = new Lazy<AccountResolver>(() => new AccountResolver());
        private static readonly Singleton<IAccountResolver> accountResolver = new Singleton<IAccountResolver>(() => accountResolverLazy.Value );

        /// <summary>
        /// Реализация типа IAccountResolver
        /// </summary>
        internal static IAccountResolver AccountResolver
        {
            get
            {
                return Singletons.AccountResolver.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAccountResolver
            /// </summary>
            internal static Singleton<IAccountResolver> AccountResolver
            {
                get
                {
                    return accountResolver;
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


        #region IAgentsSetupListStorage

        ///////////////////////// SINGLETON IAgentsSetupListStorage

        private static readonly Lazy<AgentsSetupListStorage> agentsSetupListStorageLazy = new Lazy<AgentsSetupListStorage>(() => new AgentsSetupListStorage());
        private static readonly Singleton<IAgentsSetupListStorage> agentsSetupListStorage = new Singleton<IAgentsSetupListStorage>(() => agentsSetupListStorageLazy.Value );

        /// <summary>
        /// Реализация типа IAgentsSetupListStorage
        /// </summary>
        internal static IAgentsSetupListStorage AgentsSetupListStorage
        {
            get
            {
                return Singletons.AgentsSetupListStorage.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAgentsSetupListStorage
            /// </summary>
            internal static Singleton<IAgentsSetupListStorage> AgentsSetupListStorage
            {
                get
                {
                    return agentsSetupListStorage;
                }
            }
        }

        /////////////////////////

        #endregion

    }                                                                                         
}                                                                                             
                                                                                               

