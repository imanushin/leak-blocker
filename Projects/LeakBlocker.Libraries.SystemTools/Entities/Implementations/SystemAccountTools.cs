using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Principal;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Entities.Management;
using LeakBlocker.Libraries.SystemTools.Win32;

namespace LeakBlocker.Libraries.SystemTools.Entities.Implementations
{
    internal sealed class SystemAccountTools : ISystemAccountTools
    {
        private BaseComputerAccount localComputer;

        public BaseUserAccount GetUserByIdentifier(AccountSecurityIdentifier identifier)
        {
            return AccountTools.GetLocallyAvailableUserByIdentifier(identifier);
        }

        public BaseDomainAccount GetBaseDomainAccountByName(string name, SystemAccessOptions options = default(SystemAccessOptions))
        {
            using (new ExceptionNotifier("Querying base domain account {0}. Access options: {1}.", name, options))
            {
                return AccountTools.GetBaseDomainAccountByName(name, options);
            }
        }

        public BaseComputerAccount LocalComputer
        {
            get
            {
                if (localComputer != null)
                    return localComputer;

                try
                {
                    localComputer = AccountTools.GetLocalComputer();//Сохраняем в кеш только тогда, когда resolve прошёл успешно

                    return localComputer;
                }
                catch (Exception exception)
                {
                    Log.Write("Unable to get local computer: " + exception);

                    return AccountTools.GetLocalComputerOffline();
                }
            }
        }

        public UserContactInformation GetUserContactInformation(DomainUserAccount user, Credentials credentials)
        {
            Check.ObjectIsNotNull(user, "user");
            Check.ObjectIsNotNull(credentials, "credentials");

            string filter = "(&(objectClass=user)(objectSid={0}))".Combine(user.Identifier);

            using (var domainEntry = new DirectoryEntry("LDAP://" + user.Parent.FullName, credentials.User, credentials.Password))
            using (var directorySearcher = new DirectorySearcher(domainEntry, filter))
            {
                SearchResult result = directorySearcher.FindOne();
                if (result == null)
                    Exceptions.Throw(ErrorMessage.NotFound, "User {0} was not found.".Combine(user));

                using (DirectoryEntry resultEntry = result.GetDirectoryEntry())
                {
                    return new UserContactInformation(
                        (string)resultEntry.Properties["givenname"].Value ?? string.Empty,
                        (string)resultEntry.Properties["sn"].Value ?? string.Empty,
                        (string)resultEntry.Properties["company"].Value ?? string.Empty,
                        (string)resultEntry.Properties["mail"].Value ?? string.Empty,
                        (string)resultEntry.Properties["telephonenumber"].Value ?? string.Empty);
                }
            }
        }
    }
}
