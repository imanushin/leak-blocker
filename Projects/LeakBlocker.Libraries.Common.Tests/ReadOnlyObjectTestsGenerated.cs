
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.License;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Cryptography;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Audit;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Tests.License;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

// ReSharper disable ConvertToConstant.Local
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable InconsistentNaming
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable UnusedVariable
// ReSharper disable RedundantCast
// ReSharper disable UnusedMember.Global

#pragma warning disable 67
#pragma warning disable 219


namespace LeakBlocker.Libraries.Common.Tests.Cryptography
{

    [TestClass]
    public sealed partial class EncryptionKeyTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<EncryptionKey> objects = new ObjectsCache<EncryptionKey>(GetInstances);

        internal static EncryptionKey First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static EncryptionKey Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static EncryptionKey Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<EncryptionKey>GetInstances()
        {
            return
            new EncryptionKey[0].Union(
AsymmetricPrivateEncryptionKeyTest.objects).Union(
AsymmetricPublicEncryptionKeyTest.objects).Union(
SymmetricEncryptionKeyTest.objects);
        }

        [TestMethod]
        public void EncryptionKey_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void EncryptionKey_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void EncryptionKey_SerializationTest()
        {
            BaseSerializationTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AsymmetricPrivateEncryptionKeyTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AsymmetricPrivateEncryptionKey> objects = new ObjectsCache<AsymmetricPrivateEncryptionKey>(GetInstances);

        internal static AsymmetricPrivateEncryptionKey First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AsymmetricPrivateEncryptionKey Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AsymmetricPrivateEncryptionKey Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AsymmetricPrivateEncryptionKey_CheckNullArg_key_135347()
        {
            var key = new List<Byte>{ 178 }.ToReadOnlyList();

            try
            {
                new AsymmetricPrivateEncryptionKey((ReadOnlyList<Byte>)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "key", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'key' isn't checked for null inputs");
        }

        [TestMethod]
        public void AsymmetricPrivateEncryptionKey_CheckEmptyStringArg_key_0()
        {
            CheckEmptyStringArg_key_0(string.Empty);
            CheckEmptyStringArg_key_0("    ");
            CheckEmptyStringArg_key_0(Environment.NewLine);
            CheckEmptyStringArg_key_0("\n\r");
        }

        private void CheckEmptyStringArg_key_0(string stringArgument)
        {
            var key = "text 135348";

            try
            {
                new AsymmetricPrivateEncryptionKey(stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "key", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'key' isn't checked for emply values");
        }

        [TestMethod]
        public void AsymmetricPrivateEncryptionKey_CheckNullArg_key_135349()
        {
            var key = "text 135348";

            try
            {
                new AsymmetricPrivateEncryptionKey((String)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "key", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'key' isn't checked for null inputs");
        }


        [TestMethod]
        public void AsymmetricPrivateEncryptionKey_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AsymmetricPrivateEncryptionKey_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AsymmetricPrivateEncryptionKey_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AsymmetricPrivateEncryptionKey_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AsymmetricPublicEncryptionKeyTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AsymmetricPublicEncryptionKey> objects = new ObjectsCache<AsymmetricPublicEncryptionKey>(GetInstances);

        internal static AsymmetricPublicEncryptionKey First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AsymmetricPublicEncryptionKey Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AsymmetricPublicEncryptionKey Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AsymmetricPublicEncryptionKey_CheckNullArg_key_135351()
        {
            var key = new List<Byte>{ 182 }.ToReadOnlyList();

            try
            {
                new AsymmetricPublicEncryptionKey((ReadOnlyList<Byte>)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "key", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'key' isn't checked for null inputs");
        }

        [TestMethod]
        public void AsymmetricPublicEncryptionKey_CheckEmptyStringArg_key_1()
        {
            CheckEmptyStringArg_key_1(string.Empty);
            CheckEmptyStringArg_key_1("    ");
            CheckEmptyStringArg_key_1(Environment.NewLine);
            CheckEmptyStringArg_key_1("\n\r");
        }

        private void CheckEmptyStringArg_key_1(string stringArgument)
        {
            var key = "text 135352";

            try
            {
                new AsymmetricPublicEncryptionKey(stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "key", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'key' isn't checked for emply values");
        }

        [TestMethod]
        public void AsymmetricPublicEncryptionKey_CheckNullArg_key_135353()
        {
            var key = "text 135352";

            try
            {
                new AsymmetricPublicEncryptionKey((String)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "key", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'key' isn't checked for null inputs");
        }


        [TestMethod]
        public void AsymmetricPublicEncryptionKey_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AsymmetricPublicEncryptionKey_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AsymmetricPublicEncryptionKey_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AsymmetricPublicEncryptionKey_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class SymmetricEncryptionKeyTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<SymmetricEncryptionKey> objects = new ObjectsCache<SymmetricEncryptionKey>(GetInstances);

        internal static SymmetricEncryptionKey First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static SymmetricEncryptionKey Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static SymmetricEncryptionKey Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void SymmetricEncryptionKey_CheckNullArg_key_135355()
        {
            var key = new List<Byte>{ 186 }.ToReadOnlyList();

            try
            {
                new SymmetricEncryptionKey((ReadOnlyList<Byte>)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "key", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'key' isn't checked for null inputs");
        }

        [TestMethod]
        public void SymmetricEncryptionKey_CheckEmptyStringArg_key_2()
        {
            CheckEmptyStringArg_key_2(string.Empty);
            CheckEmptyStringArg_key_2("    ");
            CheckEmptyStringArg_key_2(Environment.NewLine);
            CheckEmptyStringArg_key_2("\n\r");
        }

        private void CheckEmptyStringArg_key_2(string stringArgument)
        {
            var key = "text 135356";

            try
            {
                new SymmetricEncryptionKey(stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "key", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'key' isn't checked for emply values");
        }

        [TestMethod]
        public void SymmetricEncryptionKey_CheckNullArg_key_135357()
        {
            var key = "text 135356";

            try
            {
                new SymmetricEncryptionKey((String)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "key", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'key' isn't checked for null inputs");
        }


        [TestMethod]
        public void SymmetricEncryptionKey_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void SymmetricEncryptionKey_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void SymmetricEncryptionKey_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void SymmetricEncryptionKey_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.Libraries.Common.Tests.Entities
{

    [TestClass]
    public sealed partial class DeviceAccessMapTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DeviceAccessMap> objects = new ObjectsCache<DeviceAccessMap>(GetInstances);

        internal static DeviceAccessMap First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DeviceAccessMap Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DeviceAccessMap Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DeviceAccessMap_CheckNullArg_keys1()
        {
            var keys1 = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var keys2 = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var constructor = new Func<BaseUserAccount, DeviceDescription, DeviceAccessType>((arg1, arg2)=>default(DeviceAccessType));

            try
            {
                new DeviceAccessMap(null, keys2, constructor);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "keys1", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'keys1' isn't checked for null inputs");
        }

        [TestMethod]
        public void DeviceAccessMap_CheckNullArg_keys2()
        {
            var keys1 = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var keys2 = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var constructor = new Func<BaseUserAccount, DeviceDescription, DeviceAccessType>((arg1, arg2)=>default(DeviceAccessType));

            try
            {
                new DeviceAccessMap(keys1, null, constructor);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "keys2", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'keys2' isn't checked for null inputs");
        }

        [TestMethod]
        public void DeviceAccessMap_CheckNullArg_constructor()
        {
            var keys1 = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var keys2 = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var constructor = new Func<BaseUserAccount, DeviceDescription, DeviceAccessType>((arg1, arg2)=>default(DeviceAccessType));

            try
            {
                new DeviceAccessMap(keys1, keys2, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "constructor", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'constructor' isn't checked for null inputs");
        }


        [TestMethod]
        public void DeviceAccessMap_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DeviceAccessMap_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DeviceAccessMap_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DeviceAccessMap_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class BaseEntityTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<BaseEntity> objects = new ObjectsCache<BaseEntity>(GetInstances);

        internal static BaseEntity First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static BaseEntity Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static BaseEntity Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<BaseEntity>GetInstances()
        {
            return
            new BaseEntity[0].Union(
BaseRuleConditionTest.objects).Union(
DeviceTypeRuleConditionTest.objects).Union(
AuditItemTest.objects).Union(
DeviceDescriptionTest.objects).Union(
AccountTest.objects).Union(
AccountSecurityIdentifierTest.objects).Union(
AgentEncryptionDataTest.objects).Union(
BaseDomainAccountTest.objects).Union(
BaseComputerAccountTest.objects).Union(
BaseGroupAccountTest.objects).Union(
BaseUserAccountTest.objects).Union(
CredentialsTest.objects).Union(
DomainAccountTest.objects).Union(
DomainComputerAccountTest.objects).Union(
DomainComputerGroupAccountTest.objects).Union(
DomainComputerUserAccountTest.objects).Union(
DomainGroupAccountTest.objects).Union(
DomainUserAccountTest.objects).Union(
LocalComputerAccountTest.objects).Union(
LocalGroupAccountTest.objects).Union(
LocalUserAccountTest.objects).Union(
OrganizationalUnitTest.objects).Union(
ProgramConfigurationTest.objects).Union(
ActionDataTest.objects).Union(
CompositeRuleConditionTest.objects).Union(
ComputerListRuleConditionTest.objects).Union(
DeviceListRuleConditionTest.objects).Union(
BaseTemporaryAccessConditionTest.objects).Union(
ComputerTemporaryAccessConditionTest.objects).Union(
DeviceTemporaryAccessConditionTest.objects).Union(
UserTemporaryAccessConditionTest.objects).Union(
UserListRuleConditionTest.objects).Union(
RuleTest.objects);
        }

        [TestMethod]
        public void BaseEntity_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void BaseEntity_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void BaseEntity_SerializationTest()
        {
            BaseSerializationTest(objects);
        }
    }

    [TestClass]
    public sealed partial class UserContactInformationTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<UserContactInformation> objects = new ObjectsCache<UserContactInformation>(GetInstances);

        internal static UserContactInformation First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static UserContactInformation Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static UserContactInformation Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void UserContactInformation_CheckNullArg_firstName()
        {
            var firstName = "text 135358";
            var lastName = "text 135359";
            var companyName = "text 135360";
            var email = "text 135361";
            var phone = "text 135362";

            try
            {
                new UserContactInformation(null, lastName, companyName, email, phone);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "firstName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'firstName' isn't checked for null inputs");
        }

        [TestMethod]
        public void UserContactInformation_CheckNullArg_lastName()
        {
            var firstName = "text 135358";
            var lastName = "text 135359";
            var companyName = "text 135360";
            var email = "text 135361";
            var phone = "text 135362";

            try
            {
                new UserContactInformation(firstName, null, companyName, email, phone);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "lastName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'lastName' isn't checked for null inputs");
        }

        [TestMethod]
        public void UserContactInformation_CheckNullArg_companyName()
        {
            var firstName = "text 135358";
            var lastName = "text 135359";
            var companyName = "text 135360";
            var email = "text 135361";
            var phone = "text 135362";

            try
            {
                new UserContactInformation(firstName, lastName, null, email, phone);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "companyName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'companyName' isn't checked for null inputs");
        }

        [TestMethod]
        public void UserContactInformation_CheckNullArg_email()
        {
            var firstName = "text 135358";
            var lastName = "text 135359";
            var companyName = "text 135360";
            var email = "text 135361";
            var phone = "text 135362";

            try
            {
                new UserContactInformation(firstName, lastName, companyName, null, phone);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "email", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'email' isn't checked for null inputs");
        }

        [TestMethod]
        public void UserContactInformation_CheckNullArg_phone()
        {
            var firstName = "text 135358";
            var lastName = "text 135359";
            var companyName = "text 135360";
            var email = "text 135361";
            var phone = "text 135362";

            try
            {
                new UserContactInformation(firstName, lastName, companyName, email, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "phone", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'phone' isn't checked for null inputs");
        }


        [TestMethod]
        public void UserContactInformation_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void UserContactInformation_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void UserContactInformation_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void UserContactInformation_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class DeviceDescriptionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DeviceDescription> objects = new ObjectsCache<DeviceDescription>(GetInstances);

        internal static DeviceDescription First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DeviceDescription Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DeviceDescription Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DeviceDescription_CheckEmptyStringArg_friendlyName()
        {
            CheckEmptyStringArg_friendlyName(string.Empty);
            CheckEmptyStringArg_friendlyName("    ");
            CheckEmptyStringArg_friendlyName(Environment.NewLine);
            CheckEmptyStringArg_friendlyName("\n\r");
        }

        private void CheckEmptyStringArg_friendlyName(string stringArgument)
        {
            var friendlyName = "text 135363";
            var identifier = "text 135364";
            var category = EnumHelper<DeviceCategory>.Values.First();

            try
            {
                new DeviceDescription(stringArgument, identifier, category);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "friendlyName", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'friendlyName' isn't checked for emply values");
        }

        [TestMethod]
        public void DeviceDescription_CheckNullArg_friendlyName()
        {
            var friendlyName = "text 135363";
            var identifier = "text 135364";
            var category = EnumHelper<DeviceCategory>.Values.First();

            try
            {
                new DeviceDescription(null, identifier, category);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "friendlyName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'friendlyName' isn't checked for null inputs");
        }

        [TestMethod]
        public void DeviceDescription_CheckEmptyStringArg_identifier()
        {
            CheckEmptyStringArg_identifier(string.Empty);
            CheckEmptyStringArg_identifier("    ");
            CheckEmptyStringArg_identifier(Environment.NewLine);
            CheckEmptyStringArg_identifier("\n\r");
        }

        private void CheckEmptyStringArg_identifier(string stringArgument)
        {
            var friendlyName = "text 135363";
            var identifier = "text 135364";
            var category = EnumHelper<DeviceCategory>.Values.First();

            try
            {
                new DeviceDescription(friendlyName, stringArgument, category);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'identifier' isn't checked for emply values");
        }

        [TestMethod]
        public void DeviceDescription_CheckNullArg_identifier()
        {
            var friendlyName = "text 135363";
            var identifier = "text 135364";
            var category = EnumHelper<DeviceCategory>.Values.First();

            try
            {
                new DeviceDescription(friendlyName, null, category);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'identifier' isn't checked for null inputs");
        }

        [TestMethod]
        public void DeviceDescription_CheckWrongEnumArg_category()
        {
            var friendlyName = "text 135363";
            var identifier = "text 135364";
            var category = EnumHelper<DeviceCategory>.Values.First();

            try
            {
                new DeviceDescription(friendlyName, identifier, (LeakBlocker.Libraries.Common.Entities.DeviceCategory)13);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "category", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'category' isn't checked for the not-defined values");
        }


        [TestMethod]
        public void DeviceDescription_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DeviceDescription_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DeviceDescription_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DeviceDescription_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings
{

    [TestClass]
    public sealed partial class AuditMapTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AuditMap> objects = new ObjectsCache<AuditMap>(GetInstances);

        internal static AuditMap First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AuditMap Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AuditMap Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AuditMap_CheckNullArg_users()
        {
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var constructor = new Func<BaseUserAccount, DeviceDescription, AuditActionType>((arg1, arg2)=>default(AuditActionType));

            try
            {
                new AuditMap(null, devices, constructor);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "users", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'users' isn't checked for null inputs");
        }

        [TestMethod]
        public void AuditMap_CheckNullArg_devices()
        {
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var constructor = new Func<BaseUserAccount, DeviceDescription, AuditActionType>((arg1, arg2)=>default(AuditActionType));

            try
            {
                new AuditMap(users, null, constructor);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "devices", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'devices' isn't checked for null inputs");
        }

        [TestMethod]
        public void AuditMap_CheckNullArg_constructor()
        {
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var constructor = new Func<BaseUserAccount, DeviceDescription, AuditActionType>((arg1, arg2)=>default(AuditActionType));

            try
            {
                new AuditMap(users, devices, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "constructor", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'constructor' isn't checked for null inputs");
        }


        [TestMethod]
        public void AuditMap_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AuditMap_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AuditMap_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AuditMap_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ProgramConfigurationTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<ProgramConfiguration> objects = new ObjectsCache<ProgramConfiguration>(GetInstances);

        internal static ProgramConfiguration First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static ProgramConfiguration Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static ProgramConfiguration Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void ProgramConfiguration_CheckNullArg_rules()
        {
            var configurationVersion = 135365;
            var rules = new List<Rule>{ RuleTest.First }.ToReadOnlyList();
            var temporaryAccess = new List<BaseTemporaryAccessCondition>{ BaseTemporaryAccessConditionTest.First }.ToReadOnlyList();

            try
            {
                new ProgramConfiguration(configurationVersion, null, temporaryAccess);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rules", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rules' isn't checked for null inputs");
        }

        [TestMethod]
        public void ProgramConfiguration_CheckNullArg_temporaryAccess()
        {
            var configurationVersion = 135365;
            var rules = new List<Rule>{ RuleTest.First }.ToReadOnlyList();
            var temporaryAccess = new List<BaseTemporaryAccessCondition>{ BaseTemporaryAccessConditionTest.First }.ToReadOnlyList();

            try
            {
                new ProgramConfiguration(configurationVersion, rules, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "temporaryAccess", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'temporaryAccess' isn't checked for null inputs");
        }


        [TestMethod]
        public void ProgramConfiguration_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void ProgramConfiguration_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void ProgramConfiguration_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void ProgramConfiguration_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules.Conditions
{

    [TestClass]
    public sealed partial class BaseRuleConditionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<BaseRuleCondition> objects = new ObjectsCache<BaseRuleCondition>(GetInstances);

        internal static BaseRuleCondition First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static BaseRuleCondition Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static BaseRuleCondition Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<BaseRuleCondition>GetInstances()
        {
            return
            new BaseRuleCondition[0].Union(
DeviceTypeRuleConditionTest.objects).Union(
CompositeRuleConditionTest.objects).Union(
ComputerListRuleConditionTest.objects).Union(
DeviceListRuleConditionTest.objects).Union(
UserListRuleConditionTest.objects);
        }

        [TestMethod]
        public void BaseRuleCondition_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void BaseRuleCondition_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void BaseRuleCondition_SerializationTest()
        {
            BaseSerializationTest(objects);
        }
    }

    [TestClass]
    public sealed partial class DeviceTypeRuleConditionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DeviceTypeRuleCondition> objects = new ObjectsCache<DeviceTypeRuleCondition>(GetInstances);

        internal static DeviceTypeRuleCondition First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DeviceTypeRuleCondition Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DeviceTypeRuleCondition Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DeviceTypeRuleCondition_CheckWrongEnumArg_deviceType()
        {
            var deviceType = EnumHelper<DeviceCategory>.Values.First();
            var not = true;

            try
            {
                new DeviceTypeRuleCondition((LeakBlocker.Libraries.Common.Entities.DeviceCategory)13, not);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "deviceType", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'deviceType' isn't checked for the not-defined values");
        }


        [TestMethod]
        public void DeviceTypeRuleCondition_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DeviceTypeRuleCondition_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DeviceTypeRuleCondition_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DeviceTypeRuleCondition_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class CompositeRuleConditionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<CompositeRuleCondition> objects = new ObjectsCache<CompositeRuleCondition>(GetInstances);

        internal static CompositeRuleCondition First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static CompositeRuleCondition Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static CompositeRuleCondition Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void CompositeRuleCondition_CheckNullArg_conditions()
        {
            var not = true;
            var conditions = new List<BaseRuleCondition>{ BaseRuleConditionTest.First }.ToReadOnlyList();
            var operationType = EnumHelper<CompositeRuleConditionType>.Values.First();

            try
            {
                new CompositeRuleCondition(not, null, operationType);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "conditions", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'conditions' isn't checked for null inputs");
        }

        [TestMethod]
        public void CompositeRuleCondition_CheckWrongEnumArg_operationType()
        {
            var not = true;
            var conditions = new List<BaseRuleCondition>{ BaseRuleConditionTest.First }.ToReadOnlyList();
            var operationType = EnumHelper<CompositeRuleConditionType>.Values.First();

            try
            {
                new CompositeRuleCondition(not, conditions, (LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions.CompositeRuleConditionType)3);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "operationType", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'operationType' isn't checked for the not-defined values");
        }


        [TestMethod]
        public void CompositeRuleCondition_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void CompositeRuleCondition_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void CompositeRuleCondition_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void CompositeRuleCondition_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ComputerListRuleConditionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<ComputerListRuleCondition> objects = new ObjectsCache<ComputerListRuleCondition>(GetInstances);

        internal static ComputerListRuleCondition First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static ComputerListRuleCondition Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static ComputerListRuleCondition Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void ComputerListRuleCondition_CheckNullArg_domains()
        {
            var not = true;
            var domains = new List<DomainAccount>{ DomainAccountTest.First }.ToReadOnlyList();
            var organizationalUnits = new List<OrganizationalUnit>{ OrganizationalUnitTest.First }.ToReadOnlyList();
            var groups = new List<DomainGroupAccount>{ DomainGroupAccountTest.First }.ToReadOnlyList();
            var computers = new List<BaseComputerAccount>{ BaseComputerAccountTest.First }.ToReadOnlyList();

            try
            {
                new ComputerListRuleCondition(not, null, organizationalUnits, groups, computers);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "domains", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'domains' isn't checked for null inputs");
        }

        [TestMethod]
        public void ComputerListRuleCondition_CheckNullArg_organizationalUnits()
        {
            var not = true;
            var domains = new List<DomainAccount>{ DomainAccountTest.First }.ToReadOnlyList();
            var organizationalUnits = new List<OrganizationalUnit>{ OrganizationalUnitTest.First }.ToReadOnlyList();
            var groups = new List<DomainGroupAccount>{ DomainGroupAccountTest.First }.ToReadOnlyList();
            var computers = new List<BaseComputerAccount>{ BaseComputerAccountTest.First }.ToReadOnlyList();

            try
            {
                new ComputerListRuleCondition(not, domains, null, groups, computers);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "organizationalUnits", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'organizationalUnits' isn't checked for null inputs");
        }

        [TestMethod]
        public void ComputerListRuleCondition_CheckNullArg_groups()
        {
            var not = true;
            var domains = new List<DomainAccount>{ DomainAccountTest.First }.ToReadOnlyList();
            var organizationalUnits = new List<OrganizationalUnit>{ OrganizationalUnitTest.First }.ToReadOnlyList();
            var groups = new List<DomainGroupAccount>{ DomainGroupAccountTest.First }.ToReadOnlyList();
            var computers = new List<BaseComputerAccount>{ BaseComputerAccountTest.First }.ToReadOnlyList();

            try
            {
                new ComputerListRuleCondition(not, domains, organizationalUnits, null, computers);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "groups", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'groups' isn't checked for null inputs");
        }

        [TestMethod]
        public void ComputerListRuleCondition_CheckNullArg_computers()
        {
            var not = true;
            var domains = new List<DomainAccount>{ DomainAccountTest.First }.ToReadOnlyList();
            var organizationalUnits = new List<OrganizationalUnit>{ OrganizationalUnitTest.First }.ToReadOnlyList();
            var groups = new List<DomainGroupAccount>{ DomainGroupAccountTest.First }.ToReadOnlyList();
            var computers = new List<BaseComputerAccount>{ BaseComputerAccountTest.First }.ToReadOnlyList();

            try
            {
                new ComputerListRuleCondition(not, domains, organizationalUnits, groups, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "computers", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'computers' isn't checked for null inputs");
        }


        [TestMethod]
        public void ComputerListRuleCondition_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void ComputerListRuleCondition_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void ComputerListRuleCondition_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void ComputerListRuleCondition_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class DeviceListRuleConditionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DeviceListRuleCondition> objects = new ObjectsCache<DeviceListRuleCondition>(GetInstances);

        internal static DeviceListRuleCondition First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DeviceListRuleCondition Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DeviceListRuleCondition Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DeviceListRuleCondition_CheckNullArg_devices()
        {
            var not = true;
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();

            try
            {
                new DeviceListRuleCondition(not, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "devices", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'devices' isn't checked for null inputs");
        }


        [TestMethod]
        public void DeviceListRuleCondition_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DeviceListRuleCondition_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DeviceListRuleCondition_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DeviceListRuleCondition_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class UserListRuleConditionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<UserListRuleCondition> objects = new ObjectsCache<UserListRuleCondition>(GetInstances);

        internal static UserListRuleCondition First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static UserListRuleCondition Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static UserListRuleCondition Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void UserListRuleCondition_CheckNullArg_domains()
        {
            var not = true;
            var domains = new List<DomainAccount>{ DomainAccountTest.First }.ToReadOnlyList();
            var organizationalUnits = new List<OrganizationalUnit>{ OrganizationalUnitTest.First }.ToReadOnlyList();
            var groups = new List<BaseGroupAccount>{ BaseGroupAccountTest.First }.ToReadOnlyList();
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();

            try
            {
                new UserListRuleCondition(not, null, organizationalUnits, groups, users);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "domains", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'domains' isn't checked for null inputs");
        }

        [TestMethod]
        public void UserListRuleCondition_CheckNullArg_organizationalUnits()
        {
            var not = true;
            var domains = new List<DomainAccount>{ DomainAccountTest.First }.ToReadOnlyList();
            var organizationalUnits = new List<OrganizationalUnit>{ OrganizationalUnitTest.First }.ToReadOnlyList();
            var groups = new List<BaseGroupAccount>{ BaseGroupAccountTest.First }.ToReadOnlyList();
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();

            try
            {
                new UserListRuleCondition(not, domains, null, groups, users);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "organizationalUnits", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'organizationalUnits' isn't checked for null inputs");
        }

        [TestMethod]
        public void UserListRuleCondition_CheckNullArg_groups()
        {
            var not = true;
            var domains = new List<DomainAccount>{ DomainAccountTest.First }.ToReadOnlyList();
            var organizationalUnits = new List<OrganizationalUnit>{ OrganizationalUnitTest.First }.ToReadOnlyList();
            var groups = new List<BaseGroupAccount>{ BaseGroupAccountTest.First }.ToReadOnlyList();
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();

            try
            {
                new UserListRuleCondition(not, domains, organizationalUnits, null, users);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "groups", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'groups' isn't checked for null inputs");
        }

        [TestMethod]
        public void UserListRuleCondition_CheckNullArg_users()
        {
            var not = true;
            var domains = new List<DomainAccount>{ DomainAccountTest.First }.ToReadOnlyList();
            var organizationalUnits = new List<OrganizationalUnit>{ OrganizationalUnitTest.First }.ToReadOnlyList();
            var groups = new List<BaseGroupAccount>{ BaseGroupAccountTest.First }.ToReadOnlyList();
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();

            try
            {
                new UserListRuleCondition(not, domains, organizationalUnits, groups, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "users", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'users' isn't checked for null inputs");
        }


        [TestMethod]
        public void UserListRuleCondition_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void UserListRuleCondition_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void UserListRuleCondition_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void UserListRuleCondition_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.Libraries.Common.Tests.Entities.Audit
{

    [TestClass]
    public sealed partial class AuditFilterTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AuditFilter> objects = new ObjectsCache<AuditFilter>(GetInstances);

        internal static AuditFilter First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AuditFilter Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AuditFilter Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AuditFilter_CheckEmptyStringArg_name()
        {
            CheckEmptyStringArg_name(string.Empty);
            CheckEmptyStringArg_name("    ");
            CheckEmptyStringArg_name(Environment.NewLine);
            CheckEmptyStringArg_name("\n\r");
        }

        private void CheckEmptyStringArg_name(string stringArgument)
        {
            var name = "text 135366";
            var computers = new List<BaseComputerAccount>{ BaseComputerAccountTest.First }.ToReadOnlyList();
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var startTime = TimeTest.First;
            var endTime = TimeTest.First;
            var onlyError = true;
            var types = new List<AuditItemGroupType>{ EnumHelper<AuditItemGroupType>.Values.First() }.ToReadOnlyList();

            try
            {
                new AuditFilter(stringArgument, computers, users, devices, startTime, endTime, onlyError, types);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'name' isn't checked for emply values");
        }

        [TestMethod]
        public void AuditFilter_CheckNullArg_name()
        {
            var name = "text 135366";
            var computers = new List<BaseComputerAccount>{ BaseComputerAccountTest.First }.ToReadOnlyList();
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var startTime = TimeTest.First;
            var endTime = TimeTest.First;
            var onlyError = true;
            var types = new List<AuditItemGroupType>{ EnumHelper<AuditItemGroupType>.Values.First() }.ToReadOnlyList();

            try
            {
                new AuditFilter(null, computers, users, devices, startTime, endTime, onlyError, types);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'name' isn't checked for null inputs");
        }

        [TestMethod]
        public void AuditFilter_CheckNullArg_computers()
        {
            var name = "text 135366";
            var computers = new List<BaseComputerAccount>{ BaseComputerAccountTest.First }.ToReadOnlyList();
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var startTime = TimeTest.First;
            var endTime = TimeTest.First;
            var onlyError = true;
            var types = new List<AuditItemGroupType>{ EnumHelper<AuditItemGroupType>.Values.First() }.ToReadOnlyList();

            try
            {
                new AuditFilter(name, null, users, devices, startTime, endTime, onlyError, types);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "computers", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'computers' isn't checked for null inputs");
        }

        [TestMethod]
        public void AuditFilter_CheckNullArg_users()
        {
            var name = "text 135366";
            var computers = new List<BaseComputerAccount>{ BaseComputerAccountTest.First }.ToReadOnlyList();
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var startTime = TimeTest.First;
            var endTime = TimeTest.First;
            var onlyError = true;
            var types = new List<AuditItemGroupType>{ EnumHelper<AuditItemGroupType>.Values.First() }.ToReadOnlyList();

            try
            {
                new AuditFilter(name, computers, null, devices, startTime, endTime, onlyError, types);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "users", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'users' isn't checked for null inputs");
        }

        [TestMethod]
        public void AuditFilter_CheckNullArg_devices()
        {
            var name = "text 135366";
            var computers = new List<BaseComputerAccount>{ BaseComputerAccountTest.First }.ToReadOnlyList();
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var startTime = TimeTest.First;
            var endTime = TimeTest.First;
            var onlyError = true;
            var types = new List<AuditItemGroupType>{ EnumHelper<AuditItemGroupType>.Values.First() }.ToReadOnlyList();

            try
            {
                new AuditFilter(name, computers, users, null, startTime, endTime, onlyError, types);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "devices", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'devices' isn't checked for null inputs");
        }

        [TestMethod]
        public void AuditFilter_CheckNullArg_startTime()
        {
            var name = "text 135366";
            var computers = new List<BaseComputerAccount>{ BaseComputerAccountTest.First }.ToReadOnlyList();
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var startTime = TimeTest.First;
            var endTime = TimeTest.First;
            var onlyError = true;
            var types = new List<AuditItemGroupType>{ EnumHelper<AuditItemGroupType>.Values.First() }.ToReadOnlyList();

            try
            {
                new AuditFilter(name, computers, users, devices, null, endTime, onlyError, types);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "startTime", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'startTime' isn't checked for null inputs");
        }

        [TestMethod]
        public void AuditFilter_CheckNullArg_endTime()
        {
            var name = "text 135366";
            var computers = new List<BaseComputerAccount>{ BaseComputerAccountTest.First }.ToReadOnlyList();
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var startTime = TimeTest.First;
            var endTime = TimeTest.First;
            var onlyError = true;
            var types = new List<AuditItemGroupType>{ EnumHelper<AuditItemGroupType>.Values.First() }.ToReadOnlyList();

            try
            {
                new AuditFilter(name, computers, users, devices, startTime, null, onlyError, types);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "endTime", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'endTime' isn't checked for null inputs");
        }

        [TestMethod]
        public void AuditFilter_CheckNullArg_types()
        {
            var name = "text 135366";
            var computers = new List<BaseComputerAccount>{ BaseComputerAccountTest.First }.ToReadOnlyList();
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var startTime = TimeTest.First;
            var endTime = TimeTest.First;
            var onlyError = true;
            var types = new List<AuditItemGroupType>{ EnumHelper<AuditItemGroupType>.Values.First() }.ToReadOnlyList();

            try
            {
                new AuditFilter(name, computers, users, devices, startTime, endTime, onlyError, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "types", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'types' isn't checked for null inputs");
        }


        [TestMethod]
        public void AuditFilter_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AuditFilter_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AuditFilter_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AuditFilter_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AuditItemTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AuditItem> objects = new ObjectsCache<AuditItem>(GetInstances);

        internal static AuditItem First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AuditItem Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AuditItem Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AuditItem_CheckWrongEnumArg_type()
        {
            var type = EnumHelper<AuditItemType>.Values.First();
            var time = TimeTest.First;
            var computer = BaseComputerAccountTest.First;
            var user = BaseUserAccountTest.First;
            var textData = "text 135367";
            var additionalTextData = "text 135368";
            var device = DeviceDescriptionTest.First;
            var configuration = 135369;
            var number = 135370;

            try
            {
                new AuditItem((LeakBlocker.Libraries.Common.Entities.Audit.AuditItemType)100036, time, computer, user, textData, additionalTextData, device, configuration, number);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "type", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'type' isn't checked for the not-defined values");
        }

        [TestMethod]
        public void AuditItem_CheckNullArg_time()
        {
            var type = EnumHelper<AuditItemType>.Values.First();
            var time = TimeTest.First;
            var computer = BaseComputerAccountTest.First;
            var user = BaseUserAccountTest.First;
            var textData = "text 135367";
            var additionalTextData = "text 135368";
            var device = DeviceDescriptionTest.First;
            var configuration = 135369;
            var number = 135370;

            try
            {
                new AuditItem(type, null, computer, user, textData, additionalTextData, device, configuration, number);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "time", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'time' isn't checked for null inputs");
        }

        [TestMethod]
        public void AuditItem_CheckNullArg_computer()
        {
            var type = EnumHelper<AuditItemType>.Values.First();
            var time = TimeTest.First;
            var computer = BaseComputerAccountTest.First;
            var user = BaseUserAccountTest.First;
            var textData = "text 135367";
            var additionalTextData = "text 135368";
            var device = DeviceDescriptionTest.First;
            var configuration = 135369;
            var number = 135370;

            try
            {
                new AuditItem(type, time, null, user, textData, additionalTextData, device, configuration, number);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "computer", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'computer' isn't checked for null inputs");
        }


        [TestMethod]
        public void AuditItem_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AuditItem_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AuditItem_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AuditItem_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ReportFilterTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<ReportFilter> objects = new ObjectsCache<ReportFilter>(GetInstances);

        internal static ReportFilter First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static ReportFilter Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static ReportFilter Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void ReportFilter_CheckWrongEnumArg_block()
        {
            var errors = true;
            var block = EnumHelper<OperationDetail>.Values.First();
            var allow = EnumHelper<OperationDetail>.Values.First();
            var temporaryAccess = EnumHelper<OperationDetail>.Values.First();
            var configurationChanges = true;
            var warnings = true;

            try
            {
                new ReportFilter(errors, (LeakBlocker.Libraries.Common.Entities.Audit.OperationDetail)3, allow, temporaryAccess, configurationChanges, warnings);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "block", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'block' isn't checked for the not-defined values");
        }

        [TestMethod]
        public void ReportFilter_CheckWrongEnumArg_allow()
        {
            var errors = true;
            var block = EnumHelper<OperationDetail>.Values.First();
            var allow = EnumHelper<OperationDetail>.Values.First();
            var temporaryAccess = EnumHelper<OperationDetail>.Values.First();
            var configurationChanges = true;
            var warnings = true;

            try
            {
                new ReportFilter(errors, block, (LeakBlocker.Libraries.Common.Entities.Audit.OperationDetail)3, temporaryAccess, configurationChanges, warnings);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "allow", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'allow' isn't checked for the not-defined values");
        }

        [TestMethod]
        public void ReportFilter_CheckWrongEnumArg_temporaryAccess()
        {
            var errors = true;
            var block = EnumHelper<OperationDetail>.Values.First();
            var allow = EnumHelper<OperationDetail>.Values.First();
            var temporaryAccess = EnumHelper<OperationDetail>.Values.First();
            var configurationChanges = true;
            var warnings = true;

            try
            {
                new ReportFilter(errors, block, allow, (LeakBlocker.Libraries.Common.Entities.Audit.OperationDetail)3, configurationChanges, warnings);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "temporaryAccess", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'temporaryAccess' isn't checked for the not-defined values");
        }


        [TestMethod]
        public void ReportFilter_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void ReportFilter_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void ReportFilter_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void ReportFilter_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.Libraries.Common.Tests.Entities.Security
{

    [TestClass]
    public sealed partial class AccountTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<Account> objects = new ObjectsCache<Account>(GetInstances);

        internal static Account First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static Account Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static Account Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<Account>GetInstances()
        {
            return
            new Account[0].Union(
BaseDomainAccountTest.objects).Union(
BaseComputerAccountTest.objects).Union(
BaseGroupAccountTest.objects).Union(
BaseUserAccountTest.objects).Union(
DomainAccountTest.objects).Union(
DomainComputerAccountTest.objects).Union(
DomainComputerGroupAccountTest.objects).Union(
DomainComputerUserAccountTest.objects).Union(
DomainGroupAccountTest.objects).Union(
DomainUserAccountTest.objects).Union(
LocalComputerAccountTest.objects).Union(
LocalGroupAccountTest.objects).Union(
LocalUserAccountTest.objects);
        }

        [TestMethod]
        public void Account_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void Account_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void Account_SerializationTest()
        {
            BaseSerializationTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AccountSecurityIdentifierTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AccountSecurityIdentifier> objects = new ObjectsCache<AccountSecurityIdentifier>(GetInstances);

        internal static AccountSecurityIdentifier First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AccountSecurityIdentifier Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AccountSecurityIdentifier Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AccountSecurityIdentifier_CheckNullArg_identifier_135372()
        {
            var identifier = new List<Byte>{ 203 }.ToReadOnlyList();

            try
            {
                new AccountSecurityIdentifier((IList<Byte>)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'identifier' isn't checked for null inputs");
        }

        [TestMethod]
        public void AccountSecurityIdentifier_CheckEmptyStringArg_identifier_3()
        {
            CheckEmptyStringArg_identifier_3(string.Empty);
            CheckEmptyStringArg_identifier_3("    ");
            CheckEmptyStringArg_identifier_3(Environment.NewLine);
            CheckEmptyStringArg_identifier_3("\n\r");
        }

        private void CheckEmptyStringArg_identifier_3(string stringArgument)
        {
            var identifier = "text 135373";

            try
            {
                new AccountSecurityIdentifier(stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'identifier' isn't checked for emply values");
        }

        [TestMethod]
        public void AccountSecurityIdentifier_CheckNullArg_identifier_135374()
        {
            var identifier = "text 135373";

            try
            {
                new AccountSecurityIdentifier((String)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'identifier' isn't checked for null inputs");
        }

        [TestMethod]
        public void AccountSecurityIdentifier_CheckNullArg_identifier_135375()
        {
            var identifier = new SecurityIdentifier(WellKnownSidType.NullSid, null);

            try
            {
                new AccountSecurityIdentifier((SecurityIdentifier)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'identifier' isn't checked for null inputs");
        }


        [TestMethod]
        public void AccountSecurityIdentifier_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AccountSecurityIdentifier_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AccountSecurityIdentifier_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AccountSecurityIdentifier_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AgentEncryptionDataTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AgentEncryptionData> objects = new ObjectsCache<AgentEncryptionData>(GetInstances);

        internal static AgentEncryptionData First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AgentEncryptionData Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AgentEncryptionData Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AgentEncryptionData_CheckNullArg_computer()
        {
            var computer = BaseComputerAccountTest.First;
            var key = "text 135376";

            try
            {
                new AgentEncryptionData(null, key);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "computer", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'computer' isn't checked for null inputs");
        }

        [TestMethod]
        public void AgentEncryptionData_CheckEmptyStringArg_key()
        {
            CheckEmptyStringArg_key(string.Empty);
            CheckEmptyStringArg_key("    ");
            CheckEmptyStringArg_key(Environment.NewLine);
            CheckEmptyStringArg_key("\n\r");
        }

        private void CheckEmptyStringArg_key(string stringArgument)
        {
            var computer = BaseComputerAccountTest.First;
            var key = "text 135376";

            try
            {
                new AgentEncryptionData(computer, stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "key", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'key' isn't checked for emply values");
        }

        [TestMethod]
        public void AgentEncryptionData_CheckNullArg_key()
        {
            var computer = BaseComputerAccountTest.First;
            var key = "text 135376";

            try
            {
                new AgentEncryptionData(computer, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "key", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'key' isn't checked for null inputs");
        }


        [TestMethod]
        public void AgentEncryptionData_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AgentEncryptionData_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AgentEncryptionData_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AgentEncryptionData_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class BaseDomainAccountTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<BaseDomainAccount> objects = new ObjectsCache<BaseDomainAccount>(GetInstances);

        internal static BaseDomainAccount First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static BaseDomainAccount Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static BaseDomainAccount Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<BaseDomainAccount>GetInstances()
        {
            return
            new BaseDomainAccount[0].Union(
BaseComputerAccountTest.objects).Union(
DomainAccountTest.objects).Union(
DomainComputerAccountTest.objects).Union(
LocalComputerAccountTest.objects);
        }

        [TestMethod]
        public void BaseDomainAccount_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void BaseDomainAccount_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void BaseDomainAccount_SerializationTest()
        {
            BaseSerializationTest(objects);
        }
    }

    [TestClass]
    public sealed partial class BaseComputerAccountTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<BaseComputerAccount> objects = new ObjectsCache<BaseComputerAccount>(GetInstances);

        internal static BaseComputerAccount First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static BaseComputerAccount Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static BaseComputerAccount Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<BaseComputerAccount>GetInstances()
        {
            return
            new BaseComputerAccount[0].Union(
DomainComputerAccountTest.objects).Union(
LocalComputerAccountTest.objects);
        }

        [TestMethod]
        public void BaseComputerAccount_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void BaseComputerAccount_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void BaseComputerAccount_SerializationTest()
        {
            BaseSerializationTest(objects);
        }
    }

    [TestClass]
    public sealed partial class BaseGroupAccountTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<BaseGroupAccount> objects = new ObjectsCache<BaseGroupAccount>(GetInstances);

        internal static BaseGroupAccount First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static BaseGroupAccount Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static BaseGroupAccount Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<BaseGroupAccount>GetInstances()
        {
            return
            new BaseGroupAccount[0].Union(
DomainComputerGroupAccountTest.objects).Union(
DomainGroupAccountTest.objects).Union(
LocalGroupAccountTest.objects);
        }

        [TestMethod]
        public void BaseGroupAccount_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void BaseGroupAccount_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void BaseGroupAccount_SerializationTest()
        {
            BaseSerializationTest(objects);
        }
    }

    [TestClass]
    public sealed partial class BaseUserAccountTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<BaseUserAccount> objects = new ObjectsCache<BaseUserAccount>(GetInstances);

        internal static BaseUserAccount First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static BaseUserAccount Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static BaseUserAccount Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<BaseUserAccount>GetInstances()
        {
            return
            new BaseUserAccount[0].Union(
DomainComputerUserAccountTest.objects).Union(
DomainUserAccountTest.objects).Union(
LocalUserAccountTest.objects);
        }

        [TestMethod]
        public void BaseUserAccount_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void BaseUserAccount_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void BaseUserAccount_SerializationTest()
        {
            BaseSerializationTest(objects);
        }
    }

    [TestClass]
    public sealed partial class CredentialsTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<Credentials> objects = new ObjectsCache<Credentials>(GetInstances);

        internal static Credentials First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static Credentials Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static Credentials Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void Credentials_CheckNullArg_domain()
        {
            var domain = BaseDomainAccountTest.First;
            var user = "text 135377";
            var password = "text 135378";

            try
            {
                new Credentials(null, user, password);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "domain", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'domain' isn't checked for null inputs");
        }

        [TestMethod]
        public void Credentials_CheckEmptyStringArg_user()
        {
            CheckEmptyStringArg_user(string.Empty);
            CheckEmptyStringArg_user("    ");
            CheckEmptyStringArg_user(Environment.NewLine);
            CheckEmptyStringArg_user("\n\r");
        }

        private void CheckEmptyStringArg_user(string stringArgument)
        {
            var domain = BaseDomainAccountTest.First;
            var user = "text 135377";
            var password = "text 135378";

            try
            {
                new Credentials(domain, stringArgument, password);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "user", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'user' isn't checked for emply values");
        }

        [TestMethod]
        public void Credentials_CheckNullArg_user()
        {
            var domain = BaseDomainAccountTest.First;
            var user = "text 135377";
            var password = "text 135378";

            try
            {
                new Credentials(domain, null, password);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "user", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'user' isn't checked for null inputs");
        }

        [TestMethod]
        public void Credentials_CheckEmptyStringArg_password()
        {
            CheckEmptyStringArg_password(string.Empty);
            CheckEmptyStringArg_password("    ");
            CheckEmptyStringArg_password(Environment.NewLine);
            CheckEmptyStringArg_password("\n\r");
        }

        private void CheckEmptyStringArg_password(string stringArgument)
        {
            var domain = BaseDomainAccountTest.First;
            var user = "text 135377";
            var password = "text 135378";

            try
            {
                new Credentials(domain, user, stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "password", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'password' isn't checked for emply values");
        }

        [TestMethod]
        public void Credentials_CheckNullArg_password()
        {
            var domain = BaseDomainAccountTest.First;
            var user = "text 135377";
            var password = "text 135378";

            try
            {
                new Credentials(domain, user, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "password", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'password' isn't checked for null inputs");
        }


        [TestMethod]
        public void Credentials_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void Credentials_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void Credentials_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void Credentials_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class DomainAccountTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DomainAccount> objects = new ObjectsCache<DomainAccount>(GetInstances);

        internal static DomainAccount First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DomainAccount Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DomainAccount Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DomainAccount_CheckEmptyStringArg_netBiosName()
        {
            CheckEmptyStringArg_netBiosName(string.Empty);
            CheckEmptyStringArg_netBiosName("    ");
            CheckEmptyStringArg_netBiosName(Environment.NewLine);
            CheckEmptyStringArg_netBiosName("\n\r");
        }

        private void CheckEmptyStringArg_netBiosName(string stringArgument)
        {
            var netBiosName = "text 135379";
            var identifier = AccountSecurityIdentifierTest.First;
            var canonicalName = "text 135380";

            try
            {
                new DomainAccount(stringArgument, identifier, canonicalName);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "netBiosName", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'netBiosName' isn't checked for emply values");
        }

        [TestMethod]
        public void DomainAccount_CheckNullArg_netBiosName()
        {
            var netBiosName = "text 135379";
            var identifier = AccountSecurityIdentifierTest.First;
            var canonicalName = "text 135380";

            try
            {
                new DomainAccount(null, identifier, canonicalName);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "netBiosName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'netBiosName' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainAccount_CheckNullArg_identifier()
        {
            var netBiosName = "text 135379";
            var identifier = AccountSecurityIdentifierTest.First;
            var canonicalName = "text 135380";

            try
            {
                new DomainAccount(netBiosName, null, canonicalName);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'identifier' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainAccount_CheckEmptyStringArg_canonicalName()
        {
            CheckEmptyStringArg_canonicalName(string.Empty);
            CheckEmptyStringArg_canonicalName("    ");
            CheckEmptyStringArg_canonicalName(Environment.NewLine);
            CheckEmptyStringArg_canonicalName("\n\r");
        }

        private void CheckEmptyStringArg_canonicalName(string stringArgument)
        {
            var netBiosName = "text 135379";
            var identifier = AccountSecurityIdentifierTest.First;
            var canonicalName = "text 135380";

            try
            {
                new DomainAccount(netBiosName, identifier, stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "canonicalName", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'canonicalName' isn't checked for emply values");
        }

        [TestMethod]
        public void DomainAccount_CheckNullArg_canonicalName()
        {
            var netBiosName = "text 135379";
            var identifier = AccountSecurityIdentifierTest.First;
            var canonicalName = "text 135380";

            try
            {
                new DomainAccount(netBiosName, identifier, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "canonicalName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'canonicalName' isn't checked for null inputs");
        }


        [TestMethod]
        public void DomainAccount_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DomainAccount_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DomainAccount_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DomainAccount_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class DomainComputerAccountTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DomainComputerAccount> objects = new ObjectsCache<DomainComputerAccount>(GetInstances);

        internal static DomainComputerAccount First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DomainComputerAccount Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DomainComputerAccount Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DomainComputerAccount_CheckEmptyStringArg_name()
        {
            CheckEmptyStringArg_name(string.Empty);
            CheckEmptyStringArg_name("    ");
            CheckEmptyStringArg_name(Environment.NewLine);
            CheckEmptyStringArg_name("\n\r");
        }

        private void CheckEmptyStringArg_name(string stringArgument)
        {
            var name = "text 135381";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135382";

            try
            {
                new DomainComputerAccount(stringArgument, identifier, parent, canonicalName);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'name' isn't checked for emply values");
        }

        [TestMethod]
        public void DomainComputerAccount_CheckNullArg_name()
        {
            var name = "text 135381";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135382";

            try
            {
                new DomainComputerAccount(null, identifier, parent, canonicalName);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'name' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainComputerAccount_CheckNullArg_identifier()
        {
            var name = "text 135381";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135382";

            try
            {
                new DomainComputerAccount(name, null, parent, canonicalName);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'identifier' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainComputerAccount_CheckNullArg_parent()
        {
            var name = "text 135381";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135382";

            try
            {
                new DomainComputerAccount(name, identifier, null, canonicalName);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "parent", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'parent' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainComputerAccount_CheckEmptyStringArg_canonicalName()
        {
            CheckEmptyStringArg_canonicalName(string.Empty);
            CheckEmptyStringArg_canonicalName("    ");
            CheckEmptyStringArg_canonicalName(Environment.NewLine);
            CheckEmptyStringArg_canonicalName("\n\r");
        }

        private void CheckEmptyStringArg_canonicalName(string stringArgument)
        {
            var name = "text 135381";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135382";

            try
            {
                new DomainComputerAccount(name, identifier, parent, stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "canonicalName", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'canonicalName' isn't checked for emply values");
        }

        [TestMethod]
        public void DomainComputerAccount_CheckNullArg_canonicalName()
        {
            var name = "text 135381";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135382";

            try
            {
                new DomainComputerAccount(name, identifier, parent, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "canonicalName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'canonicalName' isn't checked for null inputs");
        }


        [TestMethod]
        public void DomainComputerAccount_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DomainComputerAccount_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DomainComputerAccount_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DomainComputerAccount_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class DomainComputerGroupAccountTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DomainComputerGroupAccount> objects = new ObjectsCache<DomainComputerGroupAccount>(GetInstances);

        internal static DomainComputerGroupAccount First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DomainComputerGroupAccount Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DomainComputerGroupAccount Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DomainComputerGroupAccount_CheckEmptyStringArg_name()
        {
            CheckEmptyStringArg_name(string.Empty);
            CheckEmptyStringArg_name("    ");
            CheckEmptyStringArg_name(Environment.NewLine);
            CheckEmptyStringArg_name("\n\r");
        }

        private void CheckEmptyStringArg_name(string stringArgument)
        {
            var name = "text 135383";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainComputerAccountTest.First;

            try
            {
                new DomainComputerGroupAccount(stringArgument, identifier, parent);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'name' isn't checked for emply values");
        }

        [TestMethod]
        public void DomainComputerGroupAccount_CheckNullArg_name()
        {
            var name = "text 135383";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainComputerAccountTest.First;

            try
            {
                new DomainComputerGroupAccount(null, identifier, parent);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'name' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainComputerGroupAccount_CheckNullArg_identifier()
        {
            var name = "text 135383";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainComputerAccountTest.First;

            try
            {
                new DomainComputerGroupAccount(name, null, parent);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'identifier' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainComputerGroupAccount_CheckNullArg_parent()
        {
            var name = "text 135383";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainComputerAccountTest.First;

            try
            {
                new DomainComputerGroupAccount(name, identifier, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "parent", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'parent' isn't checked for null inputs");
        }


        [TestMethod]
        public void DomainComputerGroupAccount_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DomainComputerGroupAccount_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DomainComputerGroupAccount_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DomainComputerGroupAccount_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class DomainComputerUserAccountTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DomainComputerUserAccount> objects = new ObjectsCache<DomainComputerUserAccount>(GetInstances);

        internal static DomainComputerUserAccount First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DomainComputerUserAccount Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DomainComputerUserAccount Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DomainComputerUserAccount_CheckEmptyStringArg_shortName()
        {
            CheckEmptyStringArg_shortName(string.Empty);
            CheckEmptyStringArg_shortName("    ");
            CheckEmptyStringArg_shortName(Environment.NewLine);
            CheckEmptyStringArg_shortName("\n\r");
        }

        private void CheckEmptyStringArg_shortName(string stringArgument)
        {
            var shortName = "text 135384";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainComputerAccountTest.First;

            try
            {
                new DomainComputerUserAccount(stringArgument, identifier, parent);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "shortName", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'shortName' isn't checked for emply values");
        }

        [TestMethod]
        public void DomainComputerUserAccount_CheckNullArg_shortName()
        {
            var shortName = "text 135384";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainComputerAccountTest.First;

            try
            {
                new DomainComputerUserAccount(null, identifier, parent);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "shortName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'shortName' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainComputerUserAccount_CheckNullArg_identifier()
        {
            var shortName = "text 135384";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainComputerAccountTest.First;

            try
            {
                new DomainComputerUserAccount(shortName, null, parent);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'identifier' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainComputerUserAccount_CheckNullArg_parent()
        {
            var shortName = "text 135384";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainComputerAccountTest.First;

            try
            {
                new DomainComputerUserAccount(shortName, identifier, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "parent", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'parent' isn't checked for null inputs");
        }


        [TestMethod]
        public void DomainComputerUserAccount_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DomainComputerUserAccount_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DomainComputerUserAccount_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DomainComputerUserAccount_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class DomainGroupAccountTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DomainGroupAccount> objects = new ObjectsCache<DomainGroupAccount>(GetInstances);

        internal static DomainGroupAccount First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DomainGroupAccount Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DomainGroupAccount Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DomainGroupAccount_CheckEmptyStringArg_name()
        {
            CheckEmptyStringArg_name(string.Empty);
            CheckEmptyStringArg_name("    ");
            CheckEmptyStringArg_name(Environment.NewLine);
            CheckEmptyStringArg_name("\n\r");
        }

        private void CheckEmptyStringArg_name(string stringArgument)
        {
            var name = "text 135385";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135386";

            try
            {
                new DomainGroupAccount(stringArgument, identifier, parent, canonicalName);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'name' isn't checked for emply values");
        }

        [TestMethod]
        public void DomainGroupAccount_CheckNullArg_name()
        {
            var name = "text 135385";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135386";

            try
            {
                new DomainGroupAccount(null, identifier, parent, canonicalName);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'name' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainGroupAccount_CheckNullArg_identifier()
        {
            var name = "text 135385";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135386";

            try
            {
                new DomainGroupAccount(name, null, parent, canonicalName);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'identifier' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainGroupAccount_CheckNullArg_parent()
        {
            var name = "text 135385";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135386";

            try
            {
                new DomainGroupAccount(name, identifier, null, canonicalName);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "parent", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'parent' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainGroupAccount_CheckEmptyStringArg_canonicalName()
        {
            CheckEmptyStringArg_canonicalName(string.Empty);
            CheckEmptyStringArg_canonicalName("    ");
            CheckEmptyStringArg_canonicalName(Environment.NewLine);
            CheckEmptyStringArg_canonicalName("\n\r");
        }

        private void CheckEmptyStringArg_canonicalName(string stringArgument)
        {
            var name = "text 135385";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135386";

            try
            {
                new DomainGroupAccount(name, identifier, parent, stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "canonicalName", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'canonicalName' isn't checked for emply values");
        }

        [TestMethod]
        public void DomainGroupAccount_CheckNullArg_canonicalName()
        {
            var name = "text 135385";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135386";

            try
            {
                new DomainGroupAccount(name, identifier, parent, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "canonicalName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'canonicalName' isn't checked for null inputs");
        }


        [TestMethod]
        public void DomainGroupAccount_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DomainGroupAccount_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DomainGroupAccount_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DomainGroupAccount_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class DomainUserAccountTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DomainUserAccount> objects = new ObjectsCache<DomainUserAccount>(GetInstances);

        internal static DomainUserAccount First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DomainUserAccount Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DomainUserAccount Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DomainUserAccount_CheckEmptyStringArg_shortName()
        {
            CheckEmptyStringArg_shortName(string.Empty);
            CheckEmptyStringArg_shortName("    ");
            CheckEmptyStringArg_shortName(Environment.NewLine);
            CheckEmptyStringArg_shortName("\n\r");
        }

        private void CheckEmptyStringArg_shortName(string stringArgument)
        {
            var shortName = "text 135387";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135388";

            try
            {
                new DomainUserAccount(stringArgument, identifier, parent, canonicalName);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "shortName", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'shortName' isn't checked for emply values");
        }

        [TestMethod]
        public void DomainUserAccount_CheckNullArg_shortName()
        {
            var shortName = "text 135387";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135388";

            try
            {
                new DomainUserAccount(null, identifier, parent, canonicalName);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "shortName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'shortName' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainUserAccount_CheckNullArg_identifier()
        {
            var shortName = "text 135387";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135388";

            try
            {
                new DomainUserAccount(shortName, null, parent, canonicalName);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'identifier' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainUserAccount_CheckNullArg_parent()
        {
            var shortName = "text 135387";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135388";

            try
            {
                new DomainUserAccount(shortName, identifier, null, canonicalName);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "parent", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'parent' isn't checked for null inputs");
        }

        [TestMethod]
        public void DomainUserAccount_CheckEmptyStringArg_canonicalName()
        {
            CheckEmptyStringArg_canonicalName(string.Empty);
            CheckEmptyStringArg_canonicalName("    ");
            CheckEmptyStringArg_canonicalName(Environment.NewLine);
            CheckEmptyStringArg_canonicalName("\n\r");
        }

        private void CheckEmptyStringArg_canonicalName(string stringArgument)
        {
            var shortName = "text 135387";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135388";

            try
            {
                new DomainUserAccount(shortName, identifier, parent, stringArgument);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "canonicalName", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'canonicalName' isn't checked for emply values");
        }

        [TestMethod]
        public void DomainUserAccount_CheckNullArg_canonicalName()
        {
            var shortName = "text 135387";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = DomainAccountTest.First;
            var canonicalName = "text 135388";

            try
            {
                new DomainUserAccount(shortName, identifier, parent, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "canonicalName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'canonicalName' isn't checked for null inputs");
        }


        [TestMethod]
        public void DomainUserAccount_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DomainUserAccount_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DomainUserAccount_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DomainUserAccount_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class LocalComputerAccountTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<LocalComputerAccount> objects = new ObjectsCache<LocalComputerAccount>(GetInstances);

        internal static LocalComputerAccount First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static LocalComputerAccount Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static LocalComputerAccount Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void LocalComputerAccount_CheckEmptyStringArg_name()
        {
            CheckEmptyStringArg_name(string.Empty);
            CheckEmptyStringArg_name("    ");
            CheckEmptyStringArg_name(Environment.NewLine);
            CheckEmptyStringArg_name("\n\r");
        }

        private void CheckEmptyStringArg_name(string stringArgument)
        {
            var name = "text 135389";
            var identifier = AccountSecurityIdentifierTest.First;

            try
            {
                new LocalComputerAccount(stringArgument, identifier);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'name' isn't checked for emply values");
        }

        [TestMethod]
        public void LocalComputerAccount_CheckNullArg_name()
        {
            var name = "text 135389";
            var identifier = AccountSecurityIdentifierTest.First;

            try
            {
                new LocalComputerAccount(null, identifier);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'name' isn't checked for null inputs");
        }

        [TestMethod]
        public void LocalComputerAccount_CheckNullArg_identifier()
        {
            var name = "text 135389";
            var identifier = AccountSecurityIdentifierTest.First;

            try
            {
                new LocalComputerAccount(name, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'identifier' isn't checked for null inputs");
        }


        [TestMethod]
        public void LocalComputerAccount_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void LocalComputerAccount_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void LocalComputerAccount_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void LocalComputerAccount_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class LocalGroupAccountTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<LocalGroupAccount> objects = new ObjectsCache<LocalGroupAccount>(GetInstances);

        internal static LocalGroupAccount First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static LocalGroupAccount Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static LocalGroupAccount Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void LocalGroupAccount_CheckEmptyStringArg_name()
        {
            CheckEmptyStringArg_name(string.Empty);
            CheckEmptyStringArg_name("    ");
            CheckEmptyStringArg_name(Environment.NewLine);
            CheckEmptyStringArg_name("\n\r");
        }

        private void CheckEmptyStringArg_name(string stringArgument)
        {
            var name = "text 135390";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = LocalComputerAccountTest.First;

            try
            {
                new LocalGroupAccount(stringArgument, identifier, parent);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'name' isn't checked for emply values");
        }

        [TestMethod]
        public void LocalGroupAccount_CheckNullArg_name()
        {
            var name = "text 135390";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = LocalComputerAccountTest.First;

            try
            {
                new LocalGroupAccount(null, identifier, parent);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'name' isn't checked for null inputs");
        }

        [TestMethod]
        public void LocalGroupAccount_CheckNullArg_identifier()
        {
            var name = "text 135390";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = LocalComputerAccountTest.First;

            try
            {
                new LocalGroupAccount(name, null, parent);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'identifier' isn't checked for null inputs");
        }

        [TestMethod]
        public void LocalGroupAccount_CheckNullArg_parent()
        {
            var name = "text 135390";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = LocalComputerAccountTest.First;

            try
            {
                new LocalGroupAccount(name, identifier, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "parent", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'parent' isn't checked for null inputs");
        }


        [TestMethod]
        public void LocalGroupAccount_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void LocalGroupAccount_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void LocalGroupAccount_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void LocalGroupAccount_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class LocalUserAccountTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<LocalUserAccount> objects = new ObjectsCache<LocalUserAccount>(GetInstances);

        internal static LocalUserAccount First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static LocalUserAccount Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static LocalUserAccount Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void LocalUserAccount_CheckEmptyStringArg_shortName()
        {
            CheckEmptyStringArg_shortName(string.Empty);
            CheckEmptyStringArg_shortName("    ");
            CheckEmptyStringArg_shortName(Environment.NewLine);
            CheckEmptyStringArg_shortName("\n\r");
        }

        private void CheckEmptyStringArg_shortName(string stringArgument)
        {
            var shortName = "text 135391";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = LocalComputerAccountTest.First;

            try
            {
                new LocalUserAccount(stringArgument, identifier, parent);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "shortName", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'shortName' isn't checked for emply values");
        }

        [TestMethod]
        public void LocalUserAccount_CheckNullArg_shortName()
        {
            var shortName = "text 135391";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = LocalComputerAccountTest.First;

            try
            {
                new LocalUserAccount(null, identifier, parent);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "shortName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'shortName' isn't checked for null inputs");
        }

        [TestMethod]
        public void LocalUserAccount_CheckNullArg_identifier()
        {
            var shortName = "text 135391";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = LocalComputerAccountTest.First;

            try
            {
                new LocalUserAccount(shortName, null, parent);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "identifier", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'identifier' isn't checked for null inputs");
        }

        [TestMethod]
        public void LocalUserAccount_CheckNullArg_parent()
        {
            var shortName = "text 135391";
            var identifier = AccountSecurityIdentifierTest.First;
            var parent = LocalComputerAccountTest.First;

            try
            {
                new LocalUserAccount(shortName, identifier, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "parent", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'parent' isn't checked for null inputs");
        }


        [TestMethod]
        public void LocalUserAccount_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void LocalUserAccount_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void LocalUserAccount_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void LocalUserAccount_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class OrganizationalUnitTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<OrganizationalUnit> objects = new ObjectsCache<OrganizationalUnit>(GetInstances);

        internal static OrganizationalUnit First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static OrganizationalUnit Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static OrganizationalUnit Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void OrganizationalUnit_CheckEmptyStringArg_name()
        {
            CheckEmptyStringArg_name(string.Empty);
            CheckEmptyStringArg_name("    ");
            CheckEmptyStringArg_name(Environment.NewLine);
            CheckEmptyStringArg_name("\n\r");
        }

        private void CheckEmptyStringArg_name(string stringArgument)
        {
            var name = "text 135392";
            var canonicalName = "text 135393";
            var parent = DomainAccountTest.First;

            try
            {
                new OrganizationalUnit(stringArgument, canonicalName, parent);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'name' isn't checked for emply values");
        }

        [TestMethod]
        public void OrganizationalUnit_CheckNullArg_name()
        {
            var name = "text 135392";
            var canonicalName = "text 135393";
            var parent = DomainAccountTest.First;

            try
            {
                new OrganizationalUnit(null, canonicalName, parent);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'name' isn't checked for null inputs");
        }

        [TestMethod]
        public void OrganizationalUnit_CheckEmptyStringArg_canonicalName()
        {
            CheckEmptyStringArg_canonicalName(string.Empty);
            CheckEmptyStringArg_canonicalName("    ");
            CheckEmptyStringArg_canonicalName(Environment.NewLine);
            CheckEmptyStringArg_canonicalName("\n\r");
        }

        private void CheckEmptyStringArg_canonicalName(string stringArgument)
        {
            var name = "text 135392";
            var canonicalName = "text 135393";
            var parent = DomainAccountTest.First;

            try
            {
                new OrganizationalUnit(name, stringArgument, parent);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "canonicalName", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'canonicalName' isn't checked for emply values");
        }

        [TestMethod]
        public void OrganizationalUnit_CheckNullArg_canonicalName()
        {
            var name = "text 135392";
            var canonicalName = "text 135393";
            var parent = DomainAccountTest.First;

            try
            {
                new OrganizationalUnit(name, null, parent);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "canonicalName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'canonicalName' isn't checked for null inputs");
        }

        [TestMethod]
        public void OrganizationalUnit_CheckNullArg_parent()
        {
            var name = "text 135392";
            var canonicalName = "text 135393";
            var parent = DomainAccountTest.First;

            try
            {
                new OrganizationalUnit(name, canonicalName, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "parent", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'parent' isn't checked for null inputs");
        }


        [TestMethod]
        public void OrganizationalUnit_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void OrganizationalUnit_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void OrganizationalUnit_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void OrganizationalUnit_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules
{

    [TestClass]
    public sealed partial class ActionDataTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<ActionData> objects = new ObjectsCache<ActionData>(GetInstances);

        internal static ActionData First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static ActionData Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static ActionData Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void ActionData_CheckWrongEnumArg_block()
        {
            var block = EnumHelper<BlockActionType>.Values.First();
            var audit = EnumHelper<AuditActionType>.Values.First();

            try
            {
                new ActionData((LeakBlocker.Libraries.Common.Entities.Settings.Rules.BlockActionType)5, audit);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "block", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'block' isn't checked for the not-defined values");
        }

        [TestMethod]
        public void ActionData_CheckWrongEnumArg_audit()
        {
            var block = EnumHelper<BlockActionType>.Values.First();
            var audit = EnumHelper<AuditActionType>.Values.First();

            try
            {
                new ActionData(block, (LeakBlocker.Libraries.Common.Entities.Settings.Rules.AuditActionType)5);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "audit", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'audit' isn't checked for the not-defined values");
        }


        [TestMethod]
        public void ActionData_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void ActionData_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void ActionData_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void ActionData_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class RuleTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<Rule> objects = new ObjectsCache<Rule>(GetInstances);

        internal static Rule First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static Rule Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static Rule Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void Rule_CheckEmptyStringArg_name()
        {
            CheckEmptyStringArg_name(string.Empty);
            CheckEmptyStringArg_name("    ");
            CheckEmptyStringArg_name(Environment.NewLine);
            CheckEmptyStringArg_name("\n\r");
        }

        private void CheckEmptyStringArg_name(string stringArgument)
        {
            var name = "text 135394";
            var priority = 135395;
            var rootCondition = BaseRuleConditionTest.First;
            var actions = ActionDataTest.First;

            try
            {
                new Rule(stringArgument, priority, rootCondition, actions);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'name' isn't checked for emply values");
        }

        [TestMethod]
        public void Rule_CheckNullArg_name()
        {
            var name = "text 135394";
            var priority = 135395;
            var rootCondition = BaseRuleConditionTest.First;
            var actions = ActionDataTest.First;

            try
            {
                new Rule(null, priority, rootCondition, actions);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "name", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'name' isn't checked for null inputs");
        }

        [TestMethod]
        public void Rule_CheckNullArg_rootCondition()
        {
            var name = "text 135394";
            var priority = 135395;
            var rootCondition = BaseRuleConditionTest.First;
            var actions = ActionDataTest.First;

            try
            {
                new Rule(name, priority, null, actions);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "rootCondition", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'rootCondition' isn't checked for null inputs");
        }

        [TestMethod]
        public void Rule_CheckNullArg_actions()
        {
            var name = "text 135394";
            var priority = 135395;
            var rootCondition = BaseRuleConditionTest.First;
            var actions = ActionDataTest.First;

            try
            {
                new Rule(name, priority, rootCondition, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "actions", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'actions' isn't checked for null inputs");
        }


        [TestMethod]
        public void Rule_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void Rule_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void Rule_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void Rule_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.Libraries.Common.Tests.Entities.Settings.TemporaryAccess
{

    [TestClass]
    public sealed partial class BaseTemporaryAccessConditionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<BaseTemporaryAccessCondition> objects = new ObjectsCache<BaseTemporaryAccessCondition>(GetInstances);

        internal static BaseTemporaryAccessCondition First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static BaseTemporaryAccessCondition Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static BaseTemporaryAccessCondition Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<BaseTemporaryAccessCondition>GetInstances()
        {
            return
            new BaseTemporaryAccessCondition[0].Union(
ComputerTemporaryAccessConditionTest.objects).Union(
DeviceTemporaryAccessConditionTest.objects).Union(
UserTemporaryAccessConditionTest.objects);
        }

        [TestMethod]
        public void BaseTemporaryAccessCondition_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void BaseTemporaryAccessCondition_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void BaseTemporaryAccessCondition_SerializationTest()
        {
            BaseSerializationTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ComputerTemporaryAccessConditionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<ComputerTemporaryAccessCondition> objects = new ObjectsCache<ComputerTemporaryAccessCondition>(GetInstances);

        internal static ComputerTemporaryAccessCondition First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static ComputerTemporaryAccessCondition Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static ComputerTemporaryAccessCondition Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void ComputerTemporaryAccessCondition_CheckNullArg_computer()
        {
            var computer = BaseComputerAccountTest.First;
            var endTime = TimeTest.First;
            var readOnly = true;

            try
            {
                new ComputerTemporaryAccessCondition(null, endTime, readOnly);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "computer", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'computer' isn't checked for null inputs");
        }

        [TestMethod]
        public void ComputerTemporaryAccessCondition_CheckNullArg_endTime()
        {
            var computer = BaseComputerAccountTest.First;
            var endTime = TimeTest.First;
            var readOnly = true;

            try
            {
                new ComputerTemporaryAccessCondition(computer, null, readOnly);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "endTime", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'endTime' isn't checked for null inputs");
        }


        [TestMethod]
        public void ComputerTemporaryAccessCondition_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void ComputerTemporaryAccessCondition_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void ComputerTemporaryAccessCondition_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void ComputerTemporaryAccessCondition_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class DeviceTemporaryAccessConditionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DeviceTemporaryAccessCondition> objects = new ObjectsCache<DeviceTemporaryAccessCondition>(GetInstances);

        internal static DeviceTemporaryAccessCondition First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DeviceTemporaryAccessCondition Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DeviceTemporaryAccessCondition Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DeviceTemporaryAccessCondition_CheckNullArg_device()
        {
            var device = DeviceDescriptionTest.First;
            var endTime = TimeTest.First;
            var readOnly = true;

            try
            {
                new DeviceTemporaryAccessCondition(null, endTime, readOnly);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "device", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'device' isn't checked for null inputs");
        }

        [TestMethod]
        public void DeviceTemporaryAccessCondition_CheckNullArg_endTime()
        {
            var device = DeviceDescriptionTest.First;
            var endTime = TimeTest.First;
            var readOnly = true;

            try
            {
                new DeviceTemporaryAccessCondition(device, null, readOnly);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "endTime", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'endTime' isn't checked for null inputs");
        }


        [TestMethod]
        public void DeviceTemporaryAccessCondition_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DeviceTemporaryAccessCondition_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DeviceTemporaryAccessCondition_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DeviceTemporaryAccessCondition_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class UserTemporaryAccessConditionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<UserTemporaryAccessCondition> objects = new ObjectsCache<UserTemporaryAccessCondition>(GetInstances);

        internal static UserTemporaryAccessCondition First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static UserTemporaryAccessCondition Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static UserTemporaryAccessCondition Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void UserTemporaryAccessCondition_CheckNullArg_user()
        {
            var user = BaseUserAccountTest.First;
            var endTime = TimeTest.First;
            var readOnly = true;

            try
            {
                new UserTemporaryAccessCondition(null, endTime, readOnly);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "user", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'user' isn't checked for null inputs");
        }

        [TestMethod]
        public void UserTemporaryAccessCondition_CheckNullArg_endTime()
        {
            var user = BaseUserAccountTest.First;
            var endTime = TimeTest.First;
            var readOnly = true;

            try
            {
                new UserTemporaryAccessCondition(user, null, readOnly);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "endTime", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'endTime' isn't checked for null inputs");
        }


        [TestMethod]
        public void UserTemporaryAccessCondition_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void UserTemporaryAccessCondition_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void UserTemporaryAccessCondition_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void UserTemporaryAccessCondition_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.Libraries.Common.Tests.License
{

    [TestClass]
    public sealed partial class LicenseInfoTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<LicenseInfo> objects = new ObjectsCache<LicenseInfo>(GetInstances);

        internal static LicenseInfo First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static LicenseInfo Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static LicenseInfo Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void LicenseInfo_CheckNullArg_suppressedLicenses_135399()
        {
            var count = 135396;
            var suppressedLicenses = new List<LicenseInfo>{ LicenseInfoTest.First }.ToReadOnlyList();
            var signature = "text 135397";
            var companyName = "text 135398";
            var creationTime = TimeTest.First;

            try
            {
                new LicenseInfo(count, (IReadOnlyCollection<LicenseInfo>)null, signature, companyName, creationTime);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "suppressedLicenses", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'suppressedLicenses' isn't checked for null inputs");
        }

        [TestMethod]
        public void LicenseInfo_CheckEmptyStringArg_signature_4()
        {
            CheckEmptyStringArg_signature_4(string.Empty);
            CheckEmptyStringArg_signature_4("    ");
            CheckEmptyStringArg_signature_4(Environment.NewLine);
            CheckEmptyStringArg_signature_4("\n\r");
        }

        private void CheckEmptyStringArg_signature_4(string stringArgument)
        {
            var count = 135396;
            var suppressedLicenses = new List<LicenseInfo>{ LicenseInfoTest.First }.ToReadOnlyList();
            var signature = "text 135397";
            var companyName = "text 135398";
            var creationTime = TimeTest.First;

            try
            {
                new LicenseInfo(count, suppressedLicenses, stringArgument, companyName, creationTime);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "signature", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'signature' isn't checked for emply values");
        }

        [TestMethod]
        public void LicenseInfo_CheckNullArg_signature_135400()
        {
            var count = 135396;
            var suppressedLicenses = new List<LicenseInfo>{ LicenseInfoTest.First }.ToReadOnlyList();
            var signature = "text 135397";
            var companyName = "text 135398";
            var creationTime = TimeTest.First;

            try
            {
                new LicenseInfo(count, suppressedLicenses, (String)null, companyName, creationTime);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "signature", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'signature' isn't checked for null inputs");
        }

        [TestMethod]
        public void LicenseInfo_CheckEmptyStringArg_companyName_5()
        {
            CheckEmptyStringArg_companyName_5(string.Empty);
            CheckEmptyStringArg_companyName_5("    ");
            CheckEmptyStringArg_companyName_5(Environment.NewLine);
            CheckEmptyStringArg_companyName_5("\n\r");
        }

        private void CheckEmptyStringArg_companyName_5(string stringArgument)
        {
            var count = 135396;
            var suppressedLicenses = new List<LicenseInfo>{ LicenseInfoTest.First }.ToReadOnlyList();
            var signature = "text 135397";
            var companyName = "text 135398";
            var creationTime = TimeTest.First;

            try
            {
                new LicenseInfo(count, suppressedLicenses, signature, stringArgument, creationTime);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "companyName", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'companyName' isn't checked for emply values");
        }

        [TestMethod]
        public void LicenseInfo_CheckNullArg_companyName_135401()
        {
            var count = 135396;
            var suppressedLicenses = new List<LicenseInfo>{ LicenseInfoTest.First }.ToReadOnlyList();
            var signature = "text 135397";
            var companyName = "text 135398";
            var creationTime = TimeTest.First;

            try
            {
                new LicenseInfo(count, suppressedLicenses, signature, (String)null, creationTime);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "companyName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'companyName' isn't checked for null inputs");
        }

        [TestMethod]
        public void LicenseInfo_CheckNullArg_creationTime_135402()
        {
            var count = 135396;
            var suppressedLicenses = new List<LicenseInfo>{ LicenseInfoTest.First }.ToReadOnlyList();
            var signature = "text 135397";
            var companyName = "text 135398";
            var creationTime = TimeTest.First;

            try
            {
                new LicenseInfo(count, suppressedLicenses, signature, companyName, (Time)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "creationTime", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'creationTime' isn't checked for null inputs");
        }

        [TestMethod]
        public void LicenseInfo_CheckEmptyStringArg_signature_6()
        {
            CheckEmptyStringArg_signature_6(string.Empty);
            CheckEmptyStringArg_signature_6("    ");
            CheckEmptyStringArg_signature_6(Environment.NewLine);
            CheckEmptyStringArg_signature_6("\n\r");
        }

        private void CheckEmptyStringArg_signature_6(string stringArgument)
        {
            var signature = "text 135403";
            var companyName = "text 135404";
            var creationTime = TimeTest.First;
            var expirationTime = TimeTest.First;

            try
            {
                new LicenseInfo(stringArgument, companyName, creationTime, expirationTime);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "signature", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'signature' isn't checked for emply values");
        }

        [TestMethod]
        public void LicenseInfo_CheckNullArg_signature_135405()
        {
            var signature = "text 135403";
            var companyName = "text 135404";
            var creationTime = TimeTest.First;
            var expirationTime = TimeTest.First;

            try
            {
                new LicenseInfo((String)null, companyName, creationTime, expirationTime);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "signature", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'signature' isn't checked for null inputs");
        }

        [TestMethod]
        public void LicenseInfo_CheckEmptyStringArg_companyName_7()
        {
            CheckEmptyStringArg_companyName_7(string.Empty);
            CheckEmptyStringArg_companyName_7("    ");
            CheckEmptyStringArg_companyName_7(Environment.NewLine);
            CheckEmptyStringArg_companyName_7("\n\r");
        }

        private void CheckEmptyStringArg_companyName_7(string stringArgument)
        {
            var signature = "text 135403";
            var companyName = "text 135404";
            var creationTime = TimeTest.First;
            var expirationTime = TimeTest.First;

            try
            {
                new LicenseInfo(signature, stringArgument, creationTime, expirationTime);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "companyName", ex.ParamName );

                return;
            }

            Assert.Fail("String in the argument 'companyName' isn't checked for emply values");
        }

        [TestMethod]
        public void LicenseInfo_CheckNullArg_companyName_135406()
        {
            var signature = "text 135403";
            var companyName = "text 135404";
            var creationTime = TimeTest.First;
            var expirationTime = TimeTest.First;

            try
            {
                new LicenseInfo(signature, (String)null, creationTime, expirationTime);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "companyName", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'companyName' isn't checked for null inputs");
        }

        [TestMethod]
        public void LicenseInfo_CheckNullArg_creationTime_135407()
        {
            var signature = "text 135403";
            var companyName = "text 135404";
            var creationTime = TimeTest.First;
            var expirationTime = TimeTest.First;

            try
            {
                new LicenseInfo(signature, companyName, (Time)null, expirationTime);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "creationTime", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'creationTime' isn't checked for null inputs");
        }

        [TestMethod]
        public void LicenseInfo_CheckNullArg_expirationTime_135408()
        {
            var signature = "text 135403";
            var companyName = "text 135404";
            var creationTime = TimeTest.First;
            var expirationTime = TimeTest.First;

            try
            {
                new LicenseInfo(signature, companyName, creationTime, (Time)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "expirationTime", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'expirationTime' isn't checked for null inputs");
        }


        [TestMethod]
        public void LicenseInfo_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void LicenseInfo_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void LicenseInfo_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void LicenseInfo_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class LicenseRequestDataTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<LicenseRequestData> objects = new ObjectsCache<LicenseRequestData>(GetInstances);

        internal static LicenseRequestData First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static LicenseRequestData Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static LicenseRequestData Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void LicenseRequestData_CheckNullArg_userContact()
        {
            var userContact = UserContactInformationTest.First;
            var count = 135409;

            try
            {
                new LicenseRequestData(null, count);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "userContact", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'userContact' isn't checked for null inputs");
        }


        [TestMethod]
        public void LicenseRequestData_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void LicenseRequestData_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void LicenseRequestData_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void LicenseRequestData_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.Libraries.Common.Tests
{

    [TestClass]
    public sealed partial class TimeTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<Time> objects = new ObjectsCache<Time>(GetInstances);

        internal static Time First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static Time Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static Time Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        

        [TestMethod]
        public void Time_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void Time_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void Time_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void Time_ToStringTest()
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

