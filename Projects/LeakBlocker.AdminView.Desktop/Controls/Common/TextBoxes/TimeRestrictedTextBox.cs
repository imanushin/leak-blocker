using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.TextBoxes
{
    internal sealed class TimeRestrictedTextBox : BaseRestrictedTextBox
    {
        private const string format = "{0,2}:{1,2}";

        private DateTime baseTime = DateTime.Now;

        public DateTime Time
        {
            get
            {
                int hours;
                int minutes;

                if (IsCorrect(Text, out hours, out minutes))
                {
                    return new DateTime(baseTime.Year, baseTime.Month, baseTime.Day, hours, minutes, 0, DateTimeKind.Local);
                }

                return baseTime;
            }
            set
            {
                Text = format.Combine(value.Hour, value.Minute).Replace(' ', '0');

                baseTime = value;
                
                CaretIndex = Text.Length;
            }
        }

        protected override bool IsCorrect(string text)
        {
            int hours;
            int minutes;
            return IsCorrect(text, out hours, out minutes);
        }

        private static bool IsCorrect(string text, out int hours, out int minutes)
        {
            hours = 0;
            minutes = 0;

            if (string.IsNullOrWhiteSpace(text))
                return false;

            if (!text.Contains(':'))
                return false;

            string[] splitted = text.Split(new[] { ':' }, StringSplitOptions.None);

            if (splitted.Length != 2)
                return false;

            if (!int.TryParse(splitted[0], out hours) && !string.IsNullOrWhiteSpace(splitted[0]))
                return false;

            if (!int.TryParse(splitted[1], out minutes) && !string.IsNullOrWhiteSpace(splitted[1]))
                return false;

            if (string.IsNullOrWhiteSpace(splitted[0]))
                hours = 0;

            if (string.IsNullOrWhiteSpace(splitted[1]))
                minutes = 0;

            if (hours < 0 || hours > 23)
                return false;

            if (minutes < 0 || minutes > 59)
                return false;

            return true;
        }
    }
}
