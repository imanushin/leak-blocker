using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation.LocalStorages
{
    internal sealed class ConfigurationStorage : BaseConfigurationManager<SimpleConfiguration>, IConfigurationStorage
    {
        private const string fileName = "simple.lbConfiguration";

        private static readonly ActionData unblockDisableAudit = new ActionData(BlockActionType.Unblock, AuditActionType.DisableAudit);
        private static readonly ActionData onlyUnblock = new ActionData(BlockActionType.Unblock, AuditActionType.Skip);

        private static class Priorities
        {
            public const int BlockScopeRulePriority = 1;
            public const int UnblockInputDevices = BlockScopeRulePriority + 1;
            public const int UnblockScopeRulePriority = UnblockInputDevices + 1;
            public const int UnblockUsersRulePriority = UnblockScopeRulePriority + 1;
            public const int UnblockDevicesRulePriority = UnblockUsersRulePriority + 1;
        }


        private static readonly SimpleConfiguration defaultConfiguration
            = new SimpleConfiguration(
                true,
                true,
                true,
                true,
                ReadOnlySet<Scope>.Empty,
                ReadOnlySet<Scope>.Empty,
                ReadOnlySet<DeviceDescription>.Empty,
                ReadOnlySet<Scope>.Empty,
                ReadOnlySet<BaseTemporaryAccessCondition>.Empty);

        private ProgramConfiguration currentFullConfiguration;

        public ConfigurationStorage()
            : base(fileName)
        {
        }

        public override void Save(SimpleConfiguration obj)
        {
            lock (SyncRoot)
            {
                base.Save(obj);

                currentFullConfiguration = CreateBaseObject(CurrentFullConfiguration.ConfigurationVersion + 1);

                StorageObjects.ConfigurationManager.SaveConfiguration(currentFullConfiguration);
            }
        }

        public SimpleConfiguration CurrentOrDefault
        {
            get
            {
                lock (SyncRoot)
                {
                    return Current ?? defaultConfiguration;
                }
            }
        }

        public ProgramConfiguration CurrentFullConfiguration
        {
            get
            {
                lock (SyncRoot)
                {

                    if (currentFullConfiguration != null)
                        return currentFullConfiguration;

                    ProgramConfiguration previous = StorageObjects.ConfigurationManager.GetLastConfiguration();

                    int version = previous == null ? 1 : previous.ConfigurationVersion;

                    currentFullConfiguration = CreateBaseObject(version);

                    return currentFullConfiguration;
                }
            }
        }

        /// <summary>
        /// Создает <see cref="ProgramConfiguration"/> из исходных настроек
        /// </summary>
        private ProgramConfiguration CreateBaseObject(int version)
        {
            IEnumerable<Rule> rules = new[] { 
                GetDevicesWhiteListRule(), 
                GetBlockedScopeRule(),
                GetUnblockedScopeRule(), 
                GetUsersWhiteListRule(),
                GetUnblockedInputDevicesRule() };

            rules = rules.SkipDefault();

            return new ProgramConfiguration(version, rules.ToReadOnlySet(), CurrentOrDefault.TemporaryAccess);
        }


        private Rule GetDevicesWhiteListRule()
        {
            if (!CurrentOrDefault.ExcludedDevices.Any())
                return null;

            var condition = new DeviceListRuleCondition(false, CurrentOrDefault.ExcludedDevices);

            return new Rule(
                ServerStrings.DevicesWhiteListRule,
                Priorities.UnblockDevicesRulePriority,
                condition,
                onlyUnblock);
        }

        private Rule GetUsersWhiteListRule()
        {
            if (!CurrentOrDefault.UsersWhiteList.Any())
                return null;

            UserListRuleCondition condition = UserListRuleCondition.CreateFromScopeList(false, CurrentOrDefault.UsersWhiteList.Select(scope => scope.TargetObject).ToReadOnlySet());

            return new Rule(
                ServerStrings.UsersWhiteListRule,
                Priorities.UnblockUsersRulePriority,
                condition,
                onlyUnblock);
        }

        private Rule GetUnblockedScopeRule()
        {
            if (!CurrentOrDefault.ExcludedScopes.Any())
                return null;

            ComputerListRuleCondition condition = ComputerListRuleCondition.CreateFromScopeList(false, CurrentOrDefault.ExcludedScopes.Select(scope => scope.TargetObject).ToReadOnlySet());

            return new Rule(
                ServerStrings.ExcludedComputersRule,
                Priorities.UnblockScopeRulePriority,
                condition,
                unblockDisableAudit);
        }


        private Rule GetUnblockedInputDevicesRule()
        {
            if (!CurrentOrDefault.AreInputDevicesAllowed)
                return null;

            var condition = new DeviceTypeRuleCondition(DeviceCategory.UserInput, false);

            return new Rule(
                ServerStrings.ExcludeInputDevicesRule,
                Priorities.UnblockInputDevices,
                condition,
                onlyUnblock);
        }
        private Rule GetBlockedScopeRule()
        {
            ComputerListRuleCondition condition = ComputerListRuleCondition.CreateFromScopeList(false, CurrentOrDefault.BlockedScopes.Select(scope => scope.TargetObject).ToReadOnlySet());

            BlockActionType fileBlock = CurrentOrDefault.IsReadOnlyAccessEnabled
                                                      ? BlockActionType.ReadOnly
                                                      : BlockActionType.Complete;

            BlockActionType blockType = CurrentOrDefault.IsBlockEnabled ? fileBlock : BlockActionType.Unblock;

            AuditActionType auditActionType = CurrentOrDefault.IsFileAuditEnabled ? AuditActionType.DeviceAndFiles : AuditActionType.Device;

            return new Rule(
                ServerStrings.BlockingRule,
                Priorities.BlockScopeRulePriority,
                condition,
                new ActionData(blockType, auditActionType));
        }
    }
}
