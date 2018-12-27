using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Common.Collections;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.AccountWindows
{
    internal sealed partial class WaitScopeDataRetrieving : IDisposable
    {
        private readonly DomainUpdateRequest domainUpdateRequest;
        private IScheduler scheduler;
        private volatile bool requested;

        internal bool Result
        {
            get;
            private set;
        }

        private WaitScopeDataRetrieving(DomainUpdateRequest domainUpdateRequest)
        {
            this.domainUpdateRequest = domainUpdateRequest;
            InitializeComponent();

            text.Text = AdminViewResources.RetrievingInformationAbout.Combine(domainUpdateRequest.Domain);

            Loaded += WindowLoaded;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            scheduler = SharedObjects.CreateScheduler(UpdateDataForScope, TimeSpan.FromSeconds(1));
        }
        
        private void UpdateDataForScope()
        {
            if (requested)
                return;

            try
            {
                using (IAccountTools client = UiObjects.CreateAccountToolsClient())
                {
                    if (client.IsRequestCompleted(domainUpdateRequest))
                    {
                        requested = true;

                        CollectionContainer.AvailableComputerScopes.UpdateAsync(Dispatcher);
                        CollectionContainer.AvailableUserScopes.UpdateAsync(Dispatcher);

                        Dispatcher.BeginInvoke(new Action(() =>
                            {

                                Result = true;
                                Close();
                            }));
                    }
                }
            }
            catch (Exception exception)
            {
                requested = true;
                Log.Write(exception);
                MessageBox.Show(exception.GetExceptionMessage(), CommonStrings.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Close();
                }));
            }
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }


        public static bool ShowWindow(DomainUpdateRequest domainUpdateRequest, Window owner)
        {
            using (var windows = new WaitScopeDataRetrieving(domainUpdateRequest))
            {
                windows.Owner = owner;
                windows.ShowDialog();

                return windows.Result;
            }
        }

        public void Dispose()
        {
            if(scheduler != null)
                Disposable.DisposeSafe(scheduler);
        }
    }
}
