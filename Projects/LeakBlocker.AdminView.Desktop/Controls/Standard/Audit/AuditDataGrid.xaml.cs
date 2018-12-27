using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.Audit
{
    internal sealed partial class AuditDataGrid
    {
        private readonly MenuItem openWithSelectedComputersMenuItem;
        private readonly MenuItem filterBySelectedComputersMenuItem;

        private readonly MenuItem openWithSelectedUsersMenuItem;
        private readonly MenuItem filterBySelectedUsersMenuItem;

        private readonly MenuItem openWithSelectedDevicesMenuItem;
        private readonly MenuItem filterBySelectedDevicesMenuItem;

        private readonly MenuItem openWithSameCategoryMenuItem;
        private readonly MenuItem filterBySameCategoryMenuItem;

        private readonly ObservableCollection<AuditItem> innerCollectoion = new ObservableCollection<AuditItem>();
        private Comparison<AuditItem> currentSorter = (left, right) => Math.Sign(left.Time.Ticks - right.Time.Ticks);
        private int currentSortMultiplier = -1;

        public event Action<ReadOnlySet<AuditItemGroupType>> FilterByTypes;
        public event Action<ReadOnlySet<BaseComputerAccount>> FilterByComputers;
        public event Action<ReadOnlySet<BaseUserAccount>> FilterByUsers;
        public event Action<ReadOnlySet<DeviceDescription>> FilterByDevices;

        public AuditDataGrid()
        {
            InitializeComponent();

            innerGrid.ItemsSource = innerCollectoion;

            #region Fill Context Menu

            var cellContextMenu = (ContextMenu)Resources["GridContextMenu"];

            openWithSelectedComputersMenuItem = CreateMenuItem(OpenWithSelectedComputersHandler);
            filterBySelectedComputersMenuItem = CreateMenuItem(FilterBySelectedComputersHandler);
            cellContextMenu.Items.Add(openWithSelectedComputersMenuItem);
            cellContextMenu.Items.Add(filterBySelectedComputersMenuItem);

            cellContextMenu.Items.Add(new Separator());

            openWithSelectedUsersMenuItem = CreateMenuItem(OpenWithSelectedUsersHandler);
            filterBySelectedUsersMenuItem = CreateMenuItem(FilterBySelectedUsersHandler);
            cellContextMenu.Items.Add(openWithSelectedUsersMenuItem);
            cellContextMenu.Items.Add(filterBySelectedUsersMenuItem);

            cellContextMenu.Items.Add(new Separator());

            openWithSelectedDevicesMenuItem = CreateMenuItem(OpenWithSelectedDevicesHandler);
            filterBySelectedDevicesMenuItem = CreateMenuItem(FilterBySelectedDevicesHandler);
            cellContextMenu.Items.Add(openWithSelectedDevicesMenuItem);
            cellContextMenu.Items.Add(filterBySelectedDevicesMenuItem);

            cellContextMenu.Items.Add(new Separator());

            openWithSameCategoryMenuItem = CreateMenuItem(OpenSameAuditHandler);
            filterBySameCategoryMenuItem = CreateMenuItem(FilterByThisCategory);
            cellContextMenu.Items.Add(openWithSameCategoryMenuItem);
            cellContextMenu.Items.Add(filterBySameCategoryMenuItem);

            #endregion Fill Context Menu
        }

        public IEnumerable<AuditItem> Items
        {
            get
            {
                return innerCollectoion;
            }
            set
            {
                innerCollectoion.Clear();

                List<AuditItem> sortedItems = value.ToList();

                sortedItems.Sort((left, right) => currentSortMultiplier * currentSorter(left, right));
                
                innerCollectoion.AddRange(sortedItems);
            }
        }

        private IReadOnlyCollection<AuditItem> SelectedAuditItems
        {
            get
            {
                return innerGrid.SelectedItems.Cast<AuditItem>().ToReadOnlySet();
            }
        }

        private ReadOnlySet<AuditItemGroupType> GetSelectedGroupTypes()
        {
            ReadOnlySet<AuditItemType> selectedTypes = SelectedAuditItems.Select(item => item.EventType).ToReadOnlySet();

            var groupTypes = new List<AuditItemGroupType>();

            foreach (AuditItemGroupType auditItemGroupType in EnumHelper<AuditItemGroupType>.Values)
            {
                ReadOnlySet<AuditItemType> typesOfGroup = LinkedEnumHelper<AuditItemType, AuditItemGroupType>.GetAllLinkedEnums(auditItemGroupType);

                if (typesOfGroup.Intersect(selectedTypes).Any())
                    groupTypes.Add(auditItemGroupType);
            }

            return groupTypes.ToReadOnlySet();
        }

        #region Context Menu Events


        private void ContextMenuOpened(object sender, RoutedEventArgs e)
        {
            ComputersMenuOpenedHanlder();
            UsersMenuOpenedHanlder();
            DevicesMenuOpenedHanlder();
            DescriptionMenuOpenedHanlder();
        }

        private void ComputersMenuOpenedHanlder()
        {
            ReadOnlySet<BaseComputerAccount> computers = SelectedAuditItems.Select(item => item.Computer).SkipDefault().ToReadOnlySet();

            string templateParameter;

            if (computers.Count > 2)
                templateParameter = computers.Count.ToString(CultureInfo.InvariantCulture);
            else
                templateParameter = string.Join(", ", computers.Select(computer => computer.ShortName));

            openWithSelectedComputersMenuItem.Header = AuditStrings.OpenWithSelectedComputersTemplate.Combine(templateParameter);
            filterBySelectedComputersMenuItem.Header = AuditStrings.FilterBySelectedComputersTemplate.Combine(templateParameter);
        }

        private void UsersMenuOpenedHanlder()
        {
            ReadOnlySet<BaseUserAccount> users = SelectedAuditItems.Select(item => item.User).SkipDefault().ToReadOnlySet();

            string templateParameter;

            if (users.Count == 0)
                templateParameter = AuditStrings.NoUsersSelected;
            else if (users.Count > 2)
                templateParameter = users.Count.ToString(CultureInfo.InvariantCulture);
            else
                templateParameter = string.Join(", ", users.Select(user => user.ShortName));

            openWithSelectedUsersMenuItem.Header = AuditStrings.OpenWithSelectedUsersTemplate.Combine(templateParameter);
            filterBySelectedUsersMenuItem.Header = AuditStrings.FilterBySelectedUsersTemplate.Combine(templateParameter);

            openWithSelectedUsersMenuItem.IsEnabled = users.Any();
            filterBySelectedUsersMenuItem.IsEnabled = users.Any();
        }

        private void DevicesMenuOpenedHanlder()
        {
            ReadOnlySet<DeviceDescription> devices = SelectedAuditItems.Select(item => item.Device).SkipDefault().ToReadOnlySet();

            string templateParameter;

            if (devices.Count == 0)
                templateParameter = AuditStrings.NoDevicesSelected;
            else if (devices.Count > 2)
                templateParameter = devices.Count.ToString(CultureInfo.InvariantCulture);
            else
                templateParameter = string.Join(", ", devices.Select(device => device.FriendlyName));

            openWithSelectedDevicesMenuItem.Header = AuditStrings.OpenWithSelectedDevicesTemplate.Combine(templateParameter);
            filterBySelectedDevicesMenuItem.Header = AuditStrings.FilterBySelectedDevicesTemplate.Combine(templateParameter);

            openWithSelectedDevicesMenuItem.IsEnabled = devices.Any();
            filterBySelectedDevicesMenuItem.IsEnabled = devices.Any();
        }

        private void DescriptionMenuOpenedHanlder()
        {
            ReadOnlySet<AuditItemGroupType> auditGroups = SelectedAuditItems
                .Select(item => LinkedEnumHelper<AuditItemType, AuditItemGroupType>.GetLinkedEnum(item.EventType)).ToReadOnlySet();

            string groupsString = string.Join(", ", auditGroups.Select(item => item.GetValueDescription()));

            openWithSameCategoryMenuItem.Header = AuditStrings.OpenSameAuditTemplate.Combine(groupsString);
            filterBySameCategoryMenuItem.Header = AuditStrings.FilterByThisCategoryTemplate.Combine(groupsString);
        }

        private void OpenSameAuditHandler()
        {
            var groupTypes = GetSelectedGroupTypes();

            if (!groupTypes.Any())
                return;

            App.AuditManager.OpenTypes(groupTypes);
        }

        private void FilterByThisCategory()
        {
            var groupTypes = GetSelectedGroupTypes();

            if (!groupTypes.Any())
                return;

            Check.ObjectIsNotNull(FilterByTypes);

            FilterByTypes(groupTypes);
        }

        private void FilterBySelectedComputersHandler()
        {
            Check.ObjectIsNotNull(FilterByComputers);

            FilterByComputers(SelectedAuditItems.Select(item => item.Computer).ToReadOnlySet());
        }

        private void OpenWithSelectedComputersHandler()
        {
            App.AuditManager.OpenComputersAudit(SelectedAuditItems.Select(item => item.Computer).ToReadOnlySet());
        }

        private void FilterBySelectedUsersHandler()
        {
            Check.ObjectIsNotNull(FilterByUsers);

            FilterByUsers(SelectedAuditItems.Select(item => item.User).Where(user => user != null).ToReadOnlySet());
        }

        private void OpenWithSelectedUsersHandler()
        {
            App.AuditManager.OpenUsersAudit(SelectedAuditItems.Select(item => item.User).Where(user => user != null).ToReadOnlySet());
        }

        private void FilterBySelectedDevicesHandler()
        {
            Check.ObjectIsNotNull(FilterByDevices);

            FilterByDevices(SelectedAuditItems.Select(item => item.Device).Where(device => device != null).ToReadOnlySet());
        }

        private void OpenWithSelectedDevicesHandler()
        {
            App.AuditManager.OpenDevicesAudit(SelectedAuditItems.Select(item => item.Device).Where(device => device != null).ToReadOnlySet());
        }

        #endregion Context Menu Events

        private static MenuItem CreateMenuItem(Action action)
        {
            var result = new AuditMenuItem();

            result.Click += (source, args) => action();

            return result;
        }

        private void SortingHandler(object sender, DataGridSortingEventArgs e)
        {
            DataGridColumn targetColumn = e.Column;

            ListSortDirection direction = targetColumn.SortDirection ?? ListSortDirection.Descending;

            currentSortMultiplier = direction == ListSortDirection.Descending ? 1 : -1;

            List<AuditItem> sortedItems = Items.ToList();

            currentSorter = GetSorter(targetColumn);

            sortedItems.Sort((left, right) => currentSortMultiplier * currentSorter(left, right));

            innerCollectoion.Clear();
            innerCollectoion.AddRange(sortedItems);

            targetColumn.SortDirection = direction == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

            e.Handled = true;
        }

        private static Comparison<AuditItem> GetSorter(DataGridColumn targetColumn)
        {
            switch (targetColumn.SortMemberPath)
            {
                case "EventType":
                    return (left, rigth) =>
                             LinkedEnumHelper<AuditItemType, AuditItemSeverityType>.GetLinkedEnum(left.EventType) -
                             LinkedEnumHelper<AuditItemType, AuditItemSeverityType>.GetLinkedEnum(rigth.EventType);

                case "Description":
                    return (left, rigth) => string.CompareOrdinal(left.ToString(), rigth.ToString());

                case "Time":
                    return (left, rigth) => left.CompareTo(rigth);
                    
                case "Computer":
                    return (left, rigth) => Comparer<BaseComputerAccount>.Default.Compare(left.Computer, rigth.Computer);

                case "User":
                    return (left, rigth) => Comparer<BaseUserAccount>.Default.Compare(left.User, rigth.User);

                case "Device":
                    return (left, rigth) => Comparer<DeviceDescription>.Default.Compare(left.Device, rigth.Device);

                default:
                    throw new InvalidOperationException("Unknown sort member path: {0}".Combine(targetColumn.SortMemberPath));
            }
        }
    }
}
