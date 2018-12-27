
using LeakBlocker.AdminView.Desktop.Common;
using LeakBlocker.AdminView.Desktop.Controls.Common.Converters;
using LeakBlocker.AdminView.Desktop.Controls.Standard.Audit;
using LeakBlocker.AdminView.Desktop.Network;
using LeakBlocker.Libraries.Common.CommonInterfaces;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.KeysAgreement;

/*
 * */

//
using System;
using LeakBlocker.Libraries.Common.IoC;

namespace LeakBlocker.AdminView.Desktop
{
    /// <summary>
    /// Набор реализаций Singleton-объектов.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class UiObjects
    {

        #region IStatusTools

        ///////////////////////// FACTORY IStatusTools

        private static readonly Factory<IStatusTools> statusToolsClient = new Factory<IStatusTools>(() => new StatusToolsClient());

        /// <summary>
        /// Реализация типа IStatusTools
        /// </summary>
        internal static IStatusTools CreateStatusToolsClient()
        {
            return Factories.StatusToolsClient.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IStatusTools
            /// </summary>
            internal static Factory<IStatusTools> StatusToolsClient
            {
                get
                {
                    return statusToolsClient;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAccountTools

        ///////////////////////// FACTORY IAccountTools

        private static readonly Factory<IAccountTools> accountToolsClient = new Factory<IAccountTools>(() => new AccountToolsClient());

        /// <summary>
        /// Реализация типа IAccountTools
        /// </summary>
        internal static IAccountTools CreateAccountToolsClient()
        {
            return Factories.AccountToolsClient.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IAccountTools
            /// </summary>
            internal static Factory<IAccountTools> AccountToolsClient
            {
                get
                {
                    return accountToolsClient;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentInstallationTools

        ///////////////////////// FACTORY IAgentInstallationTools

        private static readonly Factory<IAgentInstallationTools> agentInstallationToolsClient = new Factory<IAgentInstallationTools>(() => new AgentInstallationToolsClient());

        /// <summary>
        /// Реализация типа IAgentInstallationTools
        /// </summary>
        internal static IAgentInstallationTools CreateAgentInstallationToolsClient()
        {
            return Factories.AgentInstallationToolsClient.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IAgentInstallationTools
            /// </summary>
            internal static Factory<IAgentInstallationTools> AgentInstallationToolsClient
            {
                get
                {
                    return agentInstallationToolsClient;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IDeviceTools

        ///////////////////////// FACTORY IDeviceTools

        private static readonly Factory<IDeviceTools> deviceToolsClient = new Factory<IDeviceTools>(() => new DeviceToolsClient());

        /// <summary>
        /// Реализация типа IDeviceTools
        /// </summary>
        internal static IDeviceTools CreateDeviceToolsClient()
        {
            return Factories.DeviceToolsClient.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IDeviceTools
            /// </summary>
            internal static Factory<IDeviceTools> DeviceToolsClient
            {
                get
                {
                    return deviceToolsClient;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IConfigurationTools

        ///////////////////////// FACTORY IConfigurationTools

        private static readonly Factory<IConfigurationTools> configurationToolsClient = new Factory<IConfigurationTools>(() => new ConfigurationToolsClient());

        /// <summary>
        /// Реализация типа IConfigurationTools
        /// </summary>
        internal static IConfigurationTools CreateConfigurationToolsClient()
        {
            return Factories.ConfigurationToolsClient.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IConfigurationTools
            /// </summary>
            internal static Factory<IConfigurationTools> ConfigurationToolsClient
            {
                get
                {
                    return configurationToolsClient;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAuditTools

        ///////////////////////// FACTORY IAuditTools

        private static readonly Factory<IAuditTools> auditToolsClient = new Factory<IAuditTools>(() => new AuditToolsClient());

        /// <summary>
        /// Реализация типа IAuditTools
        /// </summary>
        internal static IAuditTools CreateAuditToolsClient()
        {
            return Factories.AuditToolsClient.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IAuditTools
            /// </summary>
            internal static Factory<IAuditTools> AuditToolsClient
            {
                get
                {
                    return auditToolsClient;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IReportTools

        ///////////////////////// FACTORY IReportTools

        private static readonly Factory<IReportTools> reportToolsClient = new Factory<IReportTools>(() => new ReportToolsClient());

        /// <summary>
        /// Реализация типа IReportTools
        /// </summary>
        internal static IReportTools CreateReportToolsClient()
        {
            return Factories.ReportToolsClient.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IReportTools
            /// </summary>
            internal static Factory<IReportTools> ReportToolsClient
            {
                get
                {
                    return reportToolsClient;
                }
            }
        }

        /////////////////////////

        #endregion


        #region ILicenseTools

        ///////////////////////// FACTORY ILicenseTools

        private static readonly Factory<ILicenseTools> licenseToolsClient = new Factory<ILicenseTools>(() => new LicenseToolsClient());

        /// <summary>
        /// Реализация типа ILicenseTools
        /// </summary>
        internal static ILicenseTools CreateLicenseToolsClient()
        {
            return Factories.LicenseToolsClient.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа ILicenseTools
            /// </summary>
            internal static Factory<ILicenseTools> LicenseToolsClient
            {
                get
                {
                    return licenseToolsClient;
                }
            }
        }

        /////////////////////////

        #endregion


        #region ILocalKeyAgreement

        ///////////////////////// FACTORY ILocalKeyAgreement

        private static readonly Factory<ILocalKeyAgreement> localKeyAgreementClient = new Factory<ILocalKeyAgreement>(() => new LocalKeyAgreementClient());

        /// <summary>
        /// Реализация типа ILocalKeyAgreement
        /// </summary>
        internal static ILocalKeyAgreement CreateLocalKeyAgreementClient()
        {
            return Factories.LocalKeyAgreementClient.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа ILocalKeyAgreement
            /// </summary>
            internal static Factory<ILocalKeyAgreement> LocalKeyAgreementClient
            {
                get
                {
                    return localKeyAgreementClient;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAgentSetupPasswordTools

        ///////////////////////// FACTORY IAgentSetupPasswordTools

        private static readonly Factory<IAgentSetupPasswordTools> agentSetupPasswordToolsClient = new Factory<IAgentSetupPasswordTools>(() => new AgentSetupPasswordToolsClient());

        /// <summary>
        /// Реализация типа IAgentSetupPasswordTools
        /// </summary>
        internal static IAgentSetupPasswordTools CreateAgentSetupPasswordToolsClient()
        {
            return Factories.AgentSetupPasswordToolsClient.CreateInstance();
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IAgentSetupPasswordTools
            /// </summary>
            internal static Factory<IAgentSetupPasswordTools> AgentSetupPasswordToolsClient
            {
                get
                {
                    return agentSetupPasswordToolsClient;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IDomainCache

        ///////////////////////// SINGLETON IDomainCache

        private static readonly Lazy<DomainCache> domainCacheLazy = new Lazy<DomainCache>(() => new DomainCache());
        private static readonly Singleton<IDomainCache> domainCache = new Singleton<IDomainCache>(() => domainCacheLazy.Value );

        /// <summary>
        /// Реализация типа IDomainCache
        /// </summary>
        internal static IDomainCache DomainCache
        {
            get
            {
                return Singletons.DomainCache.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IDomainCache
            /// </summary>
            internal static Singleton<IDomainCache> DomainCache
            {
                get
                {
                    return domainCache;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IUiConfigurationManager

        ///////////////////////// SINGLETON IUiConfigurationManager

        private static readonly Lazy<UiConfigurationManager> uiConfigurationManagerLazy = new Lazy<UiConfigurationManager>(() => new UiConfigurationManager());
        private static readonly Singleton<IUiConfigurationManager> uiConfigurationManager = new Singleton<IUiConfigurationManager>(() => uiConfigurationManagerLazy.Value );

        /// <summary>
        /// Реализация типа IUiConfigurationManager
        /// </summary>
        internal static IUiConfigurationManager UiConfigurationManager
        {
            get
            {
                return Singletons.UiConfigurationManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IUiConfigurationManager
            /// </summary>
            internal static Singleton<IUiConfigurationManager> UiConfigurationManager
            {
                get
                {
                    return uiConfigurationManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region ScopeToImageConverter

        ///////////////////////// SINGLETON ScopeToImageConverter

        private static readonly Lazy<ScopeToImageConverter> scopeToImageConverterLazy = new Lazy<ScopeToImageConverter>(() => new ScopeToImageConverter());
        private static readonly Singleton<ScopeToImageConverter> scopeToImageConverter = new Singleton<ScopeToImageConverter>(() => scopeToImageConverterLazy.Value );

        /// <summary>
        /// Реализация типа ScopeToImageConverter
        /// </summary>
        internal static ScopeToImageConverter ScopeToImageConverter
        {
            get
            {
                return Singletons.ScopeToImageConverter.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа ScopeToImageConverter
            /// </summary>
            internal static Singleton<ScopeToImageConverter> ScopeToImageConverter
            {
                get
                {
                    return scopeToImageConverter;
                }
            }
        }

        /////////////////////////

        #endregion


        #region DeviceToImageConverter

        ///////////////////////// SINGLETON DeviceToImageConverter

        private static readonly Lazy<DeviceToImageConverter> deviceToImageConverterLazy = new Lazy<DeviceToImageConverter>(() => new DeviceToImageConverter());
        private static readonly Singleton<DeviceToImageConverter> deviceToImageConverter = new Singleton<DeviceToImageConverter>(() => deviceToImageConverterLazy.Value );

        /// <summary>
        /// Реализация типа DeviceToImageConverter
        /// </summary>
        internal static DeviceToImageConverter DeviceToImageConverter
        {
            get
            {
                return Singletons.DeviceToImageConverter.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа DeviceToImageConverter
            /// </summary>
            internal static Singleton<DeviceToImageConverter> DeviceToImageConverter
            {
                get
                {
                    return deviceToImageConverter;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAdminKeyStorage

        ///////////////////////// SINGLETON IAdminKeyStorage

        private static readonly Lazy<AdminKeyStorage> adminKeyStorageLazy = new Lazy<AdminKeyStorage>(() => new AdminKeyStorage());
        private static readonly Singleton<IAdminKeyStorage> adminKeyStorage = new Singleton<IAdminKeyStorage>(() => adminKeyStorageLazy.Value );

        /// <summary>
        /// Реализация типа IAdminKeyStorage
        /// </summary>
        internal static IAdminKeyStorage AdminKeyStorage
        {
            get
            {
                return Singletons.AdminKeyStorage.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAdminKeyStorage
            /// </summary>
            internal static Singleton<IAdminKeyStorage> AdminKeyStorage
            {
                get
                {
                    return adminKeyStorage;
                }
            }
        }

        /////////////////////////

        #endregion

    }                                                                                         
}                                                                                             
                                                                                               

