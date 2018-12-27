using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Storage.Entities;
using LeakBlocker.Libraries.Storage.InternalTools;

namespace LeakBlocker.Libraries.Storage
{
    internal sealed class CredentialsManager : ICredentialsManager
    {
        public Credentials TryGetCredentials(BaseDomainAccount account)
        {
            Check.ObjectIsNotNull(account, "account");

            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                return TryGetCredentials(account, model);
            }
        }

        private static Credentials TryGetCredentials(BaseDomainAccount account, IDatabaseModel model)
        {
            DbBaseDomainAccount domainAccount = DbBaseDomainAccount.ConvertFromBaseDomainAccount(account, model);

            DbCredentials result = model.CredentialsSet.FirstOrDefault(dbCredentials => dbCredentials.Domain.Id == domainAccount.Id);

            if (result == null)
            {
                var domainComputer = account as DomainComputerAccount;

                if (domainComputer != null)
                    return TryGetCredentials(domainComputer.Parent, model);

                return null;
            }

            return result.GetCredentials();
        }

        public void UpdateCredentials(Credentials credentials)
        {
            Check.ObjectIsNotNull(credentials, "credentials");

            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                DbCredentials.ConvertFromCredentials(credentials, model);

                model.SaveChanges();
            }
        }

        public ReadOnlySet<Credentials> GetAllCredentials()
        {
            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                return model.CredentialsSet.ToList().Select(credentials => credentials.GetCredentials()).ToReadOnlySet();
            }
        }
    }
}
