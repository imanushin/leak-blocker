using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.AdminView.Desktop.Common
{
    internal sealed class UiComputer : BaseReadOnlyObject
    {
        public UiComputer(BaseComputerAccount account)
        {
            Check.ObjectIsNotNull(account, "account");

            Name = account.ShortName;
            DnsName = account.FullName;
            BaseComputer = account;
        }

        public string Name
        {
            get;
            private set;
        }

        public string DnsName
        {
            get;
            private set;
        }

        public BaseComputerAccount BaseComputer
        {
            get;
            private set;
        }

        protected override string GetString()
        {
            return DnsName;
        }

        #region Equality 

        protected override int CustomCompare(object target)
        {
            return string.CompareOrdinal(DnsName, ((UiComputer)target).DnsName);
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return BaseComputer;
        }

        #endregion
    }
}
