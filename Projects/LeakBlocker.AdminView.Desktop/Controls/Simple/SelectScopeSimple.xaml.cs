using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Simple
{
    internal sealed partial class SelectScopeSimple
    {
        private ReadOnlySet<ResultComputer> lockedComputers = new ReadOnlySet<ResultComputer>(new ResultComputer[0]);
        private ReadOnlySet<ResultComputer> unlockedComputers = new ReadOnlySet<ResultComputer>(new ResultComputer[0]);

        private SimpleConfiguration blockOptions;

        public SelectScopeSimple()
        {
            InitializeComponent();
            UpdateSummary();

            blockedScope.ScopeEntryChanged += computers => UpdateResultComputerList(computers, unlockedComputers);
            excludedScope.ScopeEntryChanged += computers => UpdateResultComputerList(lockedComputers, computers);
        }

        internal void UpdateConfiguration(SimpleConfiguration configuration)
        {
            blockOptions = configuration;
            blockedScope.Scopes = blockOptions.BlockedScopes;
            excludedScope.Scopes = blockOptions.ExcludedScopes;
        }

        internal void SetBusy(bool isBusy)
        {
            busyIndicator.IsBusy = isBusy;
        }

        private SimpleConfiguration GetConfiguration()
        {
            return blockOptions.GetFromCurrent(blockedScopes: blockedScope.Scopes, excludedScopes: excludedScope.Scopes);
        }

        private void UpdateResultComputerList(ReadOnlySet<ResultComputer> newLockedComputers, ReadOnlySet<ResultComputer> newUnlockedComputers)
        {
            Check.CollectionHasNoDefaultItems(newLockedComputers, "newLockedComputers");
            Check.CollectionHasNoDefaultItems(newUnlockedComputers, "newUnlockedComputers");

            lockedComputers = newLockedComputers;
            unlockedComputers = newUnlockedComputers;

            UpdateResultComputerList();
        }

        private void UpdateResultComputerList()
        {
            var resultComputers = lockedComputers.Without(unlockedComputers).ToReadOnlySet();

            results.ItemsSource = resultComputers;

            UpdateSummary();
        }

        private void UpdateSummary()
        {
            computersInScopeTitle.Text = AdminViewResources.ComputersInScope.Combine(lockedComputers.Except(unlockedComputers).Count());
            blockedScope.Title = AdminViewResources.BlockedArea.Combine(lockedComputers.Count);
            excludedScope.Title = AdminViewResources.Excluded.Combine(unlockedComputers.Count);
        }

        private void ExcludeComputersClicked(object sender, RoutedEventArgs e)
        {
            if (results.SelectedItems.Count == 0)
                return;

            excludedScope.AddScopes(
                results.SelectedItems
                .Cast<ResultComputer>()
                .Select(item => new Scope(item.TargetAccount)));
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            SimpleConfiguration configuration = GetConfiguration();

            StartLocalBusyStatus(AdminViewResources.SavingConfiguration);

            SharedObjects.AsyncInvoker.Invoke(SaveConfiguration, configuration);
        }

        private void StartLocalBusyStatus(string savingConfiguration)
        {
            updatingIndicator.Visibility = Visibility.Visible;
            updatingIndicator.Text = savingConfiguration;
            saveButton.IsEnabled = false;
        }

        private void ClearLocalBusyStatus()
        {
            updatingIndicator.Visibility = Visibility.Collapsed;
            saveButton.IsEnabled = true;
        }

        private void AdvancedButtonClicked(object sender, RoutedEventArgs e)
        {
            blockOptions = AdvancedOptionsWindow.OpenWindow(blockOptions, Window.GetWindow(this));
        }

        private void SaveConfiguration(SimpleConfiguration configuration)
        {
            try
            {
                UiObjects.UiConfigurationManager.SaveNewConfiguration(configuration);
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() => MessageBox.Show(ex.GetExceptionMessage(), CommonStrings.ProductName, MessageBoxButton.OK, MessageBoxImage.Error)));
            }

            Dispatcher.BeginInvoke(new Action(ClearLocalBusyStatus));
        }
    }
}
