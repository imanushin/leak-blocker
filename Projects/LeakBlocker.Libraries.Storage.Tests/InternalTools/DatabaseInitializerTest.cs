using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Storage.InternalTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Storage.Tests.InternalTools
{
    [TestClass]
    public sealed class DatabaseInitializerTest : BaseTest
    {
        [TestMethod]
        public void DatabasePassword_Short()
        {
            Mocks.ReplayAll();

            var databaseInitializer = new DatabaseInitializer();

            const string password = "Qwerty12";

            databaseInitializer.DatabasePassword = password;

            Assert.AreEqual(password, databaseInitializer.DatabasePassword);
        }

        [TestMethod]
        public void DatabasePassword_Long()
        {
            Mocks.ReplayAll();

            var databaseInitializer = new DatabaseInitializer();

            string password = string.Concat(Enumerable.Range(-50, 100).Select(val => val.ToString()));

            databaseInitializer.DatabasePassword = password;

            Assert.AreNotEqual(password, databaseInitializer.DatabasePassword);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DatabasePassword_DoubleInitializing()
        {
            Mocks.ReplayAll();

            var databaseInitializer = new DatabaseInitializer();

            databaseInitializer.DatabasePassword = "Q";
            databaseInitializer.DatabasePassword = "W";
        }
    }
}
