using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Storage.Entities;

namespace LeakBlocker.Libraries.Storage.InternalTools
{
    partial class DatabaseModel
    {
        private static bool compatibilityWasChecked;
        private static bool providerInitialized;

        private static readonly Mutex mutex = new Mutex();

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")] //Connection is disposed by DbContext
        public DatabaseModel()
            : base(CreateConnection(), true)
        {
            mutex.WaitOne();

            InitializeSets();

            Configuration.AutoDetectChangesEnabled = true;
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;

            Configuration.ValidateOnSaveEnabled = false; 

            if (!Database.Exists())
            {
                Database.CreateIfNotExists();

                SaveChanges();
            }

            CheckCompability();
        }

        private void CheckCompability()
        {
            if (compatibilityWasChecked)
                return;

            if (!Database.CompatibleWithModel(true))
            {
#if DEBUG
                Database.Delete();
                Database.Create();
#else
                throw new InvalidOperationException("Database from file {0} has different version with the current code. Please contact technical support".Combine(StorageObjects.DatabaseConstants.DatabasePath));
#endif
            }

            compatibilityWasChecked = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(StorageObjects.IndexInitializer);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<DbCompositeRuleCondition>().HasMany(item => item.Conditions).WithMany();

            modelBuilder.Entity<DbComputerListRuleCondition>().HasMany(item => item.Computers).WithMany();
            modelBuilder.Entity<DbComputerListRuleCondition>().HasMany(item => item.Domains).WithMany();
            modelBuilder.Entity<DbComputerListRuleCondition>().HasMany(item => item.Groups).WithMany();
            modelBuilder.Entity<DbComputerListRuleCondition>().HasMany(item => item.OrganizationalUnits).WithMany();
            
            modelBuilder.Entity<DbDeviceListRuleCondition>().HasMany(item => item.Devices).WithMany();

            modelBuilder.Entity<DbUserListRuleCondition>().HasMany(item => item.Users).WithMany();
            modelBuilder.Entity<DbUserListRuleCondition>().HasMany(item => item.Domains).WithMany();
            modelBuilder.Entity<DbUserListRuleCondition>().HasMany(item => item.Groups).WithMany();
            modelBuilder.Entity<DbUserListRuleCondition>().HasMany(item => item.OrganizationalUnits).WithMany();
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string errorString =
                    "Save changes failed because of these errors: {0}"
                    .Combine(
                    string.Join(
                        ";",
                        ex.EntityValidationErrors.Select(
                            error =>
                                    "Entry: {0}, is valid: {1}; errors: {2}"
                                        .Combine(
                                         error.Entry,
                                         error.IsValid,
                                         string.Join(", ", error.ValidationErrors.Select(
                                            item => "Property: {0}; Error: {1}".Combine(item.PropertyName, item.ErrorMessage)))))));

                Log.Write(errorString);

                throw;
            }
        }

        private static DbConnection CreateConnection()
        {
            if (!providerInitialized)
            {
                try
                {
                    using (var dataSet = (DataSet)System.Configuration.ConfigurationManager.GetSection("system.data"))
                    {
                        const string name = "Microsoft SQL Server Compact Data Provider";
                        const string invariant = "System.Data.SqlServerCe.4.0";
                        const string description = ".NET Framework Data Provider for Microsoft SQL Server Compact";
                        const string type = "System.Data.SqlServerCe.SqlCeProviderFactory, System.Data.SqlServerCe, Version=4.0";

                        DataRowCollection rows = dataSet.Tables[0].Rows;

                        if (!rows.Contains(invariant))
                            rows.Add(name, description, invariant, type);
                    }
                }
                catch (ConstraintException)
                {
                }
                providerInitialized = true;
            }

            string folder = StorageObjects.DatabaseConstants.DatabaseFolder;
            string databaseFile = StorageObjects.DatabaseConstants.DatabasePath;
            string databaseName = StorageObjects.DatabaseConstants.DatabaseName;

            var connectionStringBuilder = new SqlConnectionStringBuilder();

            connectionStringBuilder.DataSource = databaseFile;
            connectionStringBuilder.Password = StorageObjects.DatabaseInitializer.DatabasePassword;

            var factory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0", folder, connectionStringBuilder.ToString());

            return factory.CreateConnection(databaseName);
        }
        
        private Func<Account, DbAccount> GetAccountFinder()
        {
            return account => DbAccount.OfRuntimeType(AccountSet, account)
                .FirstOrDefault(
                    otherAccount =>
                        otherAccount.Identifier.Value == account.Identifier.Value);
        }

        private Func<AccountSecurityIdentifier, DbAccountSecurityIdentifier> GetAccountSecurityIdentifierFinder()
        {
            return FindSecurityIdentifier;
        }

        private DbAccountSecurityIdentifier FindSecurityIdentifier(AccountSecurityIdentifier identifier)
        {
            return AccountSecurityIdentifierSet.FirstOrDefault(other => other.Value == identifier.Value);
        }

        private Func<OrganizationalUnit, DbOrganizationalUnit> GetOrganizationalUnitFinder()
        {
            return ou =>
                OrganizationalUnitSet.ToList().FirstOrDefault(
                    otherOu => string.Equals(otherOu.CanonicalName, ou.CanonicalName, StringComparison.OrdinalIgnoreCase));
        }

        private Func<Credentials, DbCredentials> GetCredentialsFinder()
        {
            return credentials =>
            {
                DbBaseDomainAccount domainAccount = DbBaseDomainAccount.ConvertFromBaseDomainAccount(credentials.Domain, this);
                return CredentialsSet.FirstOrDefault(item => item.Domain.Id == domainAccount.Id);
            };
        }

        private Func<DeviceDescription, DbDeviceDescription> GetDeviceDescriptionFinder()
        {
            return device => DeviceDescriptionSet.FirstOrDefault(dbDevice => dbDevice.Identifier == device.Identifier);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            mutex.ReleaseMutex();
        }

        private Func<AgentEncryptionData, DbAgentEncryptionData> GetAgentEncryptionDataFinder()
        {
            return data => AgentEncryptionDataSet.FirstOrDefault(item => item.Computer.Identifier.Value == data.Computer.Identifier.Value);
        }
    }
}
