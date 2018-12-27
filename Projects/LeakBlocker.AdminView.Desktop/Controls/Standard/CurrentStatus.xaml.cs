using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using LeakBlocker.AdminView.Desktop.Controls.Common;
using LeakBlocker.AdminView.Desktop.Controls.Common.AccountWindows;
using LeakBlocker.AdminView.Desktop.Controls.Common.Converters;
using LeakBlocker.AdminView.Desktop.Controls.Standard.SettingsChangeActions;
using LeakBlocker.AdminView.Desktop.Controls.Standard.TemporaryAccess;
using LeakBlocker.AdminView.Desktop.Controls.Standard.Views;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard
{
    internal sealed partial class CurrentStatus : IDisposable
    {
        private readonly IScheduler statusUpdateScheduler;

        public CurrentStatus()
        {
            InitializeComponent();

            statusUpdateScheduler = SharedObjects.CreateScheduler(UpdateList, TimeSpan.FromMinutes(1), false);

            UiObjects.UiConfigurationManager.ConfigurationChanged += statusUpdateScheduler.RunNow;

            statusUpdateScheduler.RunNow();

            UpdateMenuItems();
        }

        private void UpdateList()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                busyIndicator.Visibility = Visibility.Visible;
            }));

            try
            {
                using (IStatusTools client = UiObjects.CreateStatusToolsClient())
                {
                    ReadOnlySet<ManagedComputer> statuses = client.GetStatuses();

                    Dispatcher.BeginInvoke(new Action(() =>
                        {
                            UpdateList(statuses);
                            busyIndicator.Visibility = Visibility.Collapsed;
                            lastErrorText.Visibility = Visibility.Collapsed;
                        })
                        );
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                    {
                        lastErrorText.Tag = ex.GetExceptionMessage();
                        lastErrorText.Visibility = Visibility.Visible;
                        busyIndicator.Visibility = Visibility.Collapsed;
                    }));
            }
        }

        private void UpdateList(ReadOnlySet<ManagedComputer> statuses)
        {
            ReadOnlySet<ManagedComputerView> currentComputers =
                resultsView.Items.Cast<ManagedComputerView>().ToReadOnlySet();

            List<ManagedComputerView> oldItems =
                currentComputers.Where(item => !statuses.Contains(item.Computer)).ToList();

            oldItems.ForEach(resultsView.Items.Remove);

            foreach (ManagedComputer computer in statuses)
            {
                var newView = currentComputers.FirstOrDefault(item => Equals(item.Computer, computer));

                if (newView == null)
                {
                    newView = new ManagedComputerView(computer);

                    resultsView.Items.Add(newView);
                }

                newView.Computer = computer;
            }

            UpdateLists();

            statusTextBlock.Inlines.Clear();
            statusTextBlock.Inlines.Add(ManagedComputerViewsToStatistic.GetStatusEntry(resultsView.Items));
        }

        private void SaveChangesClick(object sender, RoutedEventArgs e)
        {
            SimpleConfiguration config = UiObjects.UiConfigurationManager.Configuration;

            foreach (BaseChangeAction action in changesList.Items)
            {
                config = action.AddSettings(config);
            }

            busyIndicator.Visibility = Visibility.Visible;

            SharedObjects.AsyncInvoker.Invoke(SaveConfig, config);
        }

        private void SaveConfig(SimpleConfiguration newConfig)
        {
            UiObjects.UiConfigurationManager.SaveNewConfiguration(newConfig);

            Dispatcher.BeginInvoke(
                new Action(() =>
                      {
                          busyIndicator.Visibility = Visibility.Collapsed;
                          changesList.Items.Clear();
                          changedPanel.Visibility = Visibility.Collapsed;
                          resultsView.Focus();
                      }));
        }

        private void InitMenuItem(object sender, EventArgs eventArgs)
        {
            var menuItem = (MenuItem)sender;

            var baseTarget = (string)menuItem.Tag;

            var tooltipBinding = new Binding();
            var isEnabledBinding = new Binding();

            tooltipBinding.ElementName = baseTarget;
            tooltipBinding.Path = new PropertyPath(ToolTipProperty.Name);

            isEnabledBinding.ElementName = baseTarget;
            isEnabledBinding.Path = new PropertyPath(IsEnabledProperty.Name);

            menuItem.SetBinding(ToolTipProperty, tooltipBinding);
            menuItem.SetBinding(IsEnabledProperty, isEnabledBinding);

            menuItem.Width = double.NaN;

            NameScope.SetNameScope(menuItem, NameScope.GetNameScope(this));
        }

        private void ComputersSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMenuItems();
            UpdateLists();
        }

        private void UpdateLists()
        {
            var selectedComputer = (ManagedComputerView)resultsView.SelectedItem;

            if (selectedComputer == null)
            {
                noUsersText.Visibility = Visibility.Collapsed;
                noDevicesText.Visibility = Visibility.Collapsed;

                return;
            }

            noUsersText.Visibility = selectedComputer.Users.Any() ? Visibility.Collapsed : Visibility.Visible;
            noDevicesText.Visibility = selectedComputer.Devices.Any() ? Visibility.Collapsed : Visibility.Visible;
        }

        private void UpdateMenuItems()
        {
            ReadOnlySet<ManagedComputerView> selected = resultsView.SelectedItems.Cast<ManagedComputerView>().ToReadOnlySet();

            ReadOnlySet<BaseComputerAccount> alreadyExcluded = changesList.Items.OfType<ExcludeComputerAction>().Select(action => action.Computer).ToReadOnlySet();
            ReadOnlySet<BaseComputerAccount> selectedComputers = selected.Select(item => item.Computer.TargetComputer).ToReadOnlySet();

            excludeComputerMenu.IsEnabled = selectedComputers.Without(alreadyExcluded).Any();
            openAuditMenu.IsEnabled = selected.Any();

            getTemporaryAccessMenu.IsEnabled = selected.Any();
        }

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            statusUpdateScheduler.RunNow();
        }

        private void AddToSettingsChanges(IEnumerable<BaseChangeAction> actions)
        {
            var existingActions = changesList.Items.Cast<BaseChangeAction>().ToReadOnlySet();

            var newActions = actions.Without(existingActions).ToReadOnlySet();

            if (!newActions.Any())
                return;

            foreach (BaseChangeAction baseChangeAction in newActions)
            {
                changesList.Items.Add(baseChangeAction);
            }

            changedPanel.Visibility = Visibility.Visible;
        }

        private void ExcludeClicked(object sender, RoutedEventArgs e)
        {
            var newActions = resultsView
                .SelectedItems
                .Cast<ManagedComputerView>()
                .Select(computerView => new ExcludeComputerAction(computerView.Computer.TargetComputer));

            AddToSettingsChanges(newActions);
        }

        private void GiveComputerTemporaryAccessClicked(object sender, RoutedEventArgs e)
        {
            var computers = SelectedComputers;

            IEnumerable<BaseTemporaryAccessCondition> conditions =
                TemporaryAccessSelectionWindow.ShowDialogForComputers(
                computers,
                Window.GetWindow(this));

            var newActions = conditions.Select(condition => new GetTemporaryAccessAction(condition));

            AddToSettingsChanges(newActions);
        }

        private ReadOnlySet<BaseComputerAccount> SelectedComputers
        {
            get
            {
                ReadOnlySet<BaseComputerAccount> computers =
                    resultsView
                        .SelectedItems
                        .Cast<ManagedComputerView>()
                        .Select(item => item.Computer.TargetComputer)
                        .ToReadOnlySet();
                return computers;
            }
        }

        private ReadOnlySet<DeviceDescription> SelectedDevices
        {
            get
            {
                ReadOnlySet<DeviceDescription> result =
                    devices
                        .SelectedItems
                        .Cast<DeviceView>()
                        .Select(item => item.TargetDevice)
                        .ToReadOnlySet();
                return result;
            }
        }

        private ReadOnlySet<BaseUserAccount> SelectedUsers
        {
            get
            {
                ReadOnlySet<BaseUserAccount> result =
                    users
                        .SelectedItems
                        .Cast<BaseUserAccount>()
                        .ToReadOnlySet();
                return result;
            }
        }

        private void UndoChangeActionHandler(object sender, RoutedEventArgs e)
        {
            UndoSelectedActions();
        }

        private void UndoSelectedActions()
        {
            ReadOnlySet<BaseChangeAction> selected = changesList.SelectedItems.Cast<BaseChangeAction>().ToReadOnlySet();

            selected.ForEach(changesList.Items.Remove);

            if (changesList.Items.Count == 0)
                changedPanel.Visibility = Visibility.Collapsed;
        }

        private void ChangedListKeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete)
                return;

            UndoSelectedActions();
        }

        private void OpenAuditClicked(object sender, RoutedEventArgs e)
        {
            App.AuditManager.OpenComputersAudit(SelectedComputers);
        }

        private void CancelTemporaryAccessClicked(object sender, RoutedEventArgs e)
        {
            ReadOnlySet<CancelTemporaryAccessAction> cancelTemporaryAccessActions = changesList.Items.OfType<CancelTemporaryAccessAction>().ToReadOnlySet();

            ReadOnlySet<BaseTemporaryAccessCondition> conditions = cancelTemporaryAccessActions.Select(action => action.Condition).ToReadOnlySet();

            var resultConditions = CancelTemporaryAccessWindow.RequstCancelConditions(Window.GetWindow(this), conditions);

            if (resultConditions == null)
                return;

            ReadOnlySet<CancelTemporaryAccessAction> actionsToRemove =
                cancelTemporaryAccessActions.
                Where(action => !resultConditions.Contains(action.Condition)).
                ToReadOnlySet();

            ReadOnlySet<CancelTemporaryAccessAction> newActions =
                resultConditions.
                Where(tac => !conditions.Contains(tac)).
                Select(tac => new CancelTemporaryAccessAction(tac)).
                ToReadOnlySet();

            actionsToRemove.ForEach(changesList.Items.Remove);

            AddToSettingsChanges(newActions);

            if (changesList.Items.Count == 0)
                changedPanel.Visibility = Visibility.Collapsed;
        }

        private void UpdateCredentialsClicked(object sender, RoutedEventArgs e)
        {
            var item = (ManagedComputerView)resultsView.SelectedItem;

            if (item == null)
                return;

            var localComputer = item.Computer.TargetComputer as LocalComputerAccount;
            var domainComputer = item.Computer.TargetComputer as DomainComputerAccount;

            if (localComputer != null)
            {
                AskAndUpdateCredentialsWindow.OpenWindow(localComputer, Window.GetWindow(this));
            }
            else
            {
                Check.ObjectIsNotNull(domainComputer);

                AskAndUpdateCredentialsWindow.OpenWindow(domainComputer.Parent, Window.GetWindow(this));
            }
        }

        private void ForceAgentInstallationClicked(object sender, RoutedEventArgs e)
        {
            ReadOnlySet<BaseComputerAccount> computers = SelectedComputers;

            busyIndicator.Visibility = Visibility.Visible;

            SharedObjects.AsyncInvoker.Invoke(ForceAgentInstallation, computers);
        }

        private void ForceAgentInstallation(ReadOnlySet<BaseComputerAccount> computers)
        {
            try
            {
                using (IAgentInstallationTools client = UiObjects.CreateAgentInstallationToolsClient())
                {
                    client.ForceInstallation(computers);
                }

                Thread.Sleep(5000);//Ждем 5 секунд, чтобы установка запустилась
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Window.GetWindow(this),
                    AdminViewResources.ErrorWasOccured.Combine(ex.GetExceptionMessage()),
                    CommonStrings.ProductName,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            UpdateList();
        }

        private void OpenDeviceAuditClicked(object sender, RoutedEventArgs e)
        {
            ReadOnlySet<DeviceDescription> selectedDevices = SelectedDevices;

            if (selectedDevices.Any())
                App.AuditManager.OpenDevicesAudit(selectedDevices);
        }

        private void OpenUserAuditClicked(object sender, RoutedEventArgs e)
        {
            ReadOnlySet<BaseUserAccount> selectedUsers = SelectedUsers;

            if (selectedUsers.Any())
                App.AuditManager.OpenUsersAudit(selectedUsers);
        }

        private void GiveDeviceTemporaryAccessClicked(object sender, RoutedEventArgs e)
        {
            ReadOnlySet<DeviceDescription> selectedDevices = SelectedDevices;

            if (!selectedDevices.Any())
                return;

            IEnumerable<BaseTemporaryAccessCondition> conditions =
                TemporaryAccessSelectionWindow.ShowDialogForDevices(
                selectedDevices,
                Window.GetWindow(this));

            var newActions = conditions.Select(condition => new GetTemporaryAccessAction(condition));

            AddToSettingsChanges(newActions);
        }

        private void GiveUserTemporaryAccessClicked(object sender, RoutedEventArgs e)
        {
            ReadOnlySet<BaseUserAccount> selectedUsers = SelectedUsers;

            if (!selectedUsers.Any())
                return;

            IEnumerable<BaseTemporaryAccessCondition> conditions =
                TemporaryAccessSelectionWindow.ShowDialogForUsers(
                selectedUsers,
                Window.GetWindow(this));

            var newActions = conditions.Select(condition => new GetTemporaryAccessAction(condition));

            AddToSettingsChanges(newActions);
        }

        private void AddDeviceToWhiteList(object sender, RoutedEventArgs e)
        {
            AddToSettingsChanges(SelectedDevices.Select(device => new AddDeviceToWhiteList(device)));
        }

        private void AddUserToWhiteList(object sender, RoutedEventArgs e)
        {
            AddToSettingsChanges(SelectedUsers.Select(user => new AddUserToWhiteList(user)));
        }

        public void Dispose()
        {
            Disposable.DisposeSafe(statusUpdateScheduler);
        }

        private void LastErrorHyperlinkClicked(object sender, RoutedEventArgs e)
        {
            var errorText = (string)lastErrorText.Tag;

            Check.StringIsMeaningful(errorText);

            MessageBox.Show(
                Window.GetWindow(this),
                AdminViewResources.UnableToRetrieveComputerStatusesTemplate.Combine(errorText),
                CommonStrings.ProductName,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                SharedObjects.AsyncInvoker.Invoke(UpdateList);
            }
        }
    }
}
