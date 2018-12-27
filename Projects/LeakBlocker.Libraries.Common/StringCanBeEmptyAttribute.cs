using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Аттрибут, при назначении которого пропускается проверка параметра, который является строкой. Иначе тестируется запрет на пустые строки
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, Inherited = true, AllowMultiple = true)]
    public sealed class StringCanBeEmptyAttribute : Attribute
    {
        /// <summary>
        /// Конструктор. На входе требуется имя аргумента
        /// </summary>
        /// <param name="parameterName"></param>
        public StringCanBeEmptyAttribute([InvokerParameterName]string parameterName)
        {
            Check.ObjectIsNotNull(parameterName, "parameterName");
            ParameterName = parameterName;
        }

        /// <summary>
        /// Имя аргумента, который может быть null
        /// </summary>
        [UsedImplicitly]
        public string ParameterName
        {
            get;
            private set;
        }

        /// <summary>
        /// Проверяет, что имя свойство равно заявленному
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Match(object obj)
        {
            return (string)obj == ParameterName;
        }
    }
}
