using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.CommonInterfaces;
using LeakBlocker.Libraries.Common.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Libraries.Common.Tests
{
    [TestClass]
    public sealed class CommandLineTest : BaseTest
    {
        private IEnvironment environment;
        private CommandLine target;

        [TestInitialize]
        public void Initialize()
        {
            environment = Mocks.StrictMock<IEnvironment>();

            SharedObjects.Singletons.Environment.SetTestImplementation(environment);

            target = new CommandLine();
        }

        #region Contains

        [TestMethod]
        public void Contains_Strong()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string> { "param1", "param2" }.ToReadOnlyList());

            Mocks.ReplayAll();

            bool result = target.Contains("param1");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_DiffenetCases()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string> { "param1", "param2" }.ToReadOnlyList());

            Mocks.ReplayAll();

            bool result = target.Contains("PARAM1");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_DoesntContains()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string> { "param1", "param2" }.ToReadOnlyList());

            Mocks.ReplayAll();

            bool result = target.Contains("param3");
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void Contains_DoesntContains_NullChecks()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string> { "param1", "param2" }.ToReadOnlyList());

            Mocks.ReplayAll();

            bool result = target.Contains("param3");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contains_NullArgsChecks()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string> { "param1", "param2" }.ToReadOnlyList());

            Mocks.ReplayAll();

            CheckFirstForNull<string>(str => target.Contains(str));
        }

        #endregion Contains

        #region Get Value

        [TestMethod]
        public void GetValue_StrongKey_GetValue()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string> { "param1", "value1" }.ToReadOnlyList());

            Mocks.ReplayAll();

            string result = target.GetValue("param1");

            Assert.AreEqual("value1", result);
        }

        [TestMethod]
        public void GetValue_OtherCaseKey_GetValue()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string> { "param1", "value1" }.ToReadOnlyList());

            Mocks.ReplayAll();

            string result = target.GetValue("PARAM1");

            Assert.AreEqual("value1", result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetValue_NoParameters()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string>().ToReadOnlyList());

            Mocks.ReplayAll();

            target.GetValue("param2");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetValue_OutOfRangeCheck()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string> { "param1", "param2" }.ToReadOnlyList());

            Mocks.ReplayAll();

            target.GetValue("param2");
        }

        [TestMethod]
        public void GetValue_NullArgsCheck()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string> { "param1", "param2" }.ToReadOnlyList());

            Mocks.ReplayAll();

            CheckFirstStringForForMeaningful((str) => target.GetValue(str));
        }

        #endregion

        #region Has Value

        [TestMethod]
        public void HasValue_StrongKey_GetValue()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string> { "param1", "param2" }.ToReadOnlyList());

            Mocks.ReplayAll();

            bool result = target.HasValue("param1");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasValue_OtherCaseKey_GetValue()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string> { "param1", "param2" }.ToReadOnlyList());

            Mocks.ReplayAll();

            bool result = target.HasValue("PARAM1");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasValue_NoParameters()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string> { "param1", "param2" }.ToReadOnlyList());

            Mocks.ReplayAll();

            bool result = target.HasValue("param2");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasValue_OutOfRangeCheck()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string> { "param1", "param2" }.ToReadOnlyList());

            Mocks.ReplayAll();

            bool result = target.HasValue("param2");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasValue_NullArgsCheck()
        {
            environment.Stub(x => x.CommandLineArguments).Return(new List<string> { "param1", "param2" }.ToReadOnlyList());

            Mocks.ReplayAll();

            CheckFirstForNull<string>(str => target.HasValue(str));
        }

        #endregion

        #region Create

        [TestMethod]
        public void Create_NoArgs()
        {
            const string exe = "ie.exe";

            string commandLine = target.Create(exe);

            StringAssert.Contains(commandLine, exe);
        }

        [TestMethod]
        public void Create_SimpleArgs()
        {
            const string exe = "ie.exe";
            const string arg = "arg1";

            string commandLine = target.Create(exe, arg);

            StringAssert.Contains(commandLine, exe);
            StringAssert.Contains(commandLine, arg);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_NullArgs()
        {
            const string exe = "ie.exe";
            const string arg = "arg1";

            target.Create(exe, arg, null);
        }

        [TestMethod]
        public void Create_NullChecks()
        {
            CheckFirstStringForForMeaningful(str => target.Create(str));
            CheckSecondForNull<string, string[]>("exe", (str, args) => target.Create(str, args));
        }

        #endregion Create

        #region Create arguments

        [TestMethod]
        public void CreateArguments_Success()
        {
            const string arg1 = "arg1";
            const string arg2 = "arg2";

            string result = target.CreateArguments(arg1, arg2);

            StringAssert.Contains(result, arg1);
            StringAssert.Contains(result, arg2);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateArgument_NullArgs()
        {
            const string arg = "arg1";

            target.CreateArguments(arg, null);
        }

        [TestMethod]
        public void CreateArguments_NullChecks()
        {
            CheckFirstForNull<string[]>(args => target.CreateArguments(args));
        }


        #endregion Create arguments

        #region Split

        [TestMethod]
        public void Split_OnlyExe()
        {
            const string originalString = "c:\\Windows\\notepad.exe";

            ReadOnlyList<string> result = target.Split(originalString);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(originalString, result.First());
        }

        [TestMethod]
        public void Split_OneParameter()
        {
            const string originalString = "c:\\Windows\\notepad.exe /remote";

            ReadOnlyList<string> result = target.Split(originalString);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("c:\\Windows\\notepad.exe", result.First());
            Assert.AreEqual("/remote", result.Skip(1).First());
        }

        [TestMethod]
        public void Split_TwoParameters()
        {
            const string originalString = "c:\\Windows\\notepad.exe /remote abc";

            ReadOnlyList<string> result = target.Split(originalString);

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("c:\\Windows\\notepad.exe", result.First());
            Assert.AreEqual("/remote", result.Skip(1).First());
            Assert.AreEqual("abc", result.Skip(2).First());
        }

        #endregion Split
    }
}
