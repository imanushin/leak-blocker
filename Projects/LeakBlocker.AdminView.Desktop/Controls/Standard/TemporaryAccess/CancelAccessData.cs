using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.TemporaryAccess
{
    internal sealed class CancelAccessData : INotifyPropertyChanged
    {
        private bool isCancelled;

        public CancelAccessData(BaseTemporaryAccessCondition condition, bool isCancelled)
        {
            Condition = condition;
            IsCancelled = isCancelled;
        }

        public bool IsCancelled
        {
            get
            {
                return isCancelled;
            }
            set
            {
                isCancelled = value;

                OnPropertyChanged("IsCancelled");
            }
        }

        public BaseTemporaryAccessCondition Condition
        {
            get;
            private set;
        }

        public override int GetHashCode()
        {
            return Condition.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as CancelAccessData;

            if (other == null)
                return false;

            return other.Condition == Condition;
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
