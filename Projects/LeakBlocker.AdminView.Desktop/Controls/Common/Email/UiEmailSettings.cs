using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Email
{
    internal sealed class UiEmailSettings : INotifyPropertyChanged
    {
        public void Load(EmailSettings emailSettings)
        {
            From = emailSettings.From;
            To = emailSettings.To;
            SmtpHost = emailSettings.SmtpHost;
            UseSslConnection = emailSettings.UseSslConnection;
            IsAuthenticationEnabled = emailSettings.IsAuthenticationEnabled;
            UserName = emailSettings.UserName;
            SmtpPort = emailSettings.Port;

            OnPropertyChanged("UseSslConnection");
            OnPropertyChanged("SmtpPort");
            OnPropertyChanged("From");
            OnPropertyChanged("To");
            OnPropertyChanged("SmtpHost");
            OnPropertyChanged("IsAuthenticationEnabled");
            OnPropertyChanged("UserName");
        }

        public string From
        {
            get;
            set;
        }

        public string To
        {
            get;
            set;
        }

        public string SmtpHost
        {
            get;
            set;
        }

        public int SmtpPort
        {
            get;
            set;
        }

        public bool UseSslConnection
        {
            get;
            set;
        }

        public bool IsAuthenticationEnabled
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Error
        {
            get
            {
                if (string.IsNullOrWhiteSpace(From))
                    return AdminViewResources.PleaseEnterTheFromAddress;

                if (string.IsNullOrWhiteSpace(To))
                    return AdminViewResources.PleaseEnterTheToAddress;

                if (string.IsNullOrWhiteSpace(SmtpHost))
                    return AdminViewResources.PleaseEnterTheSmtpHost;

                if (IsAuthenticationEnabled && string.IsNullOrWhiteSpace(UserName))
                    return AdminViewResources.PleaseEnterTheUserName;
                
                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
