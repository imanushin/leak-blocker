using System;

namespace CodeGen
{
    public abstract class StaticObjectDeclaration
    {
        public StaticObjectDeclaration(string interfaceName, string implementation, bool isInternal = false, string propertyName = null)
        {
            Interface = interfaceName;
            Implementation = implementation;
            Internal = isInternal;
            PropertyName = propertyName;

            if (PropertyName == null)
                PropertyName = implementation.Replace("<", "_").Replace(">", "").Replace(",", "_");
        }

        public string Interface
        {
            get;
            private set;
        }

        public string Implementation
        {
            get;
            private set;
        }

        public bool Internal
        {
            get;
            private set;
        }

        public abstract string DeclarationType
        {
            get;
        }

        public string PropertyName
        {
            get;
            private set;
        }

        public string PrivateFieldName
        {
            get
            {
                return Char.ToLower(PropertyName[0]) + PropertyName.Substring(1, PropertyName.Length - 1);
            }
        }
    }
}
