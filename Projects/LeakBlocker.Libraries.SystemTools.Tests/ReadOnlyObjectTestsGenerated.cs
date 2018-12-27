
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.SystemTools.Devices.Implementations;
using LeakBlocker.Libraries.SystemTools.Devices.Management;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Libraries.SystemTools.Entities.Implementations;
using LeakBlocker.Libraries.SystemTools.Entities.Management;
using LeakBlocker.Libraries.SystemTools.Tests.Devices.Implementations;
using LeakBlocker.Libraries.SystemTools.Tests.Devices.Management;
using LeakBlocker.Libraries.SystemTools.Tests.Entities;
using LeakBlocker.Libraries.SystemTools.Tests.Entities.Implementations;
using LeakBlocker.Libraries.SystemTools.Tests.Entities.Management;
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


namespace LeakBlocker.Libraries.SystemTools.Tests.Entities.Management
{

    [TestClass]
    public sealed partial class AccountSecurityIdentifierSetTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<AccountSecurityIdentifierSet> objects = new ObjectsCache<AccountSecurityIdentifierSet>(GetInstances);

        internal static AccountSecurityIdentifierSet First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static AccountSecurityIdentifierSet Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static AccountSecurityIdentifierSet Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        

        [TestMethod]
        public void AccountSecurityIdentifierSet_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void AccountSecurityIdentifierSet_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void AccountSecurityIdentifierSet_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void AccountSecurityIdentifierSet_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.Libraries.SystemTools.Tests.Entities
{

    [TestClass]
    public sealed partial class CachedUserDataTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<CachedUserData> objects = new ObjectsCache<CachedUserData>(GetInstances);

        internal static CachedUserData First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static CachedUserData Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static CachedUserData Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        

        [TestMethod]
        public void CachedUserData_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void CachedUserData_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void CachedUserData_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void CachedUserData_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

    [TestClass]
    public sealed partial class CachedComputerDataTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<CachedComputerData> objects = new ObjectsCache<CachedComputerData>(GetInstances);

        internal static CachedComputerData First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static CachedComputerData Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static CachedComputerData Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        

        [TestMethod]
        public void CachedComputerData_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void CachedComputerData_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void CachedComputerData_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void CachedComputerData_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.Libraries.SystemTools.Tests.Devices.Management
{

    [TestClass]
    public sealed partial class DeviceTreeNodeTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DeviceTreeNode> objects = new ObjectsCache<DeviceTreeNode>(GetInstances);

        internal static DeviceTreeNode First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DeviceTreeNode Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DeviceTreeNode Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        

        [TestMethod]
        public void DeviceTreeNode_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DeviceTreeNode_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DeviceTreeNode_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DeviceTreeNode_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.Libraries.SystemTools.Tests.Devices.Implementations
{

    [TestClass]
    public sealed partial class SystemDeviceTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<SystemDevice> objects = new ObjectsCache<SystemDevice>(GetInstances);

        internal static SystemDevice First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static SystemDevice Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static SystemDevice Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        

        [TestMethod]
        public void SystemDevice_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void SystemDevice_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void SystemDevice_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void SystemDevice_ToStringTest()
        {
            BaseToStringTest(objects);
        }
    }

}

namespace LeakBlocker.Libraries.SystemTools.Tests.Entities.Implementations
{

    [TestClass]
    public sealed partial class DomainSnapshotTest : ReadOnlyObjectTest
    {
        internal static readonly ObjectsCache<DomainSnapshot> objects = new ObjectsCache<DomainSnapshot>(GetInstances);

        internal static DomainSnapshot First
        {
            get
            {
                return objects.Objects.First();
            }
        }

        internal static DomainSnapshot Second
        {
            get
            {
                return objects.Objects.Skip(1).First();
            }
        }

        internal static DomainSnapshot Third
        {
            get
            {
                return objects.Objects.Skip(2).First();
            }
        }

        

        [TestMethod]
        public void DomainSnapshot_GetHashCodeTest()
        {
            BaseGetHashCodeTest(objects);
        }

        [TestMethod]
        public void DomainSnapshot_EqualsTest()
        {
            BaseEqualsTest(objects);
        }

        [TestMethod]
        public void DomainSnapshot_SerializationTest()
        {
            BaseSerializationTest(objects);
        }

        [TestMethod]
        public void DomainSnapshot_ToStringTest()
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

