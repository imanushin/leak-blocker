using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;

namespace LeakBlocker.Libraries.Common.Entities
{
    /// <summary>
    /// Базовый объект для всех Readonly классов.
    /// Класс может быть использован для сохранения в базу только если он наследует этот класс
    /// </summary>
    [Serializable]
    [DataContract(IsReference = true)]
    [KnownType(typeof(AuditItem))]
    [KnownType(typeof(Account))]
    [KnownType(typeof(DeviceDescription))]
    [KnownType(typeof(ProgramConfiguration))]
    [KnownType(typeof(OrganizationalUnit))]
    [KnownType(typeof(Rule))]
    [KnownType(typeof(BaseRuleCondition))]
    [KnownType(typeof(DeviceAccessMap))]
    public abstract class BaseEntity : BaseReadOnlyObject
    {
    }
}
