using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EntitiesGenerator
{
    internal static class BaseObjectsFinder
    {
        private const string EntitiesAssemblyName = "LeakBlocker.Libraries.Common.dll";

        public static IEnumerable<Type> GetDatabaseObjects(string pathToBinaries)
        {
            Type baseObject = GetBaseObjectType(pathToBinaries);

            Assembly entitiesAssembly = Assembly.LoadFrom(Path.Combine(pathToBinaries, EntitiesAssemblyName));

            return entitiesAssembly.GetTypes()
                .Where(type => type != baseObject)
                .Where(type => type.IsSubclassOf(baseObject));
        }

        public static Type GetBaseObjectType(string pathToBinaries)
        {
            Assembly entitiesAssembly = Assembly.LoadFrom(Path.Combine(pathToBinaries, EntitiesAssemblyName));

            const string typeName = "LeakBlocker.Libraries.Common.Entities.BaseEntity";

            return entitiesAssembly.GetType(typeName, true, false);
        }
    }
}
