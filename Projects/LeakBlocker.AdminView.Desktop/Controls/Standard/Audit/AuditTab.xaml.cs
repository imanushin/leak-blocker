using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.Audit
{
    internal sealed partial class AuditTab
    {
        private readonly UiAuditManager auditManager = new UiAuditManager();

        public AuditTab()
        {
            InitializeComponent();

            auditManager.ComputerAuditRequested += ComputerAuditRequested;
            auditManager.UserAuditRequested += UserAuditRequested;
            auditManager.DeviceAuditRequested += DeviceAuditRequested;
            auditManager.TypedAuditRequested += TypedAuditRequested;

            App.RegisterAuditManager(auditManager);

            SharedObjects.AsyncInvoker.Invoke(UpdateFilters);

            filters.AddTabClicked += AddTabClickedHandler;

            noItemsText.FontSize *= 1.5;
        }

        private void AddTabClickedHandler()
        {
            var filter = new AuditFilter(
                CreateTabTitle(AuditStrings.CommonAuditPreffix), 
                ReadOnlySet<BaseComputerAccount>.Empty,
                ReadOnlySet<BaseUserAccount>.Empty, 
                ReadOnlySet<DeviceDescription>.Empty, 
                Time.Unknown,
                Time.Unknown, 
                false, 
                ReadOnlySet<AuditItemGroupType>.Empty);

            AddTab(filter);

            SharedObjects.AsyncInvoker.Invoke(AddFilter, filter);
        }

        private void UpdateFilters()
        {
            using (IAuditTools client = UiObjects.CreateAuditToolsClient())
            {
                ReadOnlyList<AuditFilter> loadedFilters = client.LoadFilters();

                Dispatcher.BeginInvoke(
                    new Action(() =>
                          {
                              filters.Items.Clear();

                              foreach (AuditFilter filter in loadedFilters)
                              {
                                  AddTab(filter, false);
                              }

                              busyIndicator.IsBusy = false;
                          }));
            }
        }

        private static void AddFilter(AuditFilter newFilter)
        {
            ChangeFilter(null, newFilter);
        }

        private static void ChangeFilterAsync(AuditFilter oldFilter, AuditFilter newFilter)
        {
            SharedObjects.AsyncInvoker.Invoke(ChangeFilter, oldFilter, newFilter);
        }

        private void RemoveFilterClicked(AuditFilterTab tab)
        {
            filters.Items.Remove(tab);

            if (filters.Items.Count == 0)
                noItemsText.Visibility = Visibility.Visible;

            SharedObjects.AsyncInvoker.Invoke(RemoveFilter, tab.Filter);
        }

        private static void ChangeFilter(AuditFilter from, AuditFilter to)
        {
            if(from == null && to == null)
                throw new ArgumentNullException(@"One of the arguments should not be null");

            using (IAuditTools client = UiObjects.CreateAuditToolsClient())
            {
                if (from == null)
                    client.CreateFilter(to);
                else if (to == null)
                    client.DeleteFilter(from);
                else
                    client.ChangeFilter(from, to);
            }
        }

        private static void RemoveFilter(AuditFilter filter)
        {
            ChangeFilter(filter, null);
        }

        private void ComputerAuditRequested(ICollection<BaseComputerAccount> computers)
        {
            var filter = new AuditFilter(
                CreateNameByObjects(computers),
                computers.ToReadOnlySet(),
                ReadOnlySet<BaseUserAccount>.Empty,
                ReadOnlySet<DeviceDescription>.Empty,
                Time.Unknown,
                Time.Unknown,
                false,
                ReadOnlySet<AuditItemGroupType>.Empty);

            SharedObjects.AsyncInvoker.Invoke(AddFilter, filter);

            AddTab(filter);
        }

        private void UserAuditRequested(ICollection<BaseUserAccount> users)
        {
            var filter = new AuditFilter(
                CreateNameByObjects(users),
                ReadOnlySet<BaseComputerAccount>.Empty,
                users.ToReadOnlySet(),
                ReadOnlySet<DeviceDescription>.Empty,
                Time.Unknown,
                Time.Unknown,
                false,
                ReadOnlySet<AuditItemGroupType>.Empty);

            SharedObjects.AsyncInvoker.Invoke(AddFilter, filter);

            AddTab(filter);
        }

        private void DeviceAuditRequested(ICollection<DeviceDescription> devices)
        {
            var filter = new AuditFilter(
                CreateTabTitle(string.Join(", ", devices.Select(device => device.FriendlyName))),
                ReadOnlySet<BaseComputerAccount>.Empty,
                ReadOnlySet<BaseUserAccount>.Empty,
                devices.ToReadOnlySet(),
                Time.Unknown,
                Time.Unknown,
                false,
                ReadOnlySet<AuditItemGroupType>.Empty);

            SharedObjects.AsyncInvoker.Invoke(AddFilter, filter);

            AddTab(filter);
        }


        private void TypedAuditRequested(ICollection<AuditItemGroupType> types)
        {
            var filter = new AuditFilter(
                CreateTabTitle(AuditStrings.CommonAuditPreffix),
                ReadOnlySet<BaseComputerAccount>.Empty,
                ReadOnlySet<BaseUserAccount>.Empty,
                ReadOnlySet<DeviceDescription>.Empty,
                Time.Unknown,
                Time.Unknown,
                false,
                types.ToReadOnlySet());

            SharedObjects.AsyncInvoker.Invoke(AddFilter, filter);

            AddTab(filter);
        }

        private static string CreateNameByObjects(IEnumerable<Account> accounts)
        {
            return string.Join(", ", accounts.Select(account => account.ShortName));
        }

        private string CreateTabTitle(string preffix)
        {
            if (!NameExists(preffix))
                return preffix;

            foreach (int suffix in Enumerable.Range(1, int.MaxValue))
            {
                string name = preffix + "_" + suffix;

                if (!NameExists(name))
                    return name;
            }

            throw new InvalidOperationException("Unable to create tab title");
        }

        private bool NameExists(string preffix)
        {
            return filters.Items.Cast<AuditFilterTab>().Select(tab => tab.Filter.Name).Contains(preffix);
        }

        private void AddTab(AuditFilter filter, bool focus = true)
        {
            var tab = new AuditFilterTab(filter);

            tab.RemoveClicked += () => RemoveFilterClicked(tab);
            tab.FilterChanged += oldFilter => ChangeFilterAsync(oldFilter, tab.Filter);

            if (focus)
                Focus();

            filters.Items.Add(tab);

            noItemsText.Visibility = Visibility.Collapsed;

            if (focus)
                filters.SelectedItem = tab;
        }
    }
}
