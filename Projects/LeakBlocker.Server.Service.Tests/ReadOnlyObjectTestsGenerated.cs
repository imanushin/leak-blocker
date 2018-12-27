
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Cryptography;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Server.Service.InternalTools.AdminUsersStorage;
using LeakBlocker.Server.Service.Tests.InternalTools.AdminUsersStorage;
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


namespace LeakBlocker.Server.Service.Tests.InternalTools.AdminUsersStorage
{

    [TestClass]
    public sealed partial class AdminUserDataTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AdminUserData> objects = new ObjectsCache<AdminUserData>(GetInstances);

        internal static AdminUserData First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AdminUserData Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AdminUserData Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AdminUserData_CheckNullArg_userIdentifier()
        {
            var userIdentifier = AccountSecurityIdentifierTest.First;
            var key = SymmetricEncryptionKeyTest.First;

            try
            {
                new AdminUserData(null, key);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "userIdentifier", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'userIdentifier' isn't checked for null inputs");
        }

        [TestMethod]
        public void AdminUserData_CheckNullArg_key()
        {
            var userIdentifier = AccountSecurityIdentifierTest.First;
            var key = SymmetricEncryptionKeyTest.First;

            try
            {
                new AdminUserData(userIdentifier, null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "key", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'key' isn't checked for null inputs");
        }


        [TestMethod]
        public void AdminUserData_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AdminUserData_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AdminUserData_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AdminUserData_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class AdminUsersTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AdminUsers> objects = new ObjectsCache<AdminUsers>(GetInstances);

        internal static AdminUsers First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AdminUsers Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AdminUsers Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        
        [TestMethod]
        public void AdminUsers_CheckNullArg_users()
        {
            var users = new Dictionary<Int32,AdminUserData>();

            try
            {
                new AdminUsers(null);
            }
            catch(ArgumentNullException ex)
            {
                CheckArgumentExceptionParameter( "users", ex.ParamName );

                return;
            }

            Assert.Fail("Argument 'users' isn't checked for null inputs");
        }


        [TestMethod]
        public void AdminUsers_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AdminUsers_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AdminUsers_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AdminUsers_ToStringTest()
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

