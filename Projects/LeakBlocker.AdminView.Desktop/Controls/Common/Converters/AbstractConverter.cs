using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Converters
{
    internal abstract class AbstractConverter<TFrom, TTo> : IValueConverter
    {
        private readonly bool allowNulls;

        protected AbstractConverter(bool allowNulls=false)
        {
            this.allowNulls = allowNulls;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!allowNulls)
                Check.ObjectIsNotNull(value, "value");

            return Convert((TFrom)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        protected abstract TTo Convert(TFrom baseValue);
    }
}
