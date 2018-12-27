using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities;


namespace LeakBlocker.Libraries.Common.Tests.Entities.Audit
{

    [TestClass]
    public sealed class AuditItemGroupTypeTest
    {
        [TestMethod]
        public void AuditItemGroupType_CheckResources()
        {
            foreach (AuditItemGroupType item in EnumHelper<AuditItemGroupType>.Values)
            {
                string description = item.GetValueDescription();

                Assert.IsNotNull(description);
            }
        }
    }

    [TestClass]
    public sealed class AuditItemSeverityTypeTest
    {
        [TestMethod]
        public void AuditItemSeverityType_CheckResources()
        {
            foreach (AuditItemSeverityType item in EnumHelper<AuditItemSeverityType>.Values)
            {
                string description = item.GetValueDescription();

                Assert.IsNotNull(description);
            }
        }
    }

    [TestClass]
    public sealed class AuditItemTypeTest
    {
        [TestMethod]
        public void AuditItemType_CheckResources()
        {
            foreach (AuditItemType item in EnumHelper<AuditItemType>.Values)
            {
                string description = item.GetValueDescription();

                Assert.IsNotNull(description);
            }
        }
    }

}

namespace LeakBlocker.Libraries.Common.Tests.Entities
{

    [TestClass]
    public sealed class DeviceCategoryTest
    {
        [TestMethod]
        public void DeviceCategory_CheckResources()
        {
            foreach (DeviceCategory item in EnumHelper<DeviceCategory>.Values)
            {
                string description = item.GetValueDescription();

                Assert.IsNotNull(description);
            }
        }
    }

}
