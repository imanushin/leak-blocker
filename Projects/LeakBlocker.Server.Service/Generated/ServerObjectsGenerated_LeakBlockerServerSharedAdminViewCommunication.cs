using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.License;

namespace LeakBlocker.Server.Service.Generated
{
    internal abstract class GeneratedAgentInstallationTools : BaseServer
    {       
        private static readonly string name = "AgentInstallationTools_" + SharedObjects.Constants.VersionString;

        protected GeneratedAgentInstallationTools(ISecuritySessionManager securitySessionManager)
            : base(securitySessionManager)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        protected abstract void ForceInstallation(ReadOnlySet<BaseComputerAccount> computers);

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override byte[] ProcessRequest(BinaryReader inputStream)
        {
            var outStream = new MemoryStream();
            
            using (var writer = new BinaryWriter(outStream))
            {
                byte functionIndex = inputStream.ReadByte();
               
                switch (functionIndex)
                {
                    case 1://ForceInstallation
                        {
                            //Read input parameters
                            var computers = ObjectFormatter.Deserialize<ReadOnlySet<BaseComputerAccount>>(inputStream);

                            Check.ObjectIsNotNull(computers, "computers");
                     
                        
                            //Call function
                            ForceInstallation(computers);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeVoidResult(writer);                     
                        
                            break;
                        }

            
                    default:
                        throw new InvalidOperationException("Unable to retrive function from index {0}".Combine(functionIndex));
                }

                var totalLength = (int)outStream.Length;
                
                byte[] resultData = outStream.GetBuffer();
                
                Array.Resize(ref resultData, totalLength);
                
                return resultData;               
            }
            
        }

    }
    internal abstract class GeneratedAgentSetupPasswordTools : BaseServer
    {       
        private static readonly string name = "AgentSetupPasswordTools_" + SharedObjects.Constants.VersionString;

        protected GeneratedAgentSetupPasswordTools(ISecuritySessionManager securitySessionManager)
            : base(securitySessionManager)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        protected abstract AgentSetupPassword GetPassword();
        protected abstract void SendPassword(EmailSettings emailSettings);

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override byte[] ProcessRequest(BinaryReader inputStream)
        {
            var outStream = new MemoryStream();
            
            using (var writer = new BinaryWriter(outStream))
            {
                byte functionIndex = inputStream.ReadByte();
               
                switch (functionIndex)
                {
                    case 1://GetPassword
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            AgentSetupPassword result = GetPassword();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 2://SendPassword
                        {
                            //Read input parameters
                            var emailSettings = ObjectFormatter.Deserialize<EmailSettings>(inputStream);

                            Check.ObjectIsNotNull(emailSettings, "emailSettings");
                     
                        
                            //Call function
                            SendPassword(emailSettings);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeVoidResult(writer);                     
                        
                            break;
                        }

            
                    default:
                        throw new InvalidOperationException("Unable to retrive function from index {0}".Combine(functionIndex));
                }

                var totalLength = (int)outStream.Length;
                
                byte[] resultData = outStream.GetBuffer();
                
                Array.Resize(ref resultData, totalLength);
                
                return resultData;               
            }
            
        }

    }
    internal abstract class GeneratedAuditTools : BaseServer
    {       
        private static readonly string name = "AuditTools_" + SharedObjects.Constants.VersionString;

        protected GeneratedAuditTools(ISecuritySessionManager securitySessionManager)
            : base(securitySessionManager)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        protected abstract void SaveFilterSet(ReadOnlyList<AuditFilter> filters);
        protected abstract ReadOnlyList<AuditFilter> LoadFilters();
        protected abstract void CreateFilter(AuditFilter filter);
        protected abstract void DeleteFilter(AuditFilter filter);
        protected abstract void ChangeFilter(AuditFilter fromFilter, AuditFilter toFilter);
        protected abstract ReadOnlySet<DeviceDescription> GetAuditDevices();
        protected abstract ReadOnlyList<AuditItem> GetItemsForFilter(AuditFilter filter, Int32 topCount);

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override byte[] ProcessRequest(BinaryReader inputStream)
        {
            var outStream = new MemoryStream();
            
            using (var writer = new BinaryWriter(outStream))
            {
                byte functionIndex = inputStream.ReadByte();
               
                switch (functionIndex)
                {
                    case 1://SaveFilterSet
                        {
                            //Read input parameters
                            var filters = ObjectFormatter.Deserialize<ReadOnlyList<AuditFilter>>(inputStream);

                            Check.ObjectIsNotNull(filters, "filters");
                     
                        
                            //Call function
                            SaveFilterSet(filters);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeVoidResult(writer);                     
                        
                            break;
                        }

                    case 2://LoadFilters
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            ReadOnlyList<AuditFilter> result = LoadFilters();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 3://CreateFilter
                        {
                            //Read input parameters
                            var filter = ObjectFormatter.Deserialize<AuditFilter>(inputStream);

                            Check.ObjectIsNotNull(filter, "filter");
                     
                        
                            //Call function
                            CreateFilter(filter);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeVoidResult(writer);                     
                        
                            break;
                        }

                    case 4://DeleteFilter
                        {
                            //Read input parameters
                            var filter = ObjectFormatter.Deserialize<AuditFilter>(inputStream);

                            Check.ObjectIsNotNull(filter, "filter");
                     
                        
                            //Call function
                            DeleteFilter(filter);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeVoidResult(writer);                     
                        
                            break;
                        }

                    case 5://ChangeFilter
                        {
                            //Read input parameters
                            var fromFilter = ObjectFormatter.Deserialize<AuditFilter>(inputStream);
                            var toFilter = ObjectFormatter.Deserialize<AuditFilter>(inputStream);

                            Check.ObjectIsNotNull(fromFilter, "fromFilter");
                            Check.ObjectIsNotNull(toFilter, "toFilter");
                     
                        
                            //Call function
                            ChangeFilter(fromFilter, toFilter);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeVoidResult(writer);                     
                        
                            break;
                        }

                    case 6://GetAuditDevices
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            ReadOnlySet<DeviceDescription> result = GetAuditDevices();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 7://GetItemsForFilter
                        {
                            //Read input parameters
                            var filter = ObjectFormatter.Deserialize<AuditFilter>(inputStream);
                            var topCount = ObjectFormatter.DeserializeInt(inputStream);

                            Check.ObjectIsNotNull(filter, "filter");
                     
                        
                            //Call function
                            ReadOnlyList<AuditItem> result = GetItemsForFilter(filter, topCount);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

            
                    default:
                        throw new InvalidOperationException("Unable to retrive function from index {0}".Combine(functionIndex));
                }

                var totalLength = (int)outStream.Length;
                
                byte[] resultData = outStream.GetBuffer();
                
                Array.Resize(ref resultData, totalLength);
                
                return resultData;               
            }
            
        }

    }
    internal abstract class GeneratedAccountTools : BaseServer
    {       
        private static readonly string name = "AccountTools_" + SharedObjects.Constants.VersionString;

        protected GeneratedAccountTools(ISecuritySessionManager securitySessionManager)
            : base(securitySessionManager)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        protected abstract ReadOnlySet<String> FindDnsDomains();
        protected abstract DomainUpdateRequest CheckAndSetCredentials(DomainCredentials credentials);
        protected abstract ReadOnlySet<Scope> GetAvailableComputerScopes();
        protected abstract ReadOnlySet<Scope> GetAvailableUserScopes();
        protected abstract ReadOnlySet<ResultComputer> GetComputers(ReadOnlySet<Scope> scope);
        protected abstract ReadOnlySet<BaseComputerAccount> GetAvailableComputers();
        protected abstract ReadOnlySet<BaseUserAccount> GetAvailableUsers();
        protected abstract String GetPreferableDomain();
        protected abstract Boolean IsRequestCompleted(DomainUpdateRequest request);
        protected abstract UserContactInformation GetCurrentUserInformation();

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override byte[] ProcessRequest(BinaryReader inputStream)
        {
            var outStream = new MemoryStream();
            
            using (var writer = new BinaryWriter(outStream))
            {
                byte functionIndex = inputStream.ReadByte();
               
                switch (functionIndex)
                {
                    case 1://FindDnsDomains
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            ReadOnlySet<String> result = FindDnsDomains();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 2://CheckAndSetCredentials
                        {
                            //Read input parameters
                            var credentials = ObjectFormatter.Deserialize<DomainCredentials>(inputStream);

                            Check.ObjectIsNotNull(credentials, "credentials");
                     
                        
                            //Call function
                            DomainUpdateRequest result = CheckAndSetCredentials(credentials);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 3://GetAvailableComputerScopes
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            ReadOnlySet<Scope> result = GetAvailableComputerScopes();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 4://GetAvailableUserScopes
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            ReadOnlySet<Scope> result = GetAvailableUserScopes();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 5://GetComputers
                        {
                            //Read input parameters
                            var scope = ObjectFormatter.Deserialize<ReadOnlySet<Scope>>(inputStream);

                            Check.ObjectIsNotNull(scope, "scope");
                     
                        
                            //Call function
                            ReadOnlySet<ResultComputer> result = GetComputers(scope);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 6://GetAvailableComputers
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            ReadOnlySet<BaseComputerAccount> result = GetAvailableComputers();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 7://GetAvailableUsers
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            ReadOnlySet<BaseUserAccount> result = GetAvailableUsers();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 8://GetPreferableDomain
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            String result = GetPreferableDomain();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 9://IsRequestCompleted
                        {
                            //Read input parameters
                            var request = ObjectFormatter.Deserialize<DomainUpdateRequest>(inputStream);

                            Check.ObjectIsNotNull(request, "request");
                     
                        
                            //Call function
                            Boolean result = IsRequestCompleted(request);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 10://GetCurrentUserInformation
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            UserContactInformation result = GetCurrentUserInformation();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

            
                    default:
                        throw new InvalidOperationException("Unable to retrive function from index {0}".Combine(functionIndex));
                }

                var totalLength = (int)outStream.Length;
                
                byte[] resultData = outStream.GetBuffer();
                
                Array.Resize(ref resultData, totalLength);
                
                return resultData;               
            }
            
        }

    }
    internal abstract class GeneratedConfigurationTools : BaseServer
    {       
        private static readonly string name = "ConfigurationTools_" + SharedObjects.Constants.VersionString;

        protected GeneratedConfigurationTools(ISecuritySessionManager securitySessionManager)
            : base(securitySessionManager)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        protected abstract SimpleConfiguration LastConfiguration();
        protected abstract void SaveConfiguration(SimpleConfiguration configuration);
        protected abstract Boolean HasConfiguration();

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override byte[] ProcessRequest(BinaryReader inputStream)
        {
            var outStream = new MemoryStream();
            
            using (var writer = new BinaryWriter(outStream))
            {
                byte functionIndex = inputStream.ReadByte();
               
                switch (functionIndex)
                {
                    case 1://LastConfiguration
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            SimpleConfiguration result = LastConfiguration();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 2://SaveConfiguration
                        {
                            //Read input parameters
                            var configuration = ObjectFormatter.Deserialize<SimpleConfiguration>(inputStream);

                            Check.ObjectIsNotNull(configuration, "configuration");
                     
                        
                            //Call function
                            SaveConfiguration(configuration);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeVoidResult(writer);                     
                        
                            break;
                        }

                    case 3://HasConfiguration
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            Boolean result = HasConfiguration();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

            
                    default:
                        throw new InvalidOperationException("Unable to retrive function from index {0}".Combine(functionIndex));
                }

                var totalLength = (int)outStream.Length;
                
                byte[] resultData = outStream.GetBuffer();
                
                Array.Resize(ref resultData, totalLength);
                
                return resultData;               
            }
            
        }

    }
    internal abstract class GeneratedDeviceTools : BaseServer
    {       
        private static readonly string name = "DeviceTools_" + SharedObjects.Constants.VersionString;

        protected GeneratedDeviceTools(ISecuritySessionManager securitySessionManager)
            : base(securitySessionManager)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        protected abstract ReadOnlySet<DeviceDescription> GetConnectedDevices();

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override byte[] ProcessRequest(BinaryReader inputStream)
        {
            var outStream = new MemoryStream();
            
            using (var writer = new BinaryWriter(outStream))
            {
                byte functionIndex = inputStream.ReadByte();
               
                switch (functionIndex)
                {
                    case 1://GetConnectedDevices
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            ReadOnlySet<DeviceDescription> result = GetConnectedDevices();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

            
                    default:
                        throw new InvalidOperationException("Unable to retrive function from index {0}".Combine(functionIndex));
                }

                var totalLength = (int)outStream.Length;
                
                byte[] resultData = outStream.GetBuffer();
                
                Array.Resize(ref resultData, totalLength);
                
                return resultData;               
            }
            
        }

    }
    internal abstract class GeneratedLicenseTools : BaseServer
    {       
        private static readonly string name = "LicenseTools_" + SharedObjects.Constants.VersionString;

        protected GeneratedLicenseTools(ISecuritySessionManager securitySessionManager)
            : base(securitySessionManager)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        protected abstract ReadOnlySet<LicenseInfo> GetAllActualLicenses();
        protected abstract void AddLicense(LicenseInfo licenseInfo);

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override byte[] ProcessRequest(BinaryReader inputStream)
        {
            var outStream = new MemoryStream();
            
            using (var writer = new BinaryWriter(outStream))
            {
                byte functionIndex = inputStream.ReadByte();
               
                switch (functionIndex)
                {
                    case 1://GetAllActualLicenses
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            ReadOnlySet<LicenseInfo> result = GetAllActualLicenses();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 2://AddLicense
                        {
                            //Read input parameters
                            var licenseInfo = ObjectFormatter.Deserialize<LicenseInfo>(inputStream);

                            Check.ObjectIsNotNull(licenseInfo, "licenseInfo");
                     
                        
                            //Call function
                            AddLicense(licenseInfo);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeVoidResult(writer);                     
                        
                            break;
                        }

            
                    default:
                        throw new InvalidOperationException("Unable to retrive function from index {0}".Combine(functionIndex));
                }

                var totalLength = (int)outStream.Length;
                
                byte[] resultData = outStream.GetBuffer();
                
                Array.Resize(ref resultData, totalLength);
                
                return resultData;               
            }
            
        }

    }
    internal abstract class GeneratedReportTools : BaseServer
    {       
        private static readonly string name = "ReportTools_" + SharedObjects.Constants.VersionString;

        protected GeneratedReportTools(ISecuritySessionManager securitySessionManager)
            : base(securitySessionManager)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        protected abstract ReportConfiguration LoadSettings();
        protected abstract void SaveSettings(ReportConfiguration settings);
        protected abstract String TrySendTestReport(ReportConfiguration configuration);

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override byte[] ProcessRequest(BinaryReader inputStream)
        {
            var outStream = new MemoryStream();
            
            using (var writer = new BinaryWriter(outStream))
            {
                byte functionIndex = inputStream.ReadByte();
               
                switch (functionIndex)
                {
                    case 1://LoadSettings
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            ReportConfiguration result = LoadSettings();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 2://SaveSettings
                        {
                            //Read input parameters
                            var settings = ObjectFormatter.Deserialize<ReportConfiguration>(inputStream);

                            Check.ObjectIsNotNull(settings, "settings");
                     
                        
                            //Call function
                            SaveSettings(settings);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeVoidResult(writer);                     
                        
                            break;
                        }

                    case 3://TrySendTestReport
                        {
                            //Read input parameters
                            var configuration = ObjectFormatter.Deserialize<ReportConfiguration>(inputStream);

                            Check.ObjectIsNotNull(configuration, "configuration");
                     
                        
                            //Call function
                            String result = TrySendTestReport(configuration);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

            
                    default:
                        throw new InvalidOperationException("Unable to retrive function from index {0}".Combine(functionIndex));
                }

                var totalLength = (int)outStream.Length;
                
                byte[] resultData = outStream.GetBuffer();
                
                Array.Resize(ref resultData, totalLength);
                
                return resultData;               
            }
            
        }

    }
    internal abstract class GeneratedStatusTools : BaseServer
    {       
        private static readonly string name = "StatusTools_" + SharedObjects.Constants.VersionString;

        protected GeneratedStatusTools(ISecuritySessionManager securitySessionManager)
            : base(securitySessionManager)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        protected abstract ReadOnlySet<ManagedComputer> GetStatuses();

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override byte[] ProcessRequest(BinaryReader inputStream)
        {
            var outStream = new MemoryStream();
            
            using (var writer = new BinaryWriter(outStream))
            {
                byte functionIndex = inputStream.ReadByte();
               
                switch (functionIndex)
                {
                    case 1://GetStatuses
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            ReadOnlySet<ManagedComputer> result = GetStatuses();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

            
                    default:
                        throw new InvalidOperationException("Unable to retrive function from index {0}".Combine(functionIndex));
                }

                var totalLength = (int)outStream.Length;
                
                byte[] resultData = outStream.GetBuffer();
                
                Array.Resize(ref resultData, totalLength);
                
                return resultData;               
            }
            
        }

    }
    internal abstract class GeneratedLocalKeyAgreement : BaseServer
    {       
        private static readonly string name = "LocalKeyAgreement_" + SharedObjects.Constants.VersionString;

        protected GeneratedLocalKeyAgreement(ISecuritySessionManager securitySessionManager)
            : base(securitySessionManager)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        protected abstract Int32 RegisterUser(String fullUserName, AccountSecurityIdentifier userSid, Time timeMark);

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override byte[] ProcessRequest(BinaryReader inputStream)
        {
            var outStream = new MemoryStream();
            
            using (var writer = new BinaryWriter(outStream))
            {
                byte functionIndex = inputStream.ReadByte();
               
                switch (functionIndex)
                {
                    case 1://RegisterUser
                        {
                            //Read input parameters
                            var fullUserName = ObjectFormatter.DeserializeString(inputStream);
                            var userSid = ObjectFormatter.Deserialize<AccountSecurityIdentifier>(inputStream);
                            var timeMark = ObjectFormatter.Deserialize<Time>(inputStream);

                            Check.StringIsMeaningful(fullUserName, "fullUserName");
                            Check.ObjectIsNotNull(userSid, "userSid");
                            Check.ObjectIsNotNull(timeMark, "timeMark");
                     
                        
                            //Call function
                            Int32 result = RegisterUser(fullUserName, userSid, timeMark);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

            
                    default:
                        throw new InvalidOperationException("Unable to retrive function from index {0}".Combine(functionIndex));
                }

                var totalLength = (int)outStream.Length;
                
                byte[] resultData = outStream.GetBuffer();
                
                Array.Resize(ref resultData, totalLength);
                
                return resultData;               
            }
            
        }

    }

}
