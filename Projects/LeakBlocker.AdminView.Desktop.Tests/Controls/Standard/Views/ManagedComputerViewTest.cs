using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.AdminView.Desktop.Tests.Controls.Standard.Views
{
    [TestClass]
    public sealed class ManagedComputerViewTest
    {
        [TestMethod]
        public void StrongComputers()
        {
            foreach (BaseComputerAccount computer in BaseComputerAccountTest.objects)
            {
                foreach (ManagedComputerData data in ManagedComputerDataTest.objects)
                {
                    var target = new ManagedComputer(computer, data);

                    Assert.IsNotNull(target.DnsName);
                    Assert.IsNotNull(target.ToString());
                }
            }
        }

    }
}
