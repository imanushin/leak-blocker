using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TcpGenerator
{
    internal static class CheckStringGenerator
    {
        private const string readonlyTypeTemplate = "Check.ObjectIsNotNull({0}, \"{0}\");";

        private const string stringTemplate = "Check.StringIsMeaningful({0}, \"{0}\");";

        private static readonly HashSet<Type> ignoredTypes = new HashSet<Type>() { typeof(bool), typeof(int) };

        public static string CreateCheckString(ParameterInfo parameter)
        {
            if (TypesFinder.IsReadOnlyType(parameter.ParameterType))
                return string.Format(readonlyTypeTemplate, parameter.Name);

            if (parameter.ParameterType == typeof(string))
                return string.Format(stringTemplate, parameter.Name);

            if (parameter.ParameterType == typeof(byte[]))
                return string.Format(readonlyTypeTemplate, parameter.Name);

            if (ignoredTypes.Contains(parameter.ParameterType))
                return string.Empty;

            throw new InvalidOperationException(string.Format("Unable to generate checker for type {0}", parameter.ParameterType));
        }
    }
}
