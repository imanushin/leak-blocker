
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Audit;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.TemporaryAccess;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable ConvertToConstant.Local
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable InconsistentNaming
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable UnusedVariable
// ReSharper disable RedundantCast
// ReSharper disable UnusedMember.Global

#pragma warning disable 67
#pragma warning disable 219


namespace LeakBlocker.ServerShared.AdminViewCommunication.Tests
{

    [TestClass]
    public sealed partial class AgentSetupPasswordTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AgentSetupPassword> objects = new ObjectsCache<AgentSetupPassword>(GetInstances);

        internal static AgentSetupPassword First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AgentSetupPassword Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AgentSetupPassword Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AgentSetupPassword_CheckEmptyStringArg_value()
        {
            CheckEmptyStringArg_value(string.Empty);
            CheckEmptyStringArg_value("    ");
            CheckEmptyStringArg_value(Environment.NewLine);
            CheckEmptyStringArg_value("\n\r");
        }

        private void CheckEmptyStringArg_value(string stringArgument)
        {
            var value = "text 135346";

            try
            {
                new AgentSetupPassword(stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "value", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'value' isn't checked for emply values");
        }

        [TestMethod]
        public void AgentSetupPassword_CheckNullArg_value()
        {
            var value = "text 135346";

            try
            {
                new AgentSetupPassword(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "value", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'value' isn't checked for null inputs");
        }


        [TestMethod]
        public void AgentSetupPassword_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AgentSetupPassword_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AgentSetupPassword_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AgentSetupPassword_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class DomainUpdateRequestTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DomainUpdateRequest> objects = new ObjectsCache<DomainUpdateRequest>(GetInstances);

        internal static DomainUpdateRequest First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DomainUpdateRequest Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DomainUpdateRequest Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DomainUpdateRequest_CheckNullArg_domain()
        {
            var domain = BaseDomainAccountTest.First;
            var requestIdentifier = new Guid("c556ea1b-20b8-4e2d-bb3f-8c1a9a691c73");

            try
            {
                new DomainUpdateRequest(null, requestIdentifier);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "domain", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'domain' isn't checked for null inputs");
        }


        [TestMethod]
        public void DomainUpdateRequest_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DomainUpdateRequest_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DomainUpdateRequest_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DomainUpdateRequest_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class EmailSettingsTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<EmailSettings> objects = new ObjectsCache<EmailSettings>(GetInstances);

        internal static EmailSettings First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static EmailSettings Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static EmailSettings Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void EmailSettings_CheckNullArg_from()
        {
            var from = "text 135347";
            var to = "text 135348";
            var smtpHost = "text 135349";
            var smtpPort = 135350;
            var useSslConnection = true;
            var isAuthenticationEnabled = true;
            var userName = "text 135351";
            var password = "text 135352";

            try
            {
                new EmailSettings(null, to, smtpHost, smtpPort, useSslConnection, isAuthenticationEnabled, userName, password);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "from", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'from' isn't checked for null inputs");
        }

        [TestMethod]
        public void EmailSettings_CheckNullArg_to()
        {
            var from = "text 135347";
            var to = "text 135348";
            var smtpHost = "text 135349";
            var smtpPort = 135350;
            var useSslConnection = true;
            var isAuthenticationEnabled = true;
            var userName = "text 135351";
            var password = "text 135352";

            try
            {
                new EmailSettings(from, null, smtpHost, smtpPort, useSslConnection, isAuthenticationEnabled, userName, password);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "to", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'to' isn't checked for null inputs");
        }

        [TestMethod]
        public void EmailSettings_CheckNullArg_smtpHost()
        {
            var from = "text 135347";
            var to = "text 135348";
            var smtpHost = "text 135349";
            var smtpPort = 135350;
            var useSslConnection = true;
            var isAuthenticationEnabled = true;
            var userName = "text 135351";
            var password = "text 135352";

            try
            {
                new EmailSettings(from, to, null, smtpPort, useSslConnection, isAuthenticationEnabled, userName, password);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "smtpHost", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'smtpHost' isn't checked for null inputs");
        }

        [TestMethod]
        public void EmailSettings_CheckNullArg_userName()
        {
            var from = "text 135347";
            var to = "text 135348";
            var smtpHost = "text 135349";
            var smtpPort = 135350;
            var useSslConnection = true;
            var isAuthenticationEnabled = true;
            var userName = "text 135351";
            var password = "text 135352";

            try
            {
                new EmailSettings(from, to, smtpHost, smtpPort, useSslConnection, isAuthenticationEnabled, null, password);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "userName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'userName' isn't checked for null inputs");
        }

        [TestMethod]
        public void EmailSettings_CheckNullArg_password()
        {
            var from = "text 135347";
            var to = "text 135348";
            var smtpHost = "text 135349";
            var smtpPort = 135350;
            var useSslConnection = true;
            var isAuthenticationEnabled = true;
            var userName = "text 135351";
            var password = "text 135352";

            try
            {
                new EmailSettings(from, to, smtpHost, smtpPort, useSslConnection, isAuthenticationEnabled, userName, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "password", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'password' isn't checked for null inputs");
        }


        [TestMethod]
        public void EmailSettings_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void EmailSettings_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void EmailSettings_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void EmailSettings_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class DomainCredentialsTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DomainCredentials> objects = new ObjectsCache<DomainCredentials>(GetInstances);

        internal static DomainCredentials First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DomainCredentials Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DomainCredentials Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DomainCredentials_CheckEmptyStringArg_fullUserName()
        {
            CheckEmptyStringArg_fullUserName(string.Empty);
            CheckEmptyStringArg_fullUserName("    ");
            CheckEmptyStringArg_fullUserName(Environment.NewLine);
            CheckEmptyStringArg_fullUserName("\n\r");
        }

        private void CheckEmptyStringArg_fullUserName(string stringArgument)
        {
            var fullUserName = "text 135353";
            var password = "text 135354";
            var domain = "text 135355";

            try
            {
                new DomainCredentials(stringArgument, password, domain);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "fullUserName", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'fullUserName' isn't checked for emply values");
        }

        [TestMethod]
        public void DomainCredentials_CheckNullArg_fullUserName()
        {
            var fullUserName = "text 135353";
            var password = "text 135354";
            var domain = "text 135355";

            try
            {
                new DomainCredentials(null, password, domain);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "fullUserName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'fullUserName' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainCredentials_CheckEmptyStringArg_password()
        {
            CheckEmptyStringArg_password(string.Empty);
            CheckEmptyStringArg_password("    ");
            CheckEmptyStringArg_password(Environment.NewLine);
            CheckEmptyStringArg_password("\n\r");
        }

        private void CheckEmptyStringArg_password(string stringArgument)
        {
            var fullUserName = "text 135353";
            var password = "text 135354";
            var domain = "text 135355";

            try
            {
                new DomainCredentials(fullUserName, stringArgument, domain);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "password", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'password' isn't checked for emply values");
        }

        [TestMethod]
        public void DomainCredentials_CheckNullArg_password()
        {
            var fullUserName = "text 135353";
            var password = "text 135354";
            var domain = "text 135355";

            try
            {
                new DomainCredentials(fullUserName, null, domain);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "password", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'password' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainCredentials_CheckEmptyStringArg_domain()
        {
            CheckEmptyStringArg_domain(string.Empty);
            CheckEmptyStringArg_domain("    ");
            CheckEmptyStringArg_domain(Environment.NewLine);
            CheckEmptyStringArg_domain("\n\r");
        }

        private void CheckEmptyStringArg_domain(string stringArgument)
        {
            var fullUserName = "text 135353";
            var password = "text 135354";
            var domain = "text 135355";

            try
            {
                new DomainCredentials(fullUserName, password, stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "domain", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'domain' isn't checked for emply values");
        }

        [TestMethod]
        public void DomainCredentials_CheckNullArg_domain()
        {
            var fullUserName = "text 135353";
            var password = "text 135354";
            var domain = "text 135355";

            try
            {
                new DomainCredentials(fullUserName, password, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "domain", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'domain' isn't checked for null inputs");
        }


        [TestMethod]
        public void DomainCredentials_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DomainCredentials_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DomainCredentials_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DomainCredentials_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ManagedComputerDataTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<ManagedComputerData> objects = new ObjectsCache<ManagedComputerData>(GetInstances);

        internal static ManagedComputerData First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static ManagedComputerData Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static ManagedComputerData Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void ManagedComputerData_CheckWrongEnumArg_status()
        {
            var status = EnumHelper<ManagedComputerStatus>.Values.First();
            var lockMap = DeviceAccessMapTest.First;

            try
            {
                new ManagedComputerData((LeakBlocker.ServerShared.AdminViewCommunication.ManagedComputerStatus)5, lockMap);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "status", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'status' isn't checked for the not-defined values");
        }

        [TestMethod]
        public void ManagedComputerData_CheckNullArg_lockMap()
        {
            var status = EnumHelper<ManagedComputerStatus>.Values.First();
            var lockMap = DeviceAccessMapTest.First;

            try
            {
                new ManagedComputerData(status, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "lockMap", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'lockMap' isn't checked for null inputs");
        }


        [TestMethod]
        public void ManagedComputerData_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void ManagedComputerData_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void ManagedComputerData_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void ManagedComputerData_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ReportConfigurationTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<ReportConfiguration> objects = new ObjectsCache<ReportConfiguration>(GetInstances);

        internal static ReportConfiguration First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static ReportConfiguration Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static ReportConfiguration Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void ReportConfiguration_CheckNullArg_filter()
        {
            var areEnabled = true;
            var filter = ReportFilterTest.First;
            var email = EmailSettingsTest.First;

            try
            {
                new ReportConfiguration(areEnabled, null, email);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "filter", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'filter' isn't checked for null inputs");
        }

        [TestMethod]
        public void ReportConfiguration_CheckNullArg_email()
        {
            var areEnabled = true;
            var filter = ReportFilterTest.First;
            var email = EmailSettingsTest.First;

            try
            {
                new ReportConfiguration(areEnabled, filter, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "email", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'email' isn't checked for null inputs");
        }


        [TestMethod]
        public void ReportConfiguration_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void ReportConfiguration_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void ReportConfiguration_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void ReportConfiguration_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ResultComputerTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<ResultComputer> objects = new ObjectsCache<ResultComputer>(GetInstances);

        internal static ResultComputer First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static ResultComputer Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static ResultComputer Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void ResultComputer_CheckNullArg_parentScope()
        {
            var parentScope = ScopeTest.First;
            var targetAccount = BaseComputerAccountTest.First;

            try
            {
                new ResultComputer(null, targetAccount);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "parentScope", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'parentScope' isn't checked for null inputs");
        }

        [TestMethod]
        public void ResultComputer_CheckNullArg_targetAccount()
        {
            var parentScope = ScopeTest.First;
            var targetAccount = BaseComputerAccountTest.First;

            try
            {
                new ResultComputer(parentScope, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "targetAccount", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'targetAccount' isn't checked for null inputs");
        }


        [TestMethod]
        public void ResultComputer_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void ResultComputer_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void ResultComputer_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void ResultComputer_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ScopeTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<Scope> objects = new ObjectsCache<Scope>(GetInstances);

        internal static Scope First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static Scope Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static Scope Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void Scope_CheckNullArg_parentObject()
        {
            var parentObject = DomainAccountTest.First;

            try
            {
                new Scope(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "parentObject", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'parentObject' isn't checked for null inputs");
        }


        [TestMethod]
        public void Scope_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void Scope_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void Scope_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void Scope_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ManagedComputerTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<ManagedComputer> objects = new ObjectsCache<ManagedComputer>(GetInstances);

        internal static ManagedComputer First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static ManagedComputer Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static ManagedComputer Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void ManagedComputer_CheckNullArg_targetComputer()
        {
            var targetComputer = BaseComputerAccountTest.First;
            var data = ManagedComputerDataTest.First;

            try
            {
                new ManagedComputer(null, data);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "targetComputer", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'targetComputer' isn't checked for null inputs");
        }

        [TestMethod]
        public void ManagedComputer_CheckNullArg_data()
        {
            var targetComputer = BaseComputerAccountTest.First;
            var data = ManagedComputerDataTest.First;

            try
            {
                new ManagedComputer(targetComputer, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "data", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'data' isn't checked for null inputs");
        }


        [TestMethod]
        public void ManagedComputer_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void ManagedComputer_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void ManagedComputer_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void ManagedComputer_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class SimpleConfigurationTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<SimpleConfiguration> objects = new ObjectsCache<SimpleConfiguration>(GetInstances);

        internal static SimpleConfiguration First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static SimpleConfiguration Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static SimpleConfiguration Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void SimpleConfiguration_CheckNullArg_blockedScopes()
        {
            var isBlockEnabled = true;
            var isReadOnlyAccessEnabled = true;
            var areInputDevicesAllowed = true;
            var isFileAuditEnabled = true;
            var blockedScopes = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var excludedScopes = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var excludedDevices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var usersWhiteList = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var temporaryAccess = new List<BaseTemporaryAccessCondition>{ BaseTemporaryAccessConditionTest.First }.ToReadOnlyList();

            try
            {
                new SimpleConfiguration(isBlockEnabled, isReadOnlyAccessEnabled, areInputDevicesAllowed, isFileAuditEnabled, null, excludedScopes, excludedDevices, usersWhiteList, temporaryAccess);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "blockedScopes", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'blockedScopes' isn't checked for null inputs");
        }

        [TestMethod]
        public void SimpleConfiguration_CheckNullArg_excludedScopes()
        {
            var isBlockEnabled = true;
            var isReadOnlyAccessEnabled = true;
            var areInputDevicesAllowed = true;
            var isFileAuditEnabled = true;
            var blockedScopes = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var excludedScopes = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var excludedDevices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var usersWhiteList = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var temporaryAccess = new List<BaseTemporaryAccessCondition>{ BaseTemporaryAccessConditionTest.First }.ToReadOnlyList();

            try
            {
                new SimpleConfiguration(isBlockEnabled, isReadOnlyAccessEnabled, areInputDevicesAllowed, isFileAuditEnabled, blockedScopes, null, excludedDevices, usersWhiteList, temporaryAccess);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "excludedScopes", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'excludedScopes' isn't checked for null inputs");
        }

        [TestMethod]
        public void SimpleConfiguration_CheckNullArg_excludedDevices()
        {
            var isBlockEnabled = true;
            var isReadOnlyAccessEnabled = true;
            var areInputDevicesAllowed = true;
            var isFileAuditEnabled = true;
            var blockedScopes = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var excludedScopes = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var excludedDevices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var usersWhiteList = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var temporaryAccess = new List<BaseTemporaryAccessCondition>{ BaseTemporaryAccessConditionTest.First }.ToReadOnlyList();

            try
            {
                new SimpleConfiguration(isBlockEnabled, isReadOnlyAccessEnabled, areInputDevicesAllowed, isFileAuditEnabled, blockedScopes, excludedScopes, null, usersWhiteList, temporaryAccess);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "excludedDevices", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'excludedDevices' isn't checked for null inputs");
        }

        [TestMethod]
        public void SimpleConfiguration_CheckNullArg_usersWhiteList()
        {
            var isBlockEnabled = true;
            var isReadOnlyAccessEnabled = true;
            var areInputDevicesAllowed = true;
            var isFileAuditEnabled = true;
            var blockedScopes = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var excludedScopes = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var excludedDevices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var usersWhiteList = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var temporaryAccess = new List<BaseTemporaryAccessCondition>{ BaseTemporaryAccessConditionTest.First }.ToReadOnlyList();

            try
            {
                new SimpleConfiguration(isBlockEnabled, isReadOnlyAccessEnabled, areInputDevicesAllowed, isFileAuditEnabled, blockedScopes, excludedScopes, excludedDevices, null, temporaryAccess);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "usersWhiteList", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'usersWhiteList' isn't checked for null inputs");
        }

        [TestMethod]
        public void SimpleConfiguration_CheckNullArg_temporaryAccess()
        {
            var isBlockEnabled = true;
            var isReadOnlyAccessEnabled = true;
            var areInputDevicesAllowed = true;
            var isFileAuditEnabled = true;
            var blockedScopes = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var excludedScopes = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var excludedDevices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var usersWhiteList = new List<Scope>{ ScopeTest.First }.ToReadOnlyList();
            var temporaryAccess = new List<BaseTemporaryAccessCondition>{ BaseTemporaryAccessConditionTest.First }.ToReadOnlyList();

            try
            {
                new SimpleConfiguration(isBlockEnabled, isReadOnlyAccessEnabled, areInputDevicesAllowed, isFileAuditEnabled, blockedScopes, excludedScopes, excludedDevices, usersWhiteList, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "temporaryAccess", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'temporaryAccess' isn't checked for null inputs");
        }


        [TestMethod]
        public void SimpleConfiguration_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void SimpleConfiguration_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void SimpleConfiguration_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void SimpleConfiguration_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}


#pragma warning restore 219
#pragma warning restore 67
// ReSharper restore RedundantCast
// ReSharper restore UnusedVariable
// ReSharper restore ConditionIsAlwaysTrueOrFalse
// ReSharper restore ObjectCreationAsStatement
// ReSharper restore ConvertToConstant.Local
// ReSharper restore InconsistentNaming
// ReSharper restore UnusedMember.Global

