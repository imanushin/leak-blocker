using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.AutoComplete
{
    internal sealed partial class HintControl
    {
        private readonly string text;

        private readonly Func<FrameworkElement> imageGetter;

        public HintControl(Func<FrameworkElement> imageGetter, string text)
        {
            InitializeComponent();

            this.imageGetter = imageGetter;

            this.text = text;

            beforeHighlighted.Text = text;
        }

        public void HighlightText(string pattern)
        {
            Check.ObjectIsNotNull(pattern, "pattern");

            if (string.IsNullOrEmpty(pattern) || !text.Contains(pattern, StringComparison.OrdinalIgnoreCase))
            {
                beforeHighlighted.Text = text;
                highlighted.Text = string.Empty;
                afterHighlighted.Text = string.Empty;

                return;
            }

            int firstIndex = text.IndexOf(pattern, StringComparison.OrdinalIgnoreCase);

            beforeHighlighted.Text = text.Substring(0, firstIndex);
            highlighted.Text = text.Substring(firstIndex, pattern.Length);
            afterHighlighted.Text = text.Substring(firstIndex + pattern.Length);
        }

        private void IsVisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility != Visibility.Visible)
                return;

            if (imageContainer.Content != null)
                return;

            imageContainer.Content = imageGetter();
        }
    }
}
