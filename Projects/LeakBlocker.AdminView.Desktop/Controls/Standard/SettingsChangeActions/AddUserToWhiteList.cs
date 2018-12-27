using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.SettingsChangeActions
{
    internal sealed class AddUserToWhiteList : BaseChangeAction
    {
        private readonly BaseUserAccount user;

        public AddUserToWhiteList(BaseUserAccount user)
        {
            Check.ObjectIsNotNull(user, "user");

            this.user = user;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return user;
        }

        public override string ShortText
        {
            get
            {
                return AdminViewResources.AddUserToTheWhiteListTemplate.Combine(user.ShortName);
            }
        }

        public override SimpleConfiguration AddSettings(SimpleConfiguration originalConfiguration)
        {
            return originalConfiguration.GetFromCurrent(usersWhiteList: originalConfiguration.UsersWhiteList.UnionWith(new Scope(user)));
        }

        protected override string GetString()
        {
            return AdminViewResources.AddUserToTheWhiteListTemplate.Combine(user.FullName); 
        }
    }
}
