using System;
using System.Collections.Generic;
using System.Linq;

namespace EntitiesGenerator.AdditionalHelpers
{
    internal static class CustomDbFields
    {
        internal const string EnumMemberTemplate =
@"
        /// <summary>
        /// Inner property for database
        /// </summary>
{2}
        [SuppressMessage(""Microsoft.Naming"", ""CA1720:IdentifiersShouldNotContainTypeNames"", MessageId = ""int"")]
        public virtual int {1}IntValue
        {{
            get;
            set;
        }}
        
        /// <summary>
        /// Property from <see cref=""{3}""/>
        /// </summary>
        internal {0} {1}
        {{
            get
            {{
                var result = ({0}){1}IntValue;

                Check.EnumerationValueIsDefined(({0}){1}IntValue);

                return result;
            }}
            set
            {{
                Check.EnumerationValueIsDefined(value, ""value"");

                {1}IntValue = (int)value;
           
                CachedItem = null;
            }}
        }}
";

        internal const string TimeMemberTemplate =
@"
        /// <summary>
        /// Inner property for time representation in database
        /// </summary>
{1}
        public virtual long {0}Ticks
        {{
            get;
            set;
        }}
        
        /// <summary>
        /// Property from <see cref=""{2}""/>
        /// </summary>
        [NotMapped]
        internal Time {0}
        {{
            get
            {{
                return new Time({0}Ticks);
            }}
            set
            {{
                Check.ObjectIsNotNull(value, ""value"");

                {0}Ticks = value.Ticks;
           
                CachedItem = null;
            }}
        }}
";


    }
}
