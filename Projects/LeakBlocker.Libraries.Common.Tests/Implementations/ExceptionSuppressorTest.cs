using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Implementations
{
    [TestClass]
    public sealed class ExceptionSuppressorTest : BaseTest
    {
        private ExceptionSuppressor target;

        [TestInitialize]
        public void Init()
        {
            target = new ExceptionSuppressor();
        }

        [TestMethod]
        public void NoArgsSuccess()
        {
            Mocks.ReplayAll();

            bool value = false;

            target.Run(
                () =>
                {
                    value = true;
                });

            Assert.IsTrue(value);
        }

        [TestMethod]
        public void OneArgsSuccess()
        {
            Mocks.ReplayAll();

            bool value = false;

            target.Run(
                arg =>
                {
                    Assert.IsNotNull(arg);
                    value = true;
                }, "123");

            Assert.IsTrue(value);
        }

        [TestMethod]
        public void TwoArgsSuccess()
        {
            Mocks.ReplayAll();

            bool value = false;

            target.Run(
                (arg1, arg2) =>
                {
                    Assert.IsNotNull(arg1);
                    Assert.IsNotNull(arg2);

                    value = true;
                }, "123", "321");
            Assert.IsTrue(value);
        }

        [TestMethod]
        public void ThreeArgsSuccess()
        {
            Mocks.ReplayAll();

            bool value = false;

            target.Run(
                (arg1, arg2, arg3) =>
                {
                    Assert.IsNotNull(arg1);
                    Assert.IsNotNull(arg2);
                    Assert.IsNotNull(arg3);

                    value = true;
                }, "123", "321", "123");

            Assert.IsTrue(value);
        }

        [TestMethod]
        public void NoArgFailed()
        {
            Mocks.ReplayAll();

            bool value = false;

            target.Run(
                () =>
                {
                    value = true;

                    throw new Exception();
                });

            Assert.IsTrue(value);
        }

        [TestMethod]
        public void OneArgFailed()
        {
            Mocks.ReplayAll();

            bool value = false;

            target.Run(
                (arg1) =>
                {
                    Assert.IsNotNull(arg1);

                    value = true;

                    throw new Exception();
                }, "1");

            Assert.IsTrue(value);
        }

        [TestMethod]
        public void TwoArgsFailed()
        {
            Mocks.ReplayAll();

            bool value = false;
            
            target.Run(
                (arg1, arg2) =>
                {
                    Assert.IsNotNull(arg1);
                    Assert.IsNotNull(arg2);

                    value = true;

                    throw new Exception();
                }, "1", "2");

            Assert.IsTrue(value);
        }

        [TestMethod]
        public void ThreeArgsFailed()
        {
            Mocks.ReplayAll();
            
            bool value = false;

            target.Run(
                (arg1, arg2, arg3) =>
                {
                    Assert.IsNotNull(arg1);
                    Assert.IsNotNull(arg2);
                    Assert.IsNotNull(arg3);

                    value = true;

                    throw new Exception();
                }, "1", "2", "3");

            Assert.IsTrue(value);
        }
    }
}
