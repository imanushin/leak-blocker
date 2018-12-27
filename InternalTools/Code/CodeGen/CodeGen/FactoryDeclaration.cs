using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen
{
    public struct InitializerParameterDeclaration
    {
        public string TypeName
        {
            get;
            set;
        }

        public string ParameterName
        {
            get;
            set;
        }

        public string DefaultValue
        {
            get;
            set;
        }
    }

    public sealed class FactoryDeclaration : StaticObjectDeclaration
    {
        public FactoryDeclaration(string interfaceName, string implementation, bool isInternal = false,
            string propertyName = null, InitializerParameterDeclaration[] initializerTypeNames = null)

            : base(interfaceName, implementation, isInternal, propertyName)
        {
            InitializerTypeNames = initializerTypeNames;
        }

        public InitializerParameterDeclaration[] InitializerTypeNames
        {
            get;
            private set;
        }

        public override string DeclarationType
        {
            get
            {
                return "Factory";
            }
        }
    }
}
