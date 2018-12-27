using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common
{
    internal abstract partial class AbstractSelectionWindow
    {
        public event Action UserInputFinished;
        public event Action FindInOtherLocationClicked;

        protected AbstractSelectionWindow(
                string pleaseEnterText,
                Func<object, FrameworkElement> imageTemplateSelector,
                string examples = null)
        {
            InitializeComponent();

            searchTextBox.SetObjectToImageTemplateConverter(imageTemplateSelector);
            searchTextBox.SetObjectToHintConverter(GetHint);

            title.Text = pleaseEnterText;
            helpTooltip.HelpText = examples;
            helpTooltip.Visibility = examples == null ? Visibility.Collapsed : Visibility.Visible;
        }

        protected abstract string GetHint(object searchObject);

        protected void SetItemsSource(IEnumerable items)
        {
            searchTextBox.SetItems(items);
        }

        protected string SearchText
        {
            get
            {
                return searchTextBox.Text;
            }
            set
            {
                searchTextBox.Text = value;
            }
        }

        protected void FocusSearchText()
        {
            searchTextBox.Focus();
            
        }

        public bool CanFindInOtherLocation
        {
            get
            {
                return findInOtherLocationButton.Visibility == Visibility.Visible;
            }
            set
            {
                findInOtherLocationButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void OkClickedHandler(object sender, RoutedEventArgs e)
        {
            if (UserInputFinished != null)
                UserInputFinished();
        }

        private void FindInOtherLocation(object sender, RoutedEventArgs e)
        {
            if (FindInOtherLocationClicked != null)
                FindInOtherLocationClicked();

            FocusSearchText();
        }

        private void InputFinished()
        {
            if (UserInputFinished != null)
                UserInputFinished();
        }

        private void ControlKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    return;

                case Key.Enter:
                    e.Handled = true;

                    if (string.IsNullOrWhiteSpace(searchTextBox.Text))
                        return;

                    InputFinished();
                    return;
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            searchTextBox.Focus();
        }
    }
}
