using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.TemporaryAccess
{
    internal sealed partial class CancelTemporaryAccessWindow
    {
        private CancelTemporaryAccessWindow()
        {
            InitializeComponent();
        }

        private void OkClickedHandler(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelClickedHandler(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        [CanBeNull]
        public static ReadOnlySet<BaseTemporaryAccessCondition> RequstCancelConditions(
            Window owner,
            ReadOnlySet<BaseTemporaryAccessCondition> cancelledConditions)
        {
            var window = new CancelTemporaryAccessWindow();

            window.Owner = owner;

            ReadOnlySet<CancelAccessData> actions = 
                UiObjects.UiConfigurationManager.Configuration.TemporaryAccess.
                Where(ta => ta.EndTime > Time.Now).
                Select(ta => new CancelAccessData(ta, cancelledConditions.Contains(ta))).
                ToReadOnlySet();

            window.currentItems.ItemsSource = actions;


            if( actions.Any() )
                window.noItemsPanel.Visibility = Visibility.Collapsed;
            
            bool? result = window.ShowDialog();

            if (true != result)
                return null;

            return window.currentItems.Items.Cast<CancelAccessData>().Where(tad => tad.IsCancelled).Select(tad => tad.Condition).ToReadOnlySet();
        }
    }
}
