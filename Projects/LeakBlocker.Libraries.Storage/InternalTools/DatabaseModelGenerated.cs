using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Storage.Entities;

namespace LeakBlocker.Libraries.Storage.InternalTools
{
    internal sealed partial class DatabaseModel : DbContext, IDatabaseModel
    {
        private bool areSetsInitialized = false;

        private void InitializeSets()
        {
            if( areSetsInitialized )
                throw new InvalidOperationException( "Sets had already been initialized" );

            BaseRuleConditionSet = Set<DbBaseRuleCondition>();
            AuditItemSet = Set<DbAuditItem>();
            DeviceDescriptionSet = Set<DbDeviceDescription>();
            DeviceDescriptionCache = new DatabaseCache<DeviceDescription,DbDeviceDescription>(GetDeviceDescriptionFinder());
            AccountSet = Set<DbAccount>();
            AccountCache = new DatabaseCache<Account,DbAccount>(GetAccountFinder());
            AccountSecurityIdentifierSet = Set<DbAccountSecurityIdentifier>();
            AccountSecurityIdentifierCache = new DatabaseCache<AccountSecurityIdentifier,DbAccountSecurityIdentifier>(GetAccountSecurityIdentifierFinder());
            AgentEncryptionDataSet = Set<DbAgentEncryptionData>();
            AgentEncryptionDataCache = new DatabaseCache<AgentEncryptionData,DbAgentEncryptionData>(GetAgentEncryptionDataFinder());
            CredentialsSet = Set<DbCredentials>();
            CredentialsCache = new DatabaseCache<Credentials,DbCredentials>(GetCredentialsFinder());
            OrganizationalUnitSet = Set<DbOrganizationalUnit>();
            OrganizationalUnitCache = new DatabaseCache<OrganizationalUnit,DbOrganizationalUnit>(GetOrganizationalUnitFinder());
            ProgramConfigurationSet = Set<DbProgramConfiguration>();
            ActionDataSet = Set<DbActionData>();
            BaseTemporaryAccessConditionSet = Set<DbBaseTemporaryAccessCondition>();
            RuleSet = Set<DbRule>();

            areSetsInitialized = true;
        }

        public DbSet<DbBaseRuleCondition> BaseRuleConditionSet
        {
            get;
            private set;
        }

        public DbSet<DbAuditItem> AuditItemSet
        {
            get;
            private set;
        }

        public DbSet<DbDeviceDescription> DeviceDescriptionSet
        {
            get;
            private set;
        }

        public DatabaseCache<DeviceDescription,DbDeviceDescription> DeviceDescriptionCache
        {
            get;
            private set;
        }

        public DbSet<DbAccount> AccountSet
        {
            get;
            private set;
        }

        public DatabaseCache<Account,DbAccount> AccountCache
        {
            get;
            private set;
        }

        public DbSet<DbAccountSecurityIdentifier> AccountSecurityIdentifierSet
        {
            get;
            private set;
        }

        public DatabaseCache<AccountSecurityIdentifier,DbAccountSecurityIdentifier> AccountSecurityIdentifierCache
        {
            get;
            private set;
        }

        public DbSet<DbAgentEncryptionData> AgentEncryptionDataSet
        {
            get;
            private set;
        }

        public DatabaseCache<AgentEncryptionData,DbAgentEncryptionData> AgentEncryptionDataCache
        {
            get;
            private set;
        }

        public DbSet<DbCredentials> CredentialsSet
        {
            get;
            private set;
        }

        public DatabaseCache<Credentials,DbCredentials> CredentialsCache
        {
            get;
            private set;
        }

        public DbSet<DbOrganizationalUnit> OrganizationalUnitSet
        {
            get;
            private set;
        }

        public DatabaseCache<OrganizationalUnit,DbOrganizationalUnit> OrganizationalUnitCache
        {
            get;
            private set;
        }

        public DbSet<DbProgramConfiguration> ProgramConfigurationSet
        {
            get;
            private set;
        }

        public DbSet<DbActionData> ActionDataSet
        {
            get;
            private set;
        }

        public DbSet<DbBaseTemporaryAccessCondition> BaseTemporaryAccessConditionSet
        {
            get;
            private set;
        }

        public DbSet<DbRule> RuleSet
        {
            get;
            private set;
        }

    }
}
