using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.CommonInterfaces;
using LeakBlocker.Libraries.Common.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests
{
    [TestClass]
    public sealed class ConstantsTest : BaseTest
    {
        [TestMethod]
        public void CheckPropertiesResultsNotNull()
        {
            IConstants target = new Constants();

            Assert.IsNotNull(target.MainModuleFolder);
            Assert.IsNotNull(target.MainModulePath);
            Assert.IsNotNull(target.Version);
            Assert.IsNotNull(target.CurrentProcessImageType);
            Assert.IsNotNull(target.UserDataFolder);
            Assert.IsNotNull(target.CurrentVersionProgramFilesFolder);
            Assert.IsNotNull(target.TemporaryFolder);
            Assert.IsNotNull(target.VersionString);
            Assert.IsTrue(target.MainModuleFolder.EndsWith("\\"));
            Assert.IsTrue(target.UserDataFolder.EndsWith("\\"));
            Assert.IsTrue(target.CurrentVersionProgramFilesFolder.EndsWith("\\"));
            Assert.IsTrue(target.TemporaryFolder.EndsWith("\\"));
            target.Version.ToString(3);
        }
    }
}
