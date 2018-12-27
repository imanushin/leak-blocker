using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TcpGenerator
{
    internal static class TypesFinder
    {
        public static IEnumerable<Type> FindTypes(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(type => type.GetCustomAttributes(false).FirstOrDefault(attr => attr.GetType().Name == "NetworkObjectAttribute") != null);
        }

        public static bool IsReadOnlyType(Type type)
        {
            while (type != null &&  type != typeof(object))
            {
                if (type.Name == "BaseReadOnlyObject")
                    return true;

                type = type.BaseType;
            }

            return false;
        }
    }
}
