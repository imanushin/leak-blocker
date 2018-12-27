using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesGenerator.AdditionalHelpers
{
    internal static class CachedGetGenerator
    {
        private const string baseObjectTemplate =
@"        /// <summary>
        /// Object using to cache items converted from <see cref=""Db{0}""/> to  <see cref=""{0}""/>
        /// </summary>
        [NotMapped]
        [CanBeNull]
        protected {0} CachedItem
        {{
            get;
            set;
        }}
";

        private const string allObjectsTemplate =
@"        internal {0} Get{0}()
        {{
            if( CachedItem == null )
            {{
                var createdItem = ForceGet{0}();

                CachedItem = createdItem;
            }}

            return ({0})CachedItem;
        }}
";

        public static string GenerateCachedGetItems(Type type, Type baseEntityType)
        {
            var result = new StringBuilder();

            bool isParentReadOnly = baseEntityType == type.BaseType;
            bool isSealed = type.IsSealed;

            result.AppendFormat(allObjectsTemplate, type.Name);

            if (isParentReadOnly)
            {
                result.AppendFormat(baseObjectTemplate, type.Name);
            }

            return result.ToString();
        }
    }
}
