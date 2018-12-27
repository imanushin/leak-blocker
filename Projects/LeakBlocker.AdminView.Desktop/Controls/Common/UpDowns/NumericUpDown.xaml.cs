using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.UpDowns
{
    internal sealed partial class NumericUpDown : INotifyPropertyChanged
    {
        public static readonly DependencyProperty ValueProperty =
             DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDown), new PropertyMetadata(default(int), CallBack));


        public NumericUpDown()
        {
            InitializeComponent();

            Value = 0;
        }

        public int MinValue
        {
            get
            {
                return inputBox.MinValue;
            }
            set
            {
                inputBox.MinValue = value;
            }
        }

        public int MaxValue
        {
            get
            {
                return inputBox.MaxValue;
            }
            set
            {
                inputBox.MaxValue = value;
            }
        }

        [Bindable(true)]
        public int Value
        {
            get
            {
                return (int)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        private static void CallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = (NumericUpDown)d;

            if (e.NewValue == e.OldValue)
                return;

            current.inputBox.Value = (int)e.NewValue;
            current.OnPropertyChanged("Value");

            if (current.inputBox.Value != current.Value)
                current.Value = current.inputBox.Value;
        }

        private void UpClickedHandler(object sender, RoutedEventArgs e)
        {
            Value++;
        }

        private void DownClickedHandler(object sender, RoutedEventArgs e)
        {
            Value--;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
