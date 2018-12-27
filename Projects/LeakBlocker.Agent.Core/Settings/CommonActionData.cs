using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;

namespace LeakBlocker.Agent.Core.Settings
{
    internal sealed class CommonActionData : BaseReadOnlyObject
    {
        public DeviceAccessType Access
        {
            get;
            private set;
        }

        public AuditActionType Audit
        {
            get;
            private set;
        }

        public CommonActionData(ActionData directData)
        {
            Check.ObjectIsNotNull(directData, "directData");

            Access = GetBlockAction(directData.BlockAction);
            Audit = directData.AuditAction;
        }

        public CommonActionData(DeviceAccessType access, AuditActionType audit)
        {
            Check.EnumerationValueIsDefined(access, "access");
            Check.EnumerationValueIsDefined(audit, "audit");

            Access = access;
            Audit = audit;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Access;
            yield return Audit;
        }

        private static DeviceAccessType GetBlockAction(BlockActionType accessAction)
        {
            switch (accessAction)
            {
                case BlockActionType.Unblock:
                case BlockActionType.Skip:
                    return DeviceAccessType.Allowed;

                case BlockActionType.Complete:
                    return DeviceAccessType.Blocked;
                case BlockActionType.ReadOnly:
                    return DeviceAccessType.ReadOnly;

                default:
                    Exceptions.Throw(ErrorMessage.EnumerationValueIncorrect);
                    return 0;
            }
        }
    }
}
