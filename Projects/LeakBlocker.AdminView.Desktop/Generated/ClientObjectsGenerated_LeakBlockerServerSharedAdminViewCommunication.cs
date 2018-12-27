using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.License;
using LeakBlocker.ServerShared.AdminViewCommunication.KeysAgreement;

// ReSharper disable UnusedVariable

namespace LeakBlocker.AdminView.Desktop.Generated
{
    internal abstract class AgentInstallationToolsClientGenerated : BaseClient, IAgentInstallationTools
    {
        private static readonly string name = "AgentInstallationTools_" + SharedObjects.Constants.VersionString;

        protected AgentInstallationToolsClientGenerated(SymmetricEncryptionKey key, byte[] sharedToken) 
            : base(key, sharedToken)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        void IAgentInstallationTools.ForceInstallation(ReadOnlySet<BaseComputerAccount> computers)
        {
            Check.ObjectIsNotNull(computers, "computers");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)1);//Index of method ForceInstallation
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, computers);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
           
                }
            }

        }

    }
    internal abstract class AgentSetupPasswordToolsClientGenerated : BaseClient, IAgentSetupPasswordTools
    {
        private static readonly string name = "AgentSetupPasswordTools_" + SharedObjects.Constants.VersionString;

        protected AgentSetupPasswordToolsClientGenerated(SymmetricEncryptionKey key, byte[] sharedToken) 
            : base(key, sharedToken)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        AgentSetupPassword IAgentSetupPasswordTools.GetPassword()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)1);//Index of method GetPassword
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<AgentSetupPassword>(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        void IAgentSetupPasswordTools.SendPassword(EmailSettings emailSettings)
        {
            Check.ObjectIsNotNull(emailSettings, "emailSettings");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)2);//Index of method SendPassword
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, emailSettings);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
           
                }
            }

        }

    }
    internal abstract class AuditToolsClientGenerated : BaseClient, IAuditTools
    {
        private static readonly string name = "AuditTools_" + SharedObjects.Constants.VersionString;

        protected AuditToolsClientGenerated(SymmetricEncryptionKey key, byte[] sharedToken) 
            : base(key, sharedToken)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        void IAuditTools.SaveFilterSet(ReadOnlyList<AuditFilter> filters)
        {
            Check.ObjectIsNotNull(filters, "filters");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)1);//Index of method SaveFilterSet
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, filters);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        ReadOnlyList<AuditFilter> IAuditTools.LoadFilters()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)2);//Index of method LoadFilters
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<ReadOnlyList<AuditFilter>>(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        void IAuditTools.CreateFilter(AuditFilter filter)
        {
            Check.ObjectIsNotNull(filter, "filter");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)3);//Index of method CreateFilter
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, filter);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        void IAuditTools.DeleteFilter(AuditFilter filter)
        {
            Check.ObjectIsNotNull(filter, "filter");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)4);//Index of method DeleteFilter
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, filter);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        void IAuditTools.ChangeFilter(AuditFilter fromFilter, AuditFilter toFilter)
        {
            Check.ObjectIsNotNull(fromFilter, "fromFilter");
            Check.ObjectIsNotNull(toFilter, "toFilter");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)5);//Index of method ChangeFilter
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, fromFilter);
                ObjectFormatter.SerializeParameter(writer, toFilter);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        ReadOnlySet<DeviceDescription> IAuditTools.GetAuditDevices()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)6);//Index of method GetAuditDevices
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<ReadOnlySet<DeviceDescription>>(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        ReadOnlyList<AuditItem> IAuditTools.GetItemsForFilter(AuditFilter filter, Int32 topCount)
        {
            Check.ObjectIsNotNull(filter, "filter");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)7);//Index of method GetItemsForFilter
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, filter);
                ObjectFormatter.SerializeParameter(writer, topCount);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<ReadOnlyList<AuditItem>>(resultStream);           
                }
            }

        }

    }
    internal abstract class AccountToolsClientGenerated : BaseClient, IAccountTools
    {
        private static readonly string name = "AccountTools_" + SharedObjects.Constants.VersionString;

        protected AccountToolsClientGenerated(SymmetricEncryptionKey key, byte[] sharedToken) 
            : base(key, sharedToken)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        ReadOnlySet<String> IAccountTools.FindDnsDomains()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)1);//Index of method FindDnsDomains
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<ReadOnlySet<String>>(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        DomainUpdateRequest IAccountTools.CheckAndSetCredentials(DomainCredentials credentials)
        {
            Check.ObjectIsNotNull(credentials, "credentials");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)2);//Index of method CheckAndSetCredentials
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, credentials);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<DomainUpdateRequest>(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        ReadOnlySet<Scope> IAccountTools.GetAvailableComputerScopes()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)3);//Index of method GetAvailableComputerScopes
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<ReadOnlySet<Scope>>(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        ReadOnlySet<Scope> IAccountTools.GetAvailableUserScopes()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)4);//Index of method GetAvailableUserScopes
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<ReadOnlySet<Scope>>(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        ReadOnlySet<ResultComputer> IAccountTools.GetComputers(ReadOnlySet<Scope> scope)
        {
            Check.ObjectIsNotNull(scope, "scope");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)5);//Index of method GetComputers
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, scope);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<ReadOnlySet<ResultComputer>>(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        ReadOnlySet<BaseComputerAccount> IAccountTools.GetAvailableComputers()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)6);//Index of method GetAvailableComputers
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<ReadOnlySet<BaseComputerAccount>>(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        ReadOnlySet<BaseUserAccount> IAccountTools.GetAvailableUsers()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)7);//Index of method GetAvailableUsers
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<ReadOnlySet<BaseUserAccount>>(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        String IAccountTools.GetPreferableDomain()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)8);//Index of method GetPreferableDomain
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.DeserializeString(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        Boolean IAccountTools.IsRequestCompleted(DomainUpdateRequest request)
        {
            Check.ObjectIsNotNull(request, "request");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)9);//Index of method IsRequestCompleted
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, request);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.DeserializeBool(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        UserContactInformation IAccountTools.GetCurrentUserInformation()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)10);//Index of method GetCurrentUserInformation
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<UserContactInformation>(resultStream);           
                }
            }

        }

    }
    internal abstract class ConfigurationToolsClientGenerated : BaseClient, IConfigurationTools
    {
        private static readonly string name = "ConfigurationTools_" + SharedObjects.Constants.VersionString;

        protected ConfigurationToolsClientGenerated(SymmetricEncryptionKey key, byte[] sharedToken) 
            : base(key, sharedToken)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        SimpleConfiguration IConfigurationTools.LastConfiguration()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)1);//Index of method LastConfiguration
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<SimpleConfiguration>(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        void IConfigurationTools.SaveConfiguration(SimpleConfiguration configuration)
        {
            Check.ObjectIsNotNull(configuration, "configuration");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)2);//Index of method SaveConfiguration
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, configuration);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        Boolean IConfigurationTools.HasConfiguration()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)3);//Index of method HasConfiguration
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.DeserializeBool(resultStream);           
                }
            }

        }

    }
    internal abstract class DeviceToolsClientGenerated : BaseClient, IDeviceTools
    {
        private static readonly string name = "DeviceTools_" + SharedObjects.Constants.VersionString;

        protected DeviceToolsClientGenerated(SymmetricEncryptionKey key, byte[] sharedToken) 
            : base(key, sharedToken)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        ReadOnlySet<DeviceDescription> IDeviceTools.GetConnectedDevices()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)1);//Index of method GetConnectedDevices
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<ReadOnlySet<DeviceDescription>>(resultStream);           
                }
            }

        }

    }
    internal abstract class LicenseToolsClientGenerated : BaseClient, ILicenseTools
    {
        private static readonly string name = "LicenseTools_" + SharedObjects.Constants.VersionString;

        protected LicenseToolsClientGenerated(SymmetricEncryptionKey key, byte[] sharedToken) 
            : base(key, sharedToken)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        ReadOnlySet<LicenseInfo> ILicenseTools.GetAllActualLicenses()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)1);//Index of method GetAllActualLicenses
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<ReadOnlySet<LicenseInfo>>(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        void ILicenseTools.AddLicense(LicenseInfo licenseInfo)
        {
            Check.ObjectIsNotNull(licenseInfo, "licenseInfo");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)2);//Index of method AddLicense
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, licenseInfo);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
           
                }
            }

        }

    }
    internal abstract class ReportToolsClientGenerated : BaseClient, IReportTools
    {
        private static readonly string name = "ReportTools_" + SharedObjects.Constants.VersionString;

        protected ReportToolsClientGenerated(SymmetricEncryptionKey key, byte[] sharedToken) 
            : base(key, sharedToken)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        ReportConfiguration IReportTools.LoadSettings()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)1);//Index of method LoadSettings
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<ReportConfiguration>(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        void IReportTools.SaveSettings(ReportConfiguration settings)
        {
            Check.ObjectIsNotNull(settings, "settings");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)2);//Index of method SaveSettings
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, settings);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        String IReportTools.TrySendTestReport(ReportConfiguration configuration)
        {
            Check.ObjectIsNotNull(configuration, "configuration");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)3);//Index of method TrySendTestReport
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, configuration);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.DeserializeString(resultStream);           
                }
            }

        }

    }
    internal abstract class StatusToolsClientGenerated : BaseClient, IStatusTools
    {
        private static readonly string name = "StatusTools_" + SharedObjects.Constants.VersionString;

        protected StatusToolsClientGenerated(SymmetricEncryptionKey key, byte[] sharedToken) 
            : base(key, sharedToken)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        ReadOnlySet<ManagedComputer> IStatusTools.GetStatuses()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)1);//Index of method GetStatuses
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<ReadOnlySet<ManagedComputer>>(resultStream);           
                }
            }

        }

    }
    internal abstract class LocalKeyAgreementClientGenerated : BaseClient, ILocalKeyAgreement
    {
        private static readonly string name = "LocalKeyAgreement_" + SharedObjects.Constants.VersionString;

        protected LocalKeyAgreementClientGenerated(SymmetricEncryptionKey key, byte[] sharedToken) 
            : base(key, sharedToken)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        Int32 ILocalKeyAgreement.RegisterUser(String fullUserName, AccountSecurityIdentifier userSid, Time timeMark)
        {
            Check.StringIsMeaningful(fullUserName, "fullUserName");
            Check.ObjectIsNotNull(userSid, "userSid");
            Check.ObjectIsNotNull(timeMark, "timeMark");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)1);//Index of method RegisterUser
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, fullUserName);
                ObjectFormatter.SerializeParameter(writer, userSid);
                ObjectFormatter.SerializeParameter(writer, timeMark);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.DeserializeInt(resultStream);           
                }
            }

        }

    }

}

// ReSharper restore UnusedVariable

