using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Indicates that enumeration has localized descriptions in the specified resource class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
    public sealed class EnumerationDescriptionProviderAttribute : Attribute
    {
        /// <summary>
        /// Type of the resource class. It is recommended to specify type of auto-generated class that 
        /// corresponds to the resx file. But in fact any type that contains static property ResourceManager of type ResourceManager can be passed.
        /// </summary>
        public Type Resource
        {
            get;
            private set;
        }

        /// <summary>
        /// Resource manager that provides description strings.
        /// </summary>
        public ResourceManager Provider
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates an instance of the EnumerationDescriptionProviderAttribute class.
        /// </summary>
        /// <param name="resource">Type of the resource class. It is recommended to specify type of auto-generated class that 
        /// corresponds to the resx file. But in fact any type that contains static property ResourceManager of type ResourceManager can be passed.</param>
        public EnumerationDescriptionProviderAttribute(Type resource)
        {
            Check.ObjectIsNotNull(resource, "resource");

            Resource = resource;

            const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetProperty;
            PropertyInfo propertyInfo = resource.GetProperty("ResourceManager", flags, null, typeof(ResourceManager), new Type[0], null);

            if (propertyInfo == null)
                Exceptions.Throw(ErrorMessage.UnsupportedType, "Specified type does not contain resource manager.");

            Provider = propertyInfo.GetValue(null, null) as ResourceManager;

            if (Provider == null)
                Exceptions.Throw(ErrorMessage.NotFound, "Failed to get resource manager.");
        }
    }
}
