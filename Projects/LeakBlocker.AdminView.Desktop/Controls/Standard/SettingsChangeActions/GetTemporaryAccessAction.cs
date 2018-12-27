using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.SettingsChangeActions
{
    internal sealed class GetTemporaryAccessAction : BaseChangeAction
    {
        private readonly BaseTemporaryAccessCondition baseTemporaryAccessCondition;

        public GetTemporaryAccessAction(BaseTemporaryAccessCondition baseTemporaryAccessCondition)
        {
            Check.ObjectIsNotNull(baseTemporaryAccessCondition, "baseTemporaryAccessCondition");

            this.baseTemporaryAccessCondition = baseTemporaryAccessCondition;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return baseTemporaryAccessCondition;
        }

        public override string ShortText
        {
            get
            {
                var computerCondition = baseTemporaryAccessCondition as ComputerTemporaryAccessCondition;
                var userCondition = baseTemporaryAccessCondition as UserTemporaryAccessCondition;
                var deviceCondition = baseTemporaryAccessCondition as DeviceTemporaryAccessCondition;

                if (computerCondition != null)
                    return TemporaryAccessStrings.ShortAccessToComputer.Combine(computerCondition.Computer);

                if (userCondition != null)
                    return TemporaryAccessStrings.ShortAccessToUser.Combine(userCondition.User);

                if (deviceCondition != null)
                    return TemporaryAccessStrings.ShortAccessToDevice.Combine(deviceCondition.Device);

                throw new InvalidOperationException("Unable to generate short text for condition {0}".Combine(baseTemporaryAccessCondition));
            }
        }

        public override SimpleConfiguration AddSettings(SimpleConfiguration originalConfiguration)
        {
            IEnumerable<BaseTemporaryAccessCondition> newConditions = originalConfiguration.TemporaryAccess.Union(new[] { baseTemporaryAccessCondition });

            return originalConfiguration.GetFromCurrent(temporaryAccess: newConditions);
        }

        protected override string GetString()
        {
            string originalString = baseTemporaryAccessCondition.ToString();

            string afterReplacing = originalString.Replace(". ", "." + Environment.NewLine);

            return TemporaryAccessStrings.CancelledCondition.Combine(afterReplacing);
        }
    }
}
