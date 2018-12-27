using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.TextBoxes
{
    internal sealed class NumberRestrictedTextBox : BaseRestrictedTextBox
    {
        public static readonly DependencyProperty ValueProperty =
    DependencyProperty.Register("Value", typeof(int), typeof(NumberRestrictedTextBox), new PropertyMetadata(default(int)));

        private int minValue = int.MinValue;
        private int maxValue = int.MaxValue;

        public NumberRestrictedTextBox()
        {
            Text = Value.ToString(CultureInfo.InvariantCulture);
        }

        public int Value
        {
            get
            {
                return (int)GetValue(ValueProperty);
            }
            set
            {
                if (value < minValue || value > maxValue)
                    return;

                if (value == Value)
                    return;

                SetValue(ValueProperty, value);

                Text = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public int MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                if (minValue > maxValue)
                    throw new InvalidOperationException("MinValue should be greater then max value. Min value: {0}, max value: {1}".Combine(minValue, maxValue));

                minValue = value;

                if (Value < minValue)
                    Value = minValue;
            }
        }

        public int MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                if (minValue > maxValue)
                    throw new InvalidOperationException("MinValue should be greater then max value. Min value: {0}, max value: {1}".Combine(minValue, maxValue));

                maxValue = value;

                if (Value > maxValue)
                    Value = maxValue;
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            Value = int.Parse(Text);
        }

        protected override bool IsCorrect(string text)
        {
            int result;

            return int.TryParse(text, out result);
        }
    }
}
