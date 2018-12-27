using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Performs various debugging checks.
    /// </summary>
    public static class Check
    {
        private static void ThrowException([NotNull] string message, string argumentName = "")
        {
            if (string.IsNullOrWhiteSpace(argumentName))
                throw new InvalidOperationException(message);

            throw new ArgumentException(message, argumentName);
        }

        /// <summary>
        /// Checks the specified argument. If argument is null then exception is thrown.
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="argumentName">Argument name.</param>
        [AssertionMethod]
        [ContractAnnotation("argument:null => halt")]
        public static void ObjectIsNotNull<T>(T argument, [InvokerParameterName] string argumentName = null)
        {
            if (argument != null)
                return;

            if (argumentName == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Object of type {0} should not be null.", typeof(T)));

            throw new ArgumentNullException(argumentName);
        }

        /// <summary>
        /// Checks the specified string argument. If argument is null or empty string then exception is thrown.
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void StringIsNotNullOrEmpty(string argument, [InvokerParameterName] string argumentName = null)
        {
            ObjectIsNotNull(argument, argumentName);

            if (string.IsNullOrEmpty(argument))
                ThrowException("String is empty.", argumentName);
        }

        /// <summary>
        /// Checks the specified string argument. If string is  is null or empty string then exception is thrown.
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="argumentName">Argument name. For internal checking please ignore this parameter</param>
        public static void StringIsMeaningful(string argument, [InvokerParameterName] string argumentName = null)
        {
            ObjectIsNotNull(argument, argumentName);

            if (string.IsNullOrWhiteSpace(argument))
                ThrowException("String is empty.", argumentName);
        }

        /// <summary>
        /// Checks that input parameter is more or equal zero.
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void IntegerIsNotLessThanZero(int argument, [InvokerParameterName] string argumentName = null)
        {
            if (argument < 0)
                ThrowException("Value should be greater than or equal to zero.", argumentName);
        }

        /// <summary>
        /// Checks that input parameter strongly more than zero
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void IntegerIsGreaterThanZero(int argument, [InvokerParameterName] string argumentName = null)
        {
            if (argument <= 0)
                ThrowException("Value should be greater then zero.", argumentName);
        }

        /// <summary>
        /// Checks that input parameter strongly more than zero
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void IntegerIsGreaterThanZero(uint argument, [InvokerParameterName] string argumentName = null)
        {
            if (argument == 0)
                ThrowException("Value should be greater then zero.", argumentName);
        }

        /// <summary>
        /// Checks that input parameter strongly more than zero
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void IntegerIsGreaterThanZero(long argument, [InvokerParameterName] string argumentName = null)
        {
            if (argument <= 0)
                ThrowException("Value should be greater then zero.", argumentName);
        }

        /// <summary>
        /// Checks that input parameter is not null
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void PointerIsNotNull(IntPtr argument, [InvokerParameterName] string argumentName = null)
        {
            if (argument == IntPtr.Zero)
                ThrowException("Value should be not zero.", argumentName);
        }

        /// <summary>
        /// Checks that collections is not null and is not empty
        /// </summary>
        /// <typeparam name="T">Type of generic collection</typeparam>
        /// <param name="argument">Argument.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void CollectionIsNotNullOrEmpty<T>(IEnumerable<T> argument, [InvokerParameterName] string argumentName = null)
        {
            ObjectIsNotNull(argument, argumentName);

            if (!argument.Any())
                ThrowException("Collection is empty.", argumentName);
        }

        /// <summary>
        /// Checks if collection contains elements and they are not default.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="argument">Argument.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void CollectionHasOnlyMeaningfulData<T>(IEnumerable<T> argument, [InvokerParameterName] string argumentName = null)
            where T : class
        {
            CollectionIsNotNullOrEmpty(argument, argumentName);

            if (argument.Any(currentItem => Equals(default(T), currentItem)))
                ThrowException("Collection contains invalid elements.", argumentName);
        }

        /// <summary>
        /// Checks if elements in the are not default.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="argument">Argument.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void CollectionHasNoDefaultItems<T>(IEnumerable<T> argument, [InvokerParameterName] string argumentName = null)
        {
            ObjectIsNotNull(argument, argumentName);

            if (argument.Any(currentItem => Equals(default(T), currentItem)))
                ThrowException("Collection is null or contains invalid elements.", argumentName);
        }

        /// <summary>
        /// Checks if the specified array and offset are valid.
        /// </summary>
        /// <param name="array">Argument.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void CheckArrayArguments(IList array, int offset, [InvokerParameterName] string argumentName = null)
        {
            ObjectIsNotNull(array, argumentName);

            if (offset < 0)
                ThrowException("Offset must be greater than or equal to zero.", argumentName);

            if (offset >= (array.Count - 1))
                ThrowException("Offset must be lesser than or equal to last item index.", argumentName);
        }

        /// <summary>
        /// Checks if the specified array, offset and count are valid.
        /// </summary>
        /// <param name="array">Argument.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void CheckArrayArguments(IList array, int offset, int count, [InvokerParameterName] string argumentName = null)
        {
            CheckArrayArguments(array, offset, argumentName);

            if (count <= 0)
                ThrowException("Count must be greater than zero.");
            if ((offset + count) > array.Count)
                ThrowException("Array does not contain enough elements.");
        }

        /// <summary>
        /// Checks if the specified array and offset are valid.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="array">Argument.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void CheckArrayArguments<T>(IList<T> array, int offset, [InvokerParameterName] string argumentName = null)
        {
            ObjectIsNotNull(array, argumentName);

            if (offset < 0)
                ThrowException("Offset must be greater than or equal to zero.", argumentName);
            if (offset >= (array.Count - 1))
                ThrowException("Offset must be lesser than or equal to last item index.", argumentName);
        }

        /// <summary>
        /// Checks if the specified array, offset and count are valid.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="array">Argument.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void CheckArrayArguments<T>(IList<T> array, int offset, int count, [InvokerParameterName] string argumentName = null)
        {
            CheckArrayArguments(array, offset, argumentName);

            if (count <= 0)
                ThrowException("Count must be greater than zero.", argumentName);
            if ((offset + count) > array.Count)
                ThrowException("Array does not contain enough elements.", argumentName);
        }

        /// <summary>
        /// Checks if the specified type is enumeration.
        /// </summary>
        /// <param name="type">Target type.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void IsEnumeration(Type type, [InvokerParameterName] string argumentName = null)
        {
            ObjectIsNotNull(type, argumentName);

            if (!type.IsEnum)
                ThrowException(string.Format(CultureInfo.InvariantCulture, "{0} is not an enumeration.", type.Name), argumentName);
        }

        /// <summary>
        /// Checks if the specified argument is enumeration.
        /// Doesn't check for null because struct can't be null
        /// </summary>
        /// <typeparam name="T">Argument type.</typeparam>
        /// <param name="argument">Argument.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void IsEnumeration<T>(T argument, [InvokerParameterName] string argumentName = null) where T : struct, IComparable, IFormattable, IConvertible
        {
            IsEnumeration(argument.GetType(), argumentName);
        }

        /// <summary>
        /// Checks if the specified value is defined in enumeration. 
        /// </summary>
        /// <typeparam name="T">Argument type.</typeparam>
        /// <param name="argument">Argument.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void EnumerationValueIsDefined<T>(T argument, [InvokerParameterName] string argumentName = null) where T : struct, IComparable, IFormattable, IConvertible
        {
            IsEnumeration(argument, argumentName);

            Type enumerationType = typeof(T);

            if (!Enum.IsDefined(enumerationType, argument))
                ThrowException(string.Format(CultureInfo.InvariantCulture, "Enumeration {0} does not contain value {1}.", enumerationType.Name, argument), argumentName);

            if (!EnumHelper<T>.Values.Contains(argument))
                ThrowException(string.Format(CultureInfo.InvariantCulture, "Enumeration value {0} is of enum {1} denied to use", argument, enumerationType), argumentName);
        }

        /// <summary>
        /// Checks if the specified object is not null and can be casted to the specified type.
        /// </summary>
        /// <typeparam name="TInput">Object type.</typeparam>
        /// <typeparam name="TResult">Target type.</typeparam>
        /// <param name="input">Checked object.</param>
        /// <param name="argumentName">Argument name.</param>
        /// <returns>Object casted to the specified type.</returns>
        public static TResult Cast<TInput, TResult>(TInput input, [InvokerParameterName] string argumentName = null)
            where TResult : class, TInput
        {
            ObjectIsNotNull(input, argumentName);

            var result = input as TResult;

            if (result == null)
                ThrowException("Unable to cast object with type {0} to the type {1}".Combine(input.GetType(), typeof(TResult)), argumentName);

            return result;
        }

        /// <summary>
        /// Checks if the specified object is not null and can be casted to the specified type.
        /// </summary>
        /// <typeparam name="TResult">Target type.</typeparam>
        /// <param name="input">Checked object.</param>
        /// <param name="argumentName">Argument name.</param>
        /// <returns>Object casted to the specified type.</returns>
        public static TResult Cast<TResult>(object input, [InvokerParameterName] string argumentName = null)
            where TResult : class
        {
            return Cast<object, TResult>(input, argumentName);
        }
    }
}
