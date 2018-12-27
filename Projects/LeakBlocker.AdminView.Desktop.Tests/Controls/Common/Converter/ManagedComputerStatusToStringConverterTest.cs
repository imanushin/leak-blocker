using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Controls.Common.Converters;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.AdminView.Desktop.Tests.Controls.Common.Converter
{
    [TestClass]
    public sealed class ManagedComputerStatusToStringConverterTest : BaseEnumToStringConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            BaseConvertTest<ManagedComputerStatus, ManagedComputerStatusToStringConverter>();
        }
    }
}
