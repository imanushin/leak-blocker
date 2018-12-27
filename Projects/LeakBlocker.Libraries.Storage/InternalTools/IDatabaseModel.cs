using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using LeakBlocker.Libraries.Storage.Entities;

namespace LeakBlocker.Libraries.Storage.InternalTools
{
    internal interface IDatabaseModel : IDisposable
    {

        DbSet<DbBaseRuleCondition> BaseRuleConditionSet
        {
            get;
        }

        DbSet<DbAuditItem> AuditItemSet
        {
            get;
        }

        DbSet<DbDeviceDescription> DeviceDescriptionSet
        {
            get;
        }

        DatabaseCache<DeviceDescription,DbDeviceDescription> DeviceDescriptionCache
        {
            get;
        }

        DbSet<DbAccount> AccountSet
        {
            get;
        }

        DatabaseCache<Account,DbAccount> AccountCache
        {
            get;
        }

        DbSet<DbAccountSecurityIdentifier> AccountSecurityIdentifierSet
        {
            get;
        }

        DatabaseCache<AccountSecurityIdentifier,DbAccountSecurityIdentifier> AccountSecurityIdentifierCache
        {
            get;
        }

        DbSet<DbAgentEncryptionData> AgentEncryptionDataSet
        {
            get;
        }

        DatabaseCache<AgentEncryptionData,DbAgentEncryptionData> AgentEncryptionDataCache
        {
            get;
        }

        DbSet<DbCredentials> CredentialsSet
        {
            get;
        }

        DatabaseCache<Credentials,DbCredentials> CredentialsCache
        {
            get;
        }

        DbSet<DbOrganizationalUnit> OrganizationalUnitSet
        {
            get;
        }

        DatabaseCache<OrganizationalUnit,DbOrganizationalUnit> OrganizationalUnitCache
        {
            get;
        }

        DbSet<DbProgramConfiguration> ProgramConfigurationSet
        {
            get;
        }

        DbSet<DbActionData> ActionDataSet
        {
            get;
        }

        DbSet<DbBaseTemporaryAccessCondition> BaseTemporaryAccessConditionSet
        {
            get;
        }

        DbSet<DbRule> RuleSet
        {
            get;
        }

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
    }
}
