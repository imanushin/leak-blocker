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
using LeakBlocker.AdminView.Desktop.Generated;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Buttons
{
    internal sealed partial class LargeVectorImagedButton
    {
        public LargeVectorImagedButton()
        {
            InitializeComponent();

            IsEnabledChanged += IsEnabledChangedHandler;
        }

        public DataTemplate InnerImage
        {
            get
            {
                return imageContainer.ContentTemplate;
            }
            set
            {
                imageContainer.ContentTemplate = value;
            }
        }

        private void IsEnabledChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            imageContainer.Effect = (bool)e.NewValue ? null : (InactivePictureEffect)Application.Current.Resources["InactivePictureEffect"];
        }
    }
}
