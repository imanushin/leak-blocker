using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests
{
    /// <summary>
    /// Тестирует Assert класс.
    /// Assert устроен так, что каждый метод имеет похожую логику:
    /// 1. return тип всегда void
    /// 2. Если проверка прошла удачно, то ничего не происходит.
    /// 3. Если на входе не-null-овое имя аргумента (argumentName) и проверяемое значение == null, то бросается ArgumentNullException
    /// 4. Если на входе прочие неверные данные, и имя аргумента опять непустое, то бросается ArgumentException
    /// 5. Во всех остальных случаях бросается InvalidOperationException.
    /// 
    /// Соответсвенно, этот класс свернут похожим образом. На входе мы всегда имеем тестируемую функцию,
    /// в которой набор один аргумент. Единственное исключение их правила сделано в виде копи-паста.
    /// Если в дальнейшем появятся более сложные Assert'ы, с несколькими аргументами, то тестовые функции следует так же расширить,
    /// то есть для каждого количества аргументов - отдельный набор. Желательно их тогда выделять в отдельные классы
    /// 
    /// Все эти операции обобщены с помощью Generic-ов, главные тесты просто запускают сразу несколько скрытых дочерних.
    /// </summary>
    [TestClass]
    public sealed class CheckTest : BaseTest
    {
        [TestMethod]
        public void ObjectIsNotNull()
        {
            CheckForNullAndWrongInput(new object(), Check.ObjectIsNotNull<object>);
        }

        [TestMethod]
        public void StringIsNotNullOrEmpty()
        {
            CheckForNullAndWrongInput("123", Check.StringIsNotNullOrEmpty, string.Empty);
        }

        [TestMethod]
        public void StringIsMeaningful()
        {
            CheckForNullAndWrongInput("123", Check.StringIsMeaningful, string.Empty, "     ");
        }

        [TestMethod]
        public void IntegerIsNotLessThanZero()
        {
            CheckForWrongInput(5, Check.IntegerIsNotLessThanZero, -1, int.MinValue);
            CheckForWrongInput(0, Check.IntegerIsNotLessThanZero);
        }

        [TestMethod]
        public void IntegerIsGreaterThanZero()
        {
            CheckForWrongInput(5, Check.IntegerIsGreaterThanZero, 0, -1, int.MinValue);
        }

        [TestMethod]
        public void EnumerationValueIsCorrect1()
        {
            CheckForWrongInput(TestEnum.Allowed,
                Check.EnumerationValueIsDefined<TestEnum>,
                (TestEnum)int.MinValue,
                TestEnum.None
                );
        }

        [TestMethod]
        public void EnumerationValueIsCorrect2()
        {
            Check.EnumerationValueIsDefined(TestEnum.NewValue);
            Check.EnumerationValueIsDefined(TestEnum.OldValue); //Because in core it equals then new value
        }

        [TestMethod]
        public void CollectionIsNotNullOrEmpty()
        {
            CheckForNullAndWrongInput<int[]>(new int[1], Check.CollectionIsNotNullOrEmpty, new int[0]);
        }

        [TestMethod]
        public void CollectionHasNoDefaultItems()
        {
            CheckForNullAndWrongInput<object[]>(new[] { new object() }, Check.CollectionHasNoDefaultItems, new object[1]);
            CheckForNullAndWrongInput(new object[0], Check.CollectionHasNoDefaultItems);
        }

        [TestMethod]
        public void CollectionHasMeaningfulData()
        {
            CheckForNullAndWrongInput(new[] { new object() }, Check.CollectionHasOnlyMeaningfulData, new object[1], new object[0]);
        }

        [TestMethod]
        public void IsEnumeration()
        {
            CheckForNullAndWrongInput(typeof(LoaderOptimization), Check.IsEnumeration, typeof(string), typeof(TestStruct));
        }

        [TestMethod]
        public void Cast_Success()
        {
            var expectedImplementation = new TestImplementation();
            ITestInterface testInterface = expectedImplementation;

            TestImplementation impl1 = Check.Cast<ITestInterface, TestImplementation>(testInterface);

            Assert.AreEqual(expectedImplementation, impl1);

            var impl2 = Check.Cast<TestImplementation>(testInterface);

            Assert.AreEqual(expectedImplementation, impl2);
        }

        [TestMethod]
        public void Cast()
        {
            var expectedImplementation = new TestImplementation();
            ITestInterface testInterface = expectedImplementation;

            CheckForWrongInput(
                testInterface,
                (input, paramName) => Check.Cast<ITestInterface, TestImplementation>(input, paramName));

            CheckForWrongInput(
                (object)testInterface,
                (input, paramName) => Check.Cast<TestImplementation>(input, paramName),
                "string");
        }

        #region IsEnumeration<T>

        [TestMethod]
        public void IsEnumeration_Good()
        {
            Check.IsEnumeration<AttributeTargets>(AttributeTargets.All);
            Check.IsEnumeration<AttributeTargets>(AttributeTargets.All, "param1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsEnumeration_Bad_Argument()
        {
            Check.IsEnumeration<TestStruct>(new TestStruct(), "param1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsEnumeration_Bad_Operation()
        {
            Check.IsEnumeration<TestStruct>(new TestStruct());
        }

        #endregion IsEnumeration<T>

        #region CheckArrayArguments

        [TestMethod]
        public void CheckArrayArguments_Good()
        {
            Check.CheckArrayArguments<int>(new List<int>()
            {1,2,3
            }, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckArrayArguments_Bad_Null_Argument()
        {
            Check.CheckArrayArguments<int>(null, 0, "param1");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckArrayArguments_Bad_Wrong_Argument()
        {
            Check.CheckArrayArguments<int>(new List<int>(), 1, "param1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CheckArrayArguments_Bad_Null_Operation()
        {
            Check.CheckArrayArguments<int>(null, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CheckArrayArguments_Bad_Wrong_Operation()
        {
            Check.CheckArrayArguments<int>(new List<int>(), 1);
        }

        #endregion CheckArrayArguments

        #region Generic Helpers

        private static void CheckForNullAndWrongInput<Tin>(Tin goodArgument, Action<Tin, string> testFunction, params Tin[] wrongArguments)
        where Tin : class
        {
            testFunction(goodArgument, null);
            testFunction(goodArgument, "param1");

            CheckForNull(testFunction);

            foreach (Tin argument in wrongArguments)
            {
                CheckExceptions(testFunction, argument, typeof(ArgumentException));
            }

        }

        private static void CheckForWrongInput<Tin>(Tin goodArgument, Action<Tin, string> testFunction, params Tin[] wrongArguments)
        {
            testFunction(goodArgument, null);
            testFunction(goodArgument, "param1");

            foreach (Tin argument in wrongArguments)
            {
                CheckExceptions(testFunction, argument, typeof(ArgumentException));
            }
        }

        private static void CheckForNull<Tin>(Action<Tin, string> testFunction)
            where Tin : class
        {
            CheckExceptions(testFunction, (Tin)null, typeof(ArgumentNullException));
        }

        private static void CheckExceptions<Tin>(Action<Tin, string> testFunction, Tin input, Type argumentExceptionType)
        {
            try
            {
                testFunction(input, "param1");
            }
            catch (Exception ex)
            {
               Assert.IsInstanceOfType(ex, argumentExceptionType);
            }

            try
            {
                testFunction(input, null);
            }
            catch (Exception ex)
            {
               Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
        }

        #endregion Generic Helpers

        #region Helpers

        private struct TestStruct : IComparable, IFormattable, IConvertible
        {
            public TypeCode GetTypeCode()
            {
                throw new NotImplementedException();
            }

            public bool ToBoolean(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public byte ToByte(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public char ToChar(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public DateTime ToDateTime(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public decimal ToDecimal(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public double ToDouble(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public short ToInt16(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public int ToInt32(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public long ToInt64(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public sbyte ToSByte(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public float ToSingle(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public string ToString(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public object ToType(Type conversionType, IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public ushort ToUInt16(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public uint ToUInt32(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public ulong ToUInt64(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                throw new NotImplementedException();
            }

            public int CompareTo(object target)
            {
                throw new NotImplementedException();
            }
        }

        private enum TestEnum
        {
            [ForbiddenToUse]
            None = 0,

            Allowed,

            [ForbiddenToUse]
            OldValue = NewValue,

            NewValue = 2
        }

        private interface ITestInterface
        {
        }

        private sealed class TestImplementation : ITestInterface
        {
        }

        #endregion Helpers
    }
}
