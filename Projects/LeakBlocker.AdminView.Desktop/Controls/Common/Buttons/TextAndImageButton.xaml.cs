using System;
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

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Buttons
{
    internal partial class TextAndImageButton
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof (string), typeof (TextAndImageButton), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ImageTemplateProperty =
            DependencyProperty.Register("ImageTemplate", typeof (DataTemplate), typeof (TextAndImageButton), new PropertyMetadata(default(DataTemplate)));

        public TextAndImageButton()
        {
            InitializeComponent();
        }

        public DataTemplate ImageTemplate
        {
            get
            {
                return (DataTemplate) GetValue(ImageTemplateProperty);
            }
            set
            {
                SetValue(ImageTemplateProperty, value);
            }
        }

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

        private void IsEnabledChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            imageContainer.Effect = (bool)e.NewValue ? null : (InactivePictureEffect)Application.Current.Resources["InactivePictureEffect"];
        }

    }
}
