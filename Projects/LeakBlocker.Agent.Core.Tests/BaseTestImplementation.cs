using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Equality;
using LeakBlocker.Libraries.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests
{
    internal abstract class BaseTestImplementation : BaseTest
    {
        private struct ExpectedMethodCall
        {
            public string Name;
            public object Argument1;
            public object Argument2;
            public object Argument3;
            public object Argument4;
            public object Argument5;
            public object Argument6;
            public object Argument7;
            public object Argument8;
            public object Argument9;

            public override bool Equals(object obj)
            {
                var target = (ExpectedMethodCall)obj;
                return (Name == target.Name) &&
                    CheckEquality(Argument1, target.Argument1) &&
                    CheckEquality(Argument2, target.Argument2) &&
                    CheckEquality(Argument3, target.Argument3) &&
                    CheckEquality(Argument4, target.Argument4) &&
                    CheckEquality(Argument5, target.Argument5) &&
                    CheckEquality(Argument6, target.Argument6) &&
                    CheckEquality(Argument7, target.Argument7) &&
                    CheckEquality(Argument8, target.Argument8) &&
                    CheckEquality(Argument9, target.Argument9);
            }

            private static bool CheckEquality(object arg1, object arg2)
            {
                if (arg1 is IList)
                    return EnumerableComparer.Compare(((IList)arg1).Cast<object>().ToList(), ((IList)arg2).Cast<object>().ToList());

                if (arg1 is IDictionary)
                    return EnumerableComparer.Compare((IDictionary)arg1, (IDictionary)arg2);

                if (arg1 is IEnumerable)
                    return EnumerableComparer.Compare(((IEnumerable)arg1).Cast<object>(), ((IEnumerable)arg2).Cast<object>());

                return Equals(arg1, arg2);
            }

            private static int CalculateHashCode(object arg)
            {
                if (arg == null)
                    return 0;

                if (arg is IList)
                    return EnumerableHashCode.GetHashCode(((IList)arg).Cast<object>().ToList());

                if (arg is IDictionary)
                    return EnumerableHashCode.GetHashCode((IDictionary)arg);

                if (arg is IEnumerable)
                    return EnumerableHashCode.GetHashCode(((IEnumerable)arg).Cast<object>());

                return arg.GetHashCode();
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode() ^
                    CalculateHashCode(Argument1) ^
                    CalculateHashCode(Argument2) ^
                    CalculateHashCode(Argument3) ^
                    CalculateHashCode(Argument4) ^
                    CalculateHashCode(Argument5) ^
                    CalculateHashCode(Argument6) ^
                    CalculateHashCode(Argument7) ^
                    CalculateHashCode(Argument8) ^
                    CalculateHashCode(Argument9);
            }

            public static bool operator ==(ExpectedMethodCall arg1, ExpectedMethodCall arg2)
            {
                return Equals(arg1, arg2);
            }

            public static bool operator !=(ExpectedMethodCall arg1, ExpectedMethodCall arg2)
            {
                return !Equals(arg1, arg2);
            }
        }

        private readonly List<ExpectedMethodCall> expected = new List<ExpectedMethodCall>();
        private readonly List<ExpectedMethodCall> calls = new List<ExpectedMethodCall>();

        protected void RegisterMethodCall(string name, object argument1 = null, object argument2 = null, object argument3 = null, object argument4 = null,
            object argument5 = null, object argument6 = null, object argument7 = null, object argument8 = null, object argument9 = null)
        {
            calls.Add(new ExpectedMethodCall
            {
                Name = name,
                Argument1 = argument1,
                Argument2 = argument2,
                Argument3 = argument3,
                Argument4 = argument4,
                Argument5 = argument5,
                Argument6 = argument6,
                Argument7 = argument7,
                Argument8 = argument8,
                Argument9 = argument9
            });
        }

        public void AddExpectedMethodCall(string name, object argument1 = null, object argument2 = null, object argument3 = null, object argument4 = null,
            object argument5 = null, object argument6 = null, object argument7 = null, object argument8 = null, object argument9 = null)
        {
            expected.Add(new ExpectedMethodCall
            {
                Name = name,
                Argument1 = argument1,
                Argument2 = argument2,
                Argument3 = argument3,
                Argument4 = argument4,
                Argument5 = argument5,
                Argument6 = argument6,
                Argument7 = argument7,
                Argument8 = argument8,
                Argument9 = argument9
            });
        }

        public void Validate(bool ordered = false)
        {
            if (ordered)
                Assert.IsTrue(EnumerableComparer.Compare(expected, calls), "Unexpected behaviour.");
            else
                Assert.IsTrue(EnumerableComparer.Compare((IEnumerable<ExpectedMethodCall>)expected, (IEnumerable<ExpectedMethodCall>)calls), "Unexpected behaviour.");

            expected.Clear();
            calls.Clear();
        }
    }
}
