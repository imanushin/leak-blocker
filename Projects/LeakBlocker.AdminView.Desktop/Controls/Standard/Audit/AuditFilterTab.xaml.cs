using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;
using Microsoft.Win32;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.Audit
{
    internal sealed partial class AuditFilterTab
    {
        private const int topElementsCount = 1000;

        public event Action RemoveClicked;

        /// <summary>
        /// Аргумент - прошлый фильтр
        /// </summary>
        public event Action<AuditFilter> FilterChanged;

        private readonly bool isInitialized;
        private static readonly ReadOnlySet<object> allGroups = EnumHelper<AuditItemGroupType>.Values.Cast<object>().ToReadOnlySet();

        public AuditFilterTab(AuditFilter filter)
        {
            InitializeComponent();

            lastNRecordsShownWarning.Text = AdminViewResources.LastItemsShownTemplate.Combine(topElementsCount);

            Filter = filter;

            startDate.SelectedDate = DateTime.Now.AddDays(-2).Date;
            endDate.SelectedDate = DateTime.Now.AddDays(1).Date;

            listSelection.Users = filter.Users;
            listSelection.Devices = filter.Devices;
            listSelection.Computers = filter.Computers;

            dateTimeSelectedBox.IsChecked = filter.StartTime != Time.Unknown && filter.EndTime != Time.Unknown;

            if (filter.StartTime != Time.Unknown)
            {
                startDate.SelectedDate = filter.StartTime.ToUtcDateTime().ToLocalTime();
            }

            if (filter.EndTime != Time.Unknown)
            {
                endDate.SelectedDate = filter.EndTime.ToUtcDateTime().ToLocalTime();
            }

            headerTextBlock.Text = filter.Name;
            headerPanel.ToolTip = filter.Name;

            groups.SetItems(allGroups, filter.Types.Cast<object>().ToReadOnlySet());
            onlyErrorCheckBox.IsChecked = filter.OnlyError;

            listSelection.FilterChanged += RaiseFilterChanged;
            groups.ValueChanged += RaiseFilterChanged;

            auditItemsGrid.FilterByComputers += FilterByComputers;
            auditItemsGrid.FilterByDevices += FilterByDevices;
            auditItemsGrid.FilterByTypes += FilterByTypes;
            auditItemsGrid.FilterByUsers += FilterByUsers;

            isInitialized = true;

            UpdateItemsAsync();
        }

        private void FilterByUsers(ReadOnlySet<BaseUserAccount> users)
        {
            listSelection.usersView.SetItems(users);
            RaiseFilterChanged();
        }

        private void FilterByTypes(ReadOnlySet<AuditItemGroupType> typeGroups)
        {
            groups.SetItems(allGroups, typeGroups.Cast<object>().ToReadOnlySet());
            RaiseFilterChanged();
        }

        private void FilterByDevices(ReadOnlySet<DeviceDescription> devices)
        {
            listSelection.devicesView.SetItems(devices);
            RaiseFilterChanged();
        }

        private void FilterByComputers(ReadOnlySet<BaseComputerAccount> computers)
        {
            listSelection.computersView.SetItems(computers);
            RaiseFilterChanged();
        }

        public AuditFilter Filter
        {
            get;
            private set;
        }

        private AuditFilter CreateFromControls()
        {
            Check.ObjectIsNotNull(startDate.SelectedDate);
            Check.ObjectIsNotNull(endDate.SelectedDate);

            return new AuditFilter
                (
                    headerTextBlock.Text,
                    listSelection.computersView.Items.Cast<BaseComputerAccount>().ToReadOnlySet(),
                    listSelection.usersView.Items.Cast<BaseUserAccount>().ToReadOnlySet(),
                    listSelection.devicesView.Items.Cast<DeviceDescription>().ToReadOnlySet(),
                    dateTimeSelectedBox.IsChecked == true ? new Time(startDate.SelectedDate.Value) : Time.Unknown,
                    dateTimeSelectedBox.IsChecked == true ? new Time(endDate.SelectedDate.Value) : Time.Unknown,
                    onlyErrorCheckBox.IsChecked == true,
                    groups.SelectedItems.Cast<AuditItemGroupType>().ToReadOnlySet()
                );
        }

        private void RaiseFilterChanged()
        {
            if (!isInitialized)
                return;

            AuditFilter newFilter = CreateFromControls();
            AuditFilter oldFilter = Filter;

            if (newFilter == oldFilter)
                return;

            Filter = newFilter;

            if (FilterChanged != null)
                FilterChanged(oldFilter);

            UpdateItemsAsync();
        }

        private void UpdateItemsAsync()
        {
            waitForDataRetrieveText.Visibility = Visibility.Visible;
            SharedObjects.AsyncInvoker.Invoke(UpdateGrid, Filter);
        }

        private void UpdateGrid(AuditFilter filter)
        {
            using (IAuditTools client = UiObjects.CreateAuditToolsClient())
            {
                try
                {
                    ReadOnlyList<AuditItem> items = client.GetItemsForFilter(filter, topElementsCount);

                    Dispatcher.BeginInvoke(
                        new Action(() =>
                            {
                                if (Filter != filter)
                                    return;

                                auditItemsGrid.Items = items;

                                lastNRecordsShownWarning.Visibility = items.Count >= topElementsCount ? Visibility.Visible : Visibility.Collapsed;

                                waitForDataRetrieveText.Visibility = Visibility.Collapsed;
                            }));
                }
                catch (Exception ex)
                {
                    Dispatcher.BeginInvoke(
                                           new Action(() =>
                                           {
                                               waitForDataRetrieveText.Visibility = Visibility.Collapsed;

                                               MessageBox.Show(
                                                   Window.GetWindow(this),
                                                   AdminViewResources.UnableToRetrieveAuditData.Combine(ex.GetExceptionMessage()),
                                                   CommonStrings.ProductName,
                                                   MessageBoxButton.OK,
                                                   MessageBoxImage.Error
                                                   );
                                           }));
                }
            }
        }

        private void CloseClicked(object sender, RoutedEventArgs e)
        {
            if (RemoveClicked != null)
                RemoveClicked();
        }

        private void DateChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            RaiseFilterChanged();
        }

        private void FilterChangedHandler(object sender, RoutedEventArgs e)
        {
            RaiseFilterChanged();
        }

        private void RenameAuditTab(object sender, RoutedEventArgs e)
        {
            headerTextBlock.Visibility = Visibility.Collapsed;
            headerContextMenu.Visibility = Visibility.Collapsed;
            headerTextBox.Visibility = Visibility.Visible;
            headerTextBox.Text = Filter.Name;

            Focus();

            headerTextBox.SelectAll();
            headerTextBox.Focus();
        }

        private void HeaderTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    SetHeaderReadOnly();

                    e.Handled = true;

                    break;

                case Key.Enter:

                    if (string.IsNullOrWhiteSpace(headerTextBox.Text))
                        SetHeaderReadOnly();
                    else
                        AcceptHeaderRenaming();

                    break;

            }
        }

        private void AcceptHeaderRenaming()
        {
            SetHeaderReadOnly();

            headerTextBlock.Text = headerTextBox.Text;

            RaiseFilterChanged();
        }

        private void SetHeaderReadOnly()
        {
            headerTextBox.Visibility = Visibility.Collapsed;
            headerContextMenu.Visibility = Visibility.Visible;
            headerTextBlock.Visibility = Visibility.Visible;
        }

        private void HeaderTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (headerTextBox.Visibility == Visibility.Visible)
                AcceptHeaderRenaming();
        }

        private void SaveHandler(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog();

            saveDialog.AddExtension = true;

            saveDialog.FileName = "{0}.html".Combine(Filter.Name);

            saveDialog.Filter = AuditStrings.HtmlFiles;

            saveDialog.Title = AuditStrings.SaveAudit;

            bool? result = saveDialog.ShowDialog(Window.GetWindow(this));

            if (true != result)
                return;

            waitForSaveText.Visibility = Visibility.Visible;

            SharedObjects.AsyncInvoker.Invoke(GetAndSaveToFile, saveDialog.FileName, Filter);
        }

        private void GetAndSaveToFile(string fileName, AuditFilter auditFilter)
        {
            try
            {
                using (var stream = File.Open(fileName, FileMode.Create))
                {
                    using (var writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        using (IAuditTools client = UiObjects.CreateAuditToolsClient())
                        {
                            var auditItems = client.GetItemsForFilter(auditFilter, -1);

                            string report = SharedObjects.ReportCreator.ExportAuditToHtml(auditItems, auditFilter);

                            writer.Write(report);
                        }
                    }
                }

                Process.Start(fileName);
            }
            catch (Exception ex)
            {
                Log.Write(ex);

                Dispatcher.BeginInvoke(new Action(() =>
                    {
                        waitForSaveText.Visibility = Visibility.Collapsed;

                        MessageBox.Show(
                            Window.GetWindow(this),
                            AdminViewResources.ErrorOccurredDuringSavingAuditDataTemplate.Combine(auditFilter.Name, Path.GetFileName(fileName), ex.GetExceptionMessage()),
                            CommonStrings.ProductName,
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                            );
                    }));
            }

            Dispatcher.BeginInvoke(new Action(() =>
                {
                    waitForSaveText.Visibility = Visibility.Collapsed;
                }));
        }

        private void RefreshHandler(object sender, RoutedEventArgs e)
        {
            UpdateItemsAsync();
        }


        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;

                UpdateItemsAsync();
            }
        }
    }
}
