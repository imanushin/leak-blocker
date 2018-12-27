using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common
{
    internal sealed partial class DateTimePicker
    {
        private const int roundForMinutes = 15;

        private enum TimePart
        {
            Hours,
            Minutes
        }

        public DateTimePicker()
        {
            InitializeComponent();
        }

        public DateTime Value
        {
            get
            {
                DateTime date = datePicker.SelectedDate ?? DateTime.Now;
                DateTime time = timeTextBox.Time;

                return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second, DateTimeKind.Local);
            }
            set
            {
                DateTime roundedValue = value.AddMinutes(roundForMinutes - value.Minute % 15);

                timeTextBox.Time = roundedValue;
                datePicker.SelectedDate = roundedValue;
            }
        }

        private void HighlightPart(TimePart part)
        {
            switch (part)
            {
                case TimePart.Hours:
                    timeTextBox.SelectionStart = 0;
                    break;
                case TimePart.Minutes:
                    timeTextBox.SelectionStart = timeTextBox.Text.IndexOf(':') + 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("part");
            }

            timeTextBox.SelectionLength = 2;
        }

        private void TimeUpClicked(object sender, RoutedEventArgs e)
        {
            UpdateTime( 1);
        }

        private void TimeDownClicked(object sender, RoutedEventArgs e)
        {
            UpdateTime( -1);
        }

        private void UpdateTime(int increaseValue)
        {
            TimePart part = GetCurrentPart();

            timeTextBox.Time = PatchTime(part, increaseValue);
            HighlightPart(part);
            timeTextBox.Focus();
        }

        private DateTime PatchTime(TimePart part, int value)
        {
            switch (part)
            {
                case TimePart.Hours:
                    return timeTextBox.Time.AddHours(value);

                case TimePart.Minutes:
                    return timeTextBox.Time.AddMinutes(value * roundForMinutes);

                default:
                    throw new ArgumentOutOfRangeException("part");
            }
        }

        private TimePart GetCurrentPart()
        {
            int caretIndex = timeTextBox.CaretIndex;

            if (caretIndex < 0)
                return TimePart.Minutes;

            int indexOfDottes = timeTextBox.Text.IndexOf(':');

            if (indexOfDottes < 0)
                return TimePart.Minutes;

            if (caretIndex > indexOfDottes)
                return TimePart.Minutes;

            return TimePart.Hours;
        }
    }
}
