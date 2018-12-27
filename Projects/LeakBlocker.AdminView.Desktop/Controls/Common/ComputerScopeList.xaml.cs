using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Controls.Common.AccountWindows;
using LeakBlocker.AdminView.Desktop.Controls.Common.ScopeSelections;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common
{
    internal sealed partial class ComputerScopeList
    {
        public ComputerScopeList()
        {
            InitializeComponent();
        }

        public string Title
        {
            get
            {
                return scopeList.Title;
            }
            set
            {
                Check.ObjectIsNotNull(value, "value");

                scopeList.Title = value;
            }
        }

        public ReadOnlySet<Scope> Scopes
        {
            get
            {
                return scopeList.Items.Cast<Scope>().ToReadOnlySet();
            }
            set
            {
                Check.ObjectIsNotNull(value, "value");

                scopeList.Items.Clear();

                AddScopes(value);
            }
        }

        private void AddItemClicked(object sender, RoutedEventArgs e)
        {
            ReadOnlySet<Scope> scopes = scopeList.Items.Cast<Scope>().ToReadOnlySet();

            Scope scope = ComputerScopeSelectionWindow.OpenSelectionWindow(scopes, Window.GetWindow(this));

            if (scope == null)
                return;

            scopeList.Items.Add(scope);

            UpdateComputersAsync();
        }

        private void RemoveClicked(object sender, RoutedEventArgs e)
        {
            UpdateComputersAsync();
        }

        private void UpdateComputersAsync()
        {
            ReadOnlySet<Scope> scopes = scopeList.Items.Cast<Scope>().ToReadOnlySet();

            SharedObjects.AsyncInvoker.Invoke(UpdateComputerList, scopes);
        }

        private void UpdateComputerList(ReadOnlySet<Scope> scopes)
        {
            using (IAccountTools client = UiObjects.CreateAccountToolsClient())
            {
                try
                {
                    ReadOnlySet<ResultComputer> computers = client.GetComputers(scopes);

                    Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Check.ObjectIsNotNull(ScopeEntryChanged);

                            ScopeEntryChanged(computers);
                        }));
                }
                catch (Exception ex)
                {
                    Log.Write(ex);

                    MessageBox.Show(
                        AdminViewResources.ErrorDuringScopeUpdating.Combine(ex.GetExceptionMessage()),
                        CommonStrings.ProductName,
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        public void AddScopes(IEnumerable<Scope> scopes)
        {
            scopes.ForEach(scope => scopeList.Items.Add(scope));
            UpdateComputersAsync();
        }

        public event Action<ReadOnlySet<ResultComputer>> ScopeEntryChanged;

        private void UpdateCredentialsClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = ((Scope)scopeList.SelectedItem).TargetObject;

            var domainItem = selectedItem as DomainAccount;
            var domainMemberItem = selectedItem as IDomainMember;
            var localComputer = selectedItem as LocalComputerAccount;

            BaseDomainAccount domainAccount;

            if (domainItem != null)
                domainAccount = domainItem;
            else if (domainMemberItem != null)
                domainAccount = domainMemberItem.Parent;
            else if (localComputer != null)
                domainAccount = localComputer;
            else
                throw new InvalidOperationException("Object with type {0} is not supported".Combine(selectedItem.GetType().Name));

            AskAndUpdateCredentialsWindow.OpenWindow(domainAccount, Window.GetWindow(this));
        }
    }
}

