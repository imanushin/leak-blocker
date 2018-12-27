using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.AdminView.Desktop.Controls.Common.Converters;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Standard.SettingsChangeActions
{
    internal sealed class CancelTemporaryAccessAction : BaseChangeAction
    {
        public CancelTemporaryAccessAction(BaseTemporaryAccessCondition condition)
        {
            Check.ObjectIsNotNull(condition, "condition");

            Condition = condition;
        }

        public BaseTemporaryAccessCondition Condition
        {
            get;
            private set;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Condition;
        }

        public override string ShortText
        {
            get
            {
                var computerCondition = Condition as ComputerTemporaryAccessCondition;
                var userCondition = Condition as UserTemporaryAccessCondition;
                var deviceCondition = Condition as DeviceTemporaryAccessCondition;

                string objectTypeString;
                string objectString;

                if (computerCondition != null)
                {
                    objectTypeString = TemporaryAccessStrings.Computer;
                    objectString = computerCondition.Computer.FullName;
                }
                else if (userCondition != null)
                {
                    objectTypeString = TemporaryAccessStrings.User;
                    objectString = userCondition.User.FullName;
                }
                else if (deviceCondition != null)
                {
                    objectTypeString = TemporaryAccessStrings.Device;
                    objectString = deviceCondition.Device.FriendlyName;
                }
                else
                    throw new InvalidOperationException("Unable to determine the target object");

                return TemporaryAccessStrings.ShortCancelAccessTo.Combine(objectTypeString, objectString);
            }
        }

        public override SimpleConfiguration AddSettings(SimpleConfiguration originalConfiguration)
        {
            return originalConfiguration
                .GetFromCurrent(temporaryAccess: originalConfiguration.TemporaryAccess.Without(new[] { Condition }));
        }


        protected override string GetString()
        {
            return CancelTemporaryAccessConditionStringConverter.ConvertCondition(Condition);
        }
    }
}
