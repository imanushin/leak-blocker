using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using LeakBlocker.AdminView.Desktop.Controls.Standard;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Simple
{
    internal sealed partial class MainWindow
    {
        public MainWindow()
        {
            SharedObjects.AsyncInvoker.Invoke(UpdateConfiguration, true);
            UiObjects.UiConfigurationManager.ConfigurationChanged += ()=> UpdateConfiguration(false);

            InitializeComponent();
            Loaded += (sender, arguments) => LoadingStatus.TriggerLoadingCompleted();

            selectScope.SetBusy(true);
        }

        private void UpdateConfiguration(bool isFirstConfigurationGet)
        {
            SimpleConfiguration blockOptions;

            try
            {
                blockOptions = UiObjects.UiConfigurationManager.Configuration;
            }
            catch (Exception ex)
            {
                Log.Write(ex);

                Dispatcher.BeginInvoke(
                    new Action(() =>
                        {
                            MessageBox.Show(
                                this,
                                AdminViewResources.ExceptionInConfigurationLoagingTemplate.Combine(ex.GetExceptionMessage()),
                                CommonStrings.ProductName,
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                            Close();//Выходим из приложения, так как инизиализация провалилась
                        })
                    );
                
                return;

            }

            Dispatcher.BeginInvoke(
                new Action(() =>
                               {
                                   Check.ObjectIsNotNull(blockOptions);

                                   selectScope.UpdateConfiguration(blockOptions);
                                   selectScope.SetBusy(false);

                                   if (isFirstConfigurationGet && UiObjects.UiConfigurationManager.WasPreviousConfig)
                                       statusTab.IsSelected = true;
                               }));
        }

        private void OpenAboutDialog(object sender, RoutedEventArgs e)
        {
            new AboutDialog
            {
                Owner = this
            }.Show();
        }
    }
}
