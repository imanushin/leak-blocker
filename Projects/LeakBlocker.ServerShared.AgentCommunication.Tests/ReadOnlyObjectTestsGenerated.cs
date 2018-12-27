
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Cryptography;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.ServerShared.AgentCommunication;
using LeakBlocker.ServerShared.AgentCommunication.Tests;
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


namespace LeakBlocker.ServerShared.AgentCommunication.Tests
{

    [TestClass]
    public sealed partial class AgentAuditItemTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AgentAuditItem> objects = new ObjectsCache<AgentAuditItem>(GetInstances);

        internal static AgentAuditItem First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AgentAuditItem Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AgentAuditItem Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AgentAuditItem_CheckWrongEnumArg_type()
        {
            var type = EnumHelper<AuditItemType>.Values.First();
            var time = TimeTest.First;
            var user = BaseUserAccountTest.First;
            var textData = "text 135346";
            var additionalTextData = "text 135347";
            var device = DeviceDescriptionTest.First;
            var configuration = 135348;
            var number = 135349;

            try
            {
                new AgentAuditItem((LeakBlocker.Libraries.Common.Entities.Audit.AuditItemType)100036, time, user, textData, additionalTextData, device, configuration, number);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "type", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'type' isn't checked for the not-defined values");
        }

        [TestMethod]
        public void AgentAuditItem_CheckNullArg_time()
        {
            var type = EnumHelper<AuditItemType>.Values.First();
            var time = TimeTest.First;
            var user = BaseUserAccountTest.First;
            var textData = "text 135346";
            var additionalTextData = "text 135347";
            var device = DeviceDescriptionTest.First;
            var configuration = 135348;
            var number = 135349;

            try
            {
                new AgentAuditItem(type, null, user, textData, additionalTextData, device, configuration, number);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "time", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'time' isn't checked for null inputs");
        }


        [TestMethod]
        public void AgentAuditItem_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AgentAuditItem_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AgentAuditItem_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AgentAuditItem_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AgentAuditItemDataTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AgentAuditItemData> objects = new ObjectsCache<AgentAuditItemData>(GetInstances);

        internal static AgentAuditItemData First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AgentAuditItemData Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AgentAuditItemData Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AgentAuditItemData_CheckNullArg_auditItem()
        {
            var auditItem = AgentAuditItemTest.First;

            try
            {
                new AgentAuditItemData(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "auditItem", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'auditItem' isn't checked for null inputs");
        }


        [TestMethod]
        public void AgentAuditItemData_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AgentAuditItemData_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AgentAuditItemData_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AgentAuditItemData_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AgentConfigurationTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AgentConfiguration> objects = new ObjectsCache<AgentConfiguration>(GetInstances);

        internal static AgentConfiguration First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AgentConfiguration Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AgentConfiguration Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AgentConfiguration_CheckNullArg_settings()
        {
            var settings = ProgramConfigurationTest.First;
            var licensed = true;
            var managed = true;

            try
            {
                new AgentConfiguration(null, licensed, managed);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "settings", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'settings' isn't checked for null inputs");
        }


        [TestMethod]
        public void AgentConfiguration_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AgentConfiguration_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AgentConfiguration_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AgentConfiguration_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AgentStateTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AgentState> objects = new ObjectsCache<AgentState>(GetInstances);

        internal static AgentState First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AgentState Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AgentState Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AgentState_CheckNullArg_audit()
        {
            var audit = AuditItemPackageTest.First;
            var deviceAccess = DeviceAccessMapTest.First;

            try
            {
                new AgentState(null, deviceAccess);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "audit", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'audit' isn't checked for null inputs");
        }

        [TestMethod]
        public void AgentState_CheckNullArg_deviceAccess()
        {
            var audit = AuditItemPackageTest.First;
            var deviceAccess = DeviceAccessMapTest.First;

            try
            {
                new AgentState(audit, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "deviceAccess", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'deviceAccess' isn't checked for null inputs");
        }


        [TestMethod]
        public void AgentState_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AgentState_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AgentState_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AgentState_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AuditItemPackageTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AuditItemPackage> objects = new ObjectsCache<AuditItemPackage>(GetInstances);

        internal static AuditItemPackage First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AuditItemPackage Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AuditItemPackage Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AuditItemPackage_CheckNullArg_data()
        {
            var data = new List<AgentAuditItem>{ AgentAuditItemTest.First }.ToReadOnlyList();

            try
            {
                new AuditItemPackage(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "data", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'data' isn't checked for null inputs");
        }


        [TestMethod]
        public void AuditItemPackage_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AuditItemPackage_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AuditItemPackage_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AuditItemPackage_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class KeyExchangeReplyTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<KeyExchangeReply> objects = new ObjectsCache<KeyExchangeReply>(GetInstances);

        internal static KeyExchangeReply First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static KeyExchangeReply Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static KeyExchangeReply Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void KeyExchangeReply_CheckNullArg_request()
        {
            var request = KeyExchangeRequestTest.First;
            var key = SymmetricEncryptionKeyTest.First;

            try
            {
                new KeyExchangeReply(null, key);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "request", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'request' isn't checked for null inputs");
        }

        [TestMethod]
        public void KeyExchangeReply_CheckNullArg_key()
        {
            var request = KeyExchangeRequestTest.First;
            var key = SymmetricEncryptionKeyTest.First;

            try
            {
                new KeyExchangeReply(request, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "key", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'key' isn't checked for null inputs");
        }


        [TestMethod]
        public void KeyExchangeReply_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void KeyExchangeReply_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void KeyExchangeReply_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void KeyExchangeReply_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class KeyExchangeRequestTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<KeyExchangeRequest> objects = new ObjectsCache<KeyExchangeRequest>(GetInstances);

        internal static KeyExchangeRequest First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static KeyExchangeRequest Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static KeyExchangeRequest Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        

        [TestMethod]
        public void KeyExchangeRequest_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void KeyExchangeRequest_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void KeyExchangeRequest_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void KeyExchangeRequest_ToStringTest()
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

