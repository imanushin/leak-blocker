using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Tests.Controls.Common.Converter
{
    public abstract class BaseEnumToStringConverterTest
    {
        public void BaseConvertTest<TEnumType,TConverterType>()
            where TConverterType : IValueConverter, new()
            where TEnumType : struct, IComparable, IFormattable, IConvertible
        {
            var target = new TConverterType();

            foreach (TEnumType scopeType in EnumHelper<TEnumType>.Values)
            {
                target.Convert(scopeType, typeof(string), null, CultureInfo.InvariantCulture);
            }
        }
    }
}
