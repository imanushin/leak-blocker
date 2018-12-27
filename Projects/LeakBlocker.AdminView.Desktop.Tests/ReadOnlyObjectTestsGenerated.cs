
using LeakBlocker.AdminView.Desktop.Common;
using LeakBlocker.AdminView.Desktop.Controls.Standard.SettingsChangeActions;
using LeakBlocker.AdminView.Desktop.Tests.Common;
using LeakBlocker.AdminView.Desktop.Tests.Controls.Standard.SettingsChangeActions;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.TemporaryAccess;
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


namespace LeakBlocker.AdminView.Desktop.Tests.Common
{

    [TestClass]
    public sealed partial class UiComputerTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<UiComputer> objects = new ObjectsCache<UiComputer>(GetInstances);

        internal static UiComputer First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static UiComputer Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static UiComputer Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void UiComputer_CheckNullArg_account()
        {
            var account = BaseComputerAccountTest.First;

            try
            {
                new UiComputer(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "account", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'account' isn't checked for null inputs");
        }


        [TestMethod]
        public void UiComputer_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void UiComputer_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void UiComputer_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void UiComputer_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.AdminView.Desktop.Tests.Controls.Standard.SettingsChangeActions
{

    [TestClass]
    public sealed partial class BaseChangeActionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<BaseChangeAction> objects = new ObjectsCache<BaseChangeAction>(GetInstances);

        internal static BaseChangeAction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static BaseChangeAction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static BaseChangeAction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        private static IEnumerable<BaseChangeAction>GetInstances()
        {
            return
            new BaseChangeAction[0].Union(
AddDeviceToWhiteListTest.objects).Union(
AddUserToWhiteListTest.objects).Union(
CancelTemporaryAccessActionTest.objects).Union(
ExcludeComputerActionTest.objects).Union(
GetTemporaryAccessActionTest.objects);
        }

        [TestMethod]
        public void BaseChangeAction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void BaseChangeAction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void BaseChangeAction_SerializationTest()
        {
            BaseSerializationTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AddDeviceToWhiteListTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AddDeviceToWhiteList> objects = new ObjectsCache<AddDeviceToWhiteList>(GetInstances);

        internal static AddDeviceToWhiteList First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AddDeviceToWhiteList Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AddDeviceToWhiteList Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AddDeviceToWhiteList_CheckNullArg_device()
        {
            var device = DeviceDescriptionTest.First;

            try
            {
                new AddDeviceToWhiteList(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "device", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'device' isn't checked for null inputs");
        }


        [TestMethod]
        public void AddDeviceToWhiteList_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AddDeviceToWhiteList_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AddDeviceToWhiteList_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AddDeviceToWhiteList_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AddUserToWhiteListTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AddUserToWhiteList> objects = new ObjectsCache<AddUserToWhiteList>(GetInstances);

        internal static AddUserToWhiteList First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AddUserToWhiteList Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AddUserToWhiteList Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AddUserToWhiteList_CheckNullArg_user()
        {
            var user = BaseUserAccountTest.First;

            try
            {
                new AddUserToWhiteList(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "user", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'user' isn't checked for null inputs");
        }


        [TestMethod]
        public void AddUserToWhiteList_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AddUserToWhiteList_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AddUserToWhiteList_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AddUserToWhiteList_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class CancelTemporaryAccessActionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<CancelTemporaryAccessAction> objects = new ObjectsCache<CancelTemporaryAccessAction>(GetInstances);

        internal static CancelTemporaryAccessAction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static CancelTemporaryAccessAction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static CancelTemporaryAccessAction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void CancelTemporaryAccessAction_CheckNullArg_condition()
        {
            var condition = BaseTemporaryAccessConditionTest.First;

            try
            {
                new CancelTemporaryAccessAction(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "condition", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'condition' isn't checked for null inputs");
        }


        [TestMethod]
        public void CancelTemporaryAccessAction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void CancelTemporaryAccessAction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void CancelTemporaryAccessAction_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void CancelTemporaryAccessAction_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class ExcludeComputerActionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<ExcludeComputerAction> objects = new ObjectsCache<ExcludeComputerAction>(GetInstances);

        internal static ExcludeComputerAction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static ExcludeComputerAction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static ExcludeComputerAction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void ExcludeComputerAction_CheckNullArg_computerToExclude()
        {
            var computerToExclude = BaseComputerAccountTest.First;

            try
            {
                new ExcludeComputerAction(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "computerToExclude", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'computerToExclude' isn't checked for null inputs");
        }


        [TestMethod]
        public void ExcludeComputerAction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void ExcludeComputerAction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void ExcludeComputerAction_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void ExcludeComputerAction_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class GetTemporaryAccessActionTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<GetTemporaryAccessAction> objects = new ObjectsCache<GetTemporaryAccessAction>(GetInstances);

        internal static GetTemporaryAccessAction First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static GetTemporaryAccessAction Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static GetTemporaryAccessAction Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void GetTemporaryAccessAction_CheckNullArg_baseTemporaryAccessCondition()
        {
            var baseTemporaryAccessCondition = BaseTemporaryAccessConditionTest.First;

            try
            {
                new GetTemporaryAccessAction(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "baseTemporaryAccessCondition", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'baseTemporaryAccessCondition' isn't checked for null inputs");
        }


        [TestMethod]
        public void GetTemporaryAccessAction_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void GetTemporaryAccessAction_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void GetTemporaryAccessAction_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void GetTemporaryAccessAction_ToStringTest()
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

