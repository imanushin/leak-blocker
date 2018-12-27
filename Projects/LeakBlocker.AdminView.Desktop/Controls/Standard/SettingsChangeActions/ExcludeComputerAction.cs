using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.SettingsChangeActions
{
    internal sealed class ExcludeComputerAction : BaseChangeAction
    {
        private readonly BaseComputerAccount computerToExclude;

        public ExcludeComputerAction(BaseComputerAccount computerToExclude)
        {
            Check.ObjectIsNotNull(computerToExclude, "computerToExclude");
            this.computerToExclude = computerToExclude;
        }

        public BaseComputerAccount Computer
        {
            get
            {
                return computerToExclude;
            }
        }

        public override string ShortText
        {
            get
            {
                return ToString();
            }
        }

        public override SimpleConfiguration AddSettings(SimpleConfiguration originalConfiguration)
        {
            IEnumerable<Scope> excludedScopes = originalConfiguration.ExcludedScopes.Union(new[] { new Scope(computerToExclude) });

            return originalConfiguration.GetFromCurrent(excludedScopes: excludedScopes);
        }

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return computerToExclude;
        }

        protected override string GetString()
        {
            return AdminViewResources.ExcludeComputerTemplate.Combine(computerToExclude);
        }
    }
}
