
using LeakBlocker.Agent.Core.Implementations.AgentDataStorageObjects;
using LeakBlocker.Agent.Core.Settings;
using LeakBlocker.Agent.Core.Tests.Implementations.AgentDataStorageObjects;
using LeakBlocker.Agent.Core.Tests.Settings;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Libraries.SystemTools.Tests.Entities;
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


namespace LeakBlocker.Agent.Core.Tests.Implementations.AgentDataStorageObjects
{

    [TestClass]
    public sealed partial class DataDiskCacheTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DataDiskCache> objects = new ObjectsCache<DataDiskCache>(GetInstances);

        internal static DataDiskCache First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DataDiskCache Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DataDiskCache Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DataDiskCache_CheckNullArg_users()
        {
            var users = new List<CachedUserData>{ CachedUserDataTest.First }.ToReadOnlyList();
            var deviceStates = new List<DeviceState>{ DeviceStateTest.First }.ToReadOnlyList();
            var settings = ProgramConfigurationTest.First;
            var computer = CachedComputerDataTest.First;
            var consoleUser = BaseUserAccountTest.First;

            try
            {
                new DataDiskCache(null, deviceStates, settings, computer, consoleUser);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "users", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'users' isn't checked for null inputs");
        }

        [TestMethod]
        public void DataDiskCache_CheckNullArg_deviceStates()
        {
            var users = new List<CachedUserData>{ CachedUserDataTest.First }.ToReadOnlyList();
            var deviceStates = new List<DeviceState>{ DeviceStateTest.First }.ToReadOnlyList();
            var settings = ProgramConfigurationTest.First;
            var computer = CachedComputerDataTest.First;
            var consoleUser = BaseUserAccountTest.First;

            try
            {
                new DataDiskCache(users, null, settings, computer, consoleUser);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "deviceStates", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'deviceStates' isn't checked for null inputs");
        }


        [TestMethod]
        public void DataDiskCache_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DataDiskCache_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DataDiskCache_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DataDiskCache_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class DeviceStateTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DeviceState> objects = new ObjectsCache<DeviceState>(GetInstances);

        internal static DeviceState First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DeviceState Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DeviceState Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void DeviceState_CheckNullArg_device()
        {
            var device = DeviceDescriptionTest.First;
            var state = EnumHelper<DeviceAccessType>.Values.First();

            try
            {
                new DeviceState(null, state);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "device", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'device' isn't checked for null inputs");
        }

        [TestMethod]
        public void DeviceState_CheckWrongEnumArg_state()
        {
            var device = DeviceDescriptionTest.First;
            var state = EnumHelper<DeviceAccessType>.Values.First();

            try
            {
                new DeviceState(device, (LeakBlocker.Libraries.Common.Entities.DeviceAccessType)7);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "state", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'state' isn't checked for the not-defined values");
        }


        [TestMethod]
        public void DeviceState_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DeviceState_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DeviceState_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DeviceState_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.Agent.Core.Tests.Settings
{

    [TestClass]
    public sealed partial class AgentComputerStateTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AgentComputerState> objects = new ObjectsCache<AgentComputerState>(GetInstances);

        internal static AgentComputerState First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AgentComputerState Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AgentComputerState Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AgentComputerState_CheckNullArg_targetComputer()
        {
            var targetComputer = BaseComputerAccountTest.First;
            var computerGroups = new List<AccountSecurityIdentifier>{ AccountSecurityIdentifierTest.First }.ToReadOnlyList();
            var loggedOnUsers = new Dictionary<BaseUserAccount,ReadOnlySet<AccountSecurityIdentifier>>().ToReadOnlyDictionary();
            var connectedDevices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();

            try
            {
                new AgentComputerState(null, computerGroups, loggedOnUsers, connectedDevices);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "targetComputer", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'targetComputer' isn't checked for null inputs");
        }

        [TestMethod]
        public void AgentComputerState_CheckNullArg_computerGroups()
        {
            var targetComputer = BaseComputerAccountTest.First;
            var computerGroups = new List<AccountSecurityIdentifier>{ AccountSecurityIdentifierTest.First }.ToReadOnlyList();
            var loggedOnUsers = new Dictionary<BaseUserAccount,ReadOnlySet<AccountSecurityIdentifier>>().ToReadOnlyDictionary();
            var connectedDevices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();

            try
            {
                new AgentComputerState(targetComputer, null, loggedOnUsers, connectedDevices);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "computerGroups", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'computerGroups' isn't checked for null inputs");
        }

        [TestMethod]
        public void AgentComputerState_CheckNullArg_loggedOnUsers()
        {
            var targetComputer = BaseComputerAccountTest.First;
            var computerGroups = new List<AccountSecurityIdentifier>{ AccountSecurityIdentifierTest.First }.ToReadOnlyList();
            var loggedOnUsers = new Dictionary<BaseUserAccount,ReadOnlySet<AccountSecurityIdentifier>>().ToReadOnlyDictionary();
            var connectedDevices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();

            try
            {
                new AgentComputerState(targetComputer, computerGroups, null, connectedDevices);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "loggedOnUsers", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'loggedOnUsers' isn't checked for null inputs");
        }

        [TestMethod]
        public void AgentComputerState_CheckNullArg_connectedDevices()
        {
            var targetComputer = BaseComputerAccountTest.First;
            var computerGroups = new List<AccountSecurityIdentifier>{ AccountSecurityIdentifierTest.First }.ToReadOnlyList();
            var loggedOnUsers = new Dictionary<BaseUserAccount,ReadOnlySet<AccountSecurityIdentifier>>().ToReadOnlyDictionary();
            var connectedDevices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();

            try
            {
                new AgentComputerState(targetComputer, computerGroups, loggedOnUsers, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "connectedDevices", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'connectedDevices' isn't checked for null inputs");
        }


        [TestMethod]
        public void AgentComputerState_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AgentComputerState_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AgentComputerState_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AgentComputerState_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class CommonActionDataTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<CommonActionData> objects = new ObjectsCache<CommonActionData>(GetInstances);

        internal static CommonActionData First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static CommonActionData Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static CommonActionData Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void CommonActionData_CheckNullArg_directData_135346()
        {
            var directData = ActionDataTest.First;

            try
            {
                new CommonActionData((ActionData)null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "directData", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'directData' isn't checked for null inputs");
        }

        [TestMethod]
        public void CommonActionData_CheckWrongEnumArg_access()
        {
            var access = EnumHelper<DeviceAccessType>.Values.First();
            var audit = EnumHelper<AuditActionType>.Values.First();

            try
            {
                new CommonActionData((LeakBlocker.Libraries.Common.Entities.DeviceAccessType)7, audit);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "access", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'access' isn't checked for the not-defined values");
        }

        [TestMethod]
        public void CommonActionData_CheckWrongEnumArg_audit()
        {
            var access = EnumHelper<DeviceAccessType>.Values.First();
            var audit = EnumHelper<AuditActionType>.Values.First();

            try
            {
                new CommonActionData(access, (LeakBlocker.Libraries.Common.Entities.Settings.Rules.AuditActionType)5);
            }
            catch(ArgumentException ex)
            {
                CheckArgumentExceptionParameter( "audit", ex.ParamName );

                return;
            }

            Assert.Fail("Enumeration in the argument 'audit' isn't checked for the not-defined values");
        }


        [TestMethod]
        public void CommonActionData_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void CommonActionData_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void CommonActionData_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void CommonActionData_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class RuleCheckResultTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<RuleCheckResult> objects = new ObjectsCache<RuleCheckResult>(GetInstances);

        internal static RuleCheckResult First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static RuleCheckResult Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static RuleCheckResult Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void RuleCheckResult_CheckNullArg_users()
        {
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var creator = new Func<BaseUserAccount, DeviceDescription, CommonActionData>((arg1, arg2)=>default(CommonActionData));

            try
            {
                new RuleCheckResult(null, devices, creator);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "users", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'users' isn't checked for null inputs");
        }

        [TestMethod]
        public void RuleCheckResult_CheckNullArg_devices()
        {
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var creator = new Func<BaseUserAccount, DeviceDescription, CommonActionData>((arg1, arg2)=>default(CommonActionData));

            try
            {
                new RuleCheckResult(users, null, creator);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "devices", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'devices' isn't checked for null inputs");
        }

        [TestMethod]
        public void RuleCheckResult_CheckNullArg_creator()
        {
            var users = new List<BaseUserAccount>{ BaseUserAccountTest.First }.ToReadOnlyList();
            var devices = new List<DeviceDescription>{ DeviceDescriptionTest.First }.ToReadOnlyList();
            var creator = new Func<BaseUserAccount, DeviceDescription, CommonActionData>((arg1, arg2)=>default(CommonActionData));

            try
            {
                new RuleCheckResult(users, devices, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "creator", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'creator' isn't checked for null inputs");
        }


        [TestMethod]
        public void RuleCheckResult_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void RuleCheckResult_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void RuleCheckResult_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void RuleCheckResult_ToStringTest()
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

