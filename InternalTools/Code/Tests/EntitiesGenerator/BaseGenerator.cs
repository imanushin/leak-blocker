using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesGenerator
{
    internal abstract class BaseGenerator
    {
        private readonly Type baseObjectType;

        protected BaseGenerator(Type baseObjectType)
        {
            this.baseObjectType = baseObjectType;
        }

        protected Type BaseObjectType
        {
            get
            {
                return baseObjectType;
            }
        }

        protected bool IsParentBaseType(Type type)
        {
            return baseObjectType == type.BaseType;
        }

        protected Type GetBaseEntityType(Type type)
        {
            Type currentType = type;

            while (baseObjectType != currentType.BaseType)
            {
                currentType = currentType.BaseType;
            }

            return currentType;
        }

    }
}
