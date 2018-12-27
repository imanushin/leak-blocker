using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Implementations;
using LeakBlocker.Agent.Core.Tests.External;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.SystemTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests
{
    [TestClass]
    public class StackStorageTest : BaseTest
    {
        [TestMethod]
        public void StackStorageTest1()
        {
            IStackStorage target = new StackStorage();
            target.Write(new string[] { "qqqq" }.ToReadOnlySet());
            target.Read(33253646, data => Assert.AreEqual(0, data.Count()));
            target.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StackStorageTest2()
        {
            IStackStorage target = new StackStorage();
            target.Read(0, data => Assert.AreEqual(0, data.Count()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StackStorageTest3()
        {
            IStackStorage target = new StackStorage();
            target.Read(435346, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StackStorageTest4()
        {
            IStackStorage target = new StackStorage();
            target.Write(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StackStorageTest5()
        {
            IStackStorage target = new StackStorage(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StackStorageTest6()
        {
            IStackStorage target = new StackStorage("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StackStorageTest7()
        {
            IStackStorage target = new StackStorage("   ");
        }

        [TestMethod]
        public void StackStorageTest8()
        {
            SystemObjects.Factories.PrivateFile.EnqueueConstructor(file => new PrivateFileImplementation());

            IStackStorage target = new StackStorage("test.txt");
            target.Write(new string[] { "testdata1", "data2" }.ToReadOnlySet());

            target.Read(25, data =>
                {
                    Assert.AreEqual(data.Count(), 1);
                    Assert.AreEqual(data.First(), "data2");
                });

            target.Write(new string[] { "testdata3", "data4" }.ToReadOnlySet());

            target.Read(65536, data =>
            {
                Assert.AreEqual(data.Count(), 3);
                Assert.AreEqual(data.First(), "testdata1");
                Assert.AreEqual(data.Skip(1).First(), "testdata3");
                Assert.AreEqual(data.Skip(2).First(), "data4");
            });

            target.Dispose();
        }

        [TestMethod]
        public void StackStorageTest9()
        {
            SystemObjects.Factories.PrivateFile.EnqueueConstructor(file => new PrivateFileImplementation());

            IStackStorage target = new StackStorage("test.txt");
 
            target.Read(10, data =>
            {
                Assert.AreEqual(data.Count(), 0);
            });

            target.Dispose();
        }

        [TestMethod]
        public void StackStorageTest10()
        {
            SystemObjects.Factories.PrivateFile.EnqueueConstructor(file => new PrivateFileImplementation());

            IStackStorage target = new StackStorage("test.txt");

            target.Write(new string[] { "testdata3", "data4" }.ToReadOnlySet());

            target.Read(65536, data =>
            {
                throw new InvalidOperationException();
            });

            target.Dispose();
        }
    }
}
