﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Animations
{
    internal sealed partial class WaitIndicatorText
    {
        public WaitIndicatorText()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(WaitIndicatorText), new PropertyMetadata(default(string)));
        
        public string Text
        {
            get
            {
                return (string) GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
    }
}
