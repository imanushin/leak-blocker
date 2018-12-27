using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Layouts
{
    internal sealed class GridLengthCollectionConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            return s != null ? ParseString(s) : base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is GridLengthCollection)
                return ToString((GridLengthCollection)value);

            return base.ConvertTo(context, culture, value, destinationType);
        }

        private string ToString(IEnumerable<GridLength> value)
        {
            return string.Join(",", value.Select(v => ConvertToString(v)));
        }

        private static GridLengthCollection ParseString(string inputString)
        {
            var converter = new GridLengthConverter();
            var lengths = inputString.Replace(" ", string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => (GridLength)converter.ConvertFromString(p.Trim()));
            return new GridLengthCollection(lengths.ToArray());
        }
    }
}