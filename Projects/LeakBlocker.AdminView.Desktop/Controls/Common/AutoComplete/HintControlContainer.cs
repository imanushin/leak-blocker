using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.AutoComplete
{
    internal sealed class HintControlContainer : ContentControl
    {
        private readonly Func<FrameworkElement> imageGetter;
        private readonly string text;

        public HintControlContainer(Func<FrameworkElement> imageGetter, string text)
        {
            this.text = text;
            this.imageGetter = imageGetter;

            IsVisibleChanged += HintControlContainer_IsVisibleChanged;
            Style = (Style) Application.Current.Resources["ContentControlStyle"];
        }

        public void HintControlContainer_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Content != null || Visibility != Visibility.Visible)
                return;

            Content = new HintControl(imageGetter, text);
        }

        public void HighlightText(string pattern)
        {
            if(Content == null)
                Content = new HintControl(imageGetter, text);

            ((HintControl)Content).HighlightText(pattern);
        }

        public override string ToString()
        {
            return text;
        }
    }
}
