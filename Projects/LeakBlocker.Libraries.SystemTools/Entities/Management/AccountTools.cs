using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.SystemTools.Entities.Management
{
    internal static class AccountTools
    {
        internal static AccountSecurityIdentifier GetParentIdentifier(this AccountSecurityIdentifier identifier)
        {
            Check.ObjectIsNotNull(identifier, "identifier");

            var currentIdentifier = new SecurityIdentifier(identifier.Value);
            if ((currentIdentifier.AccountDomainSid == null) || (currentIdentifier.AccountDomainSid == currentIdentifier))
                return null;

            return new AccountSecurityIdentifier(currentIdentifier.AccountDomainSid);
        }

        internal static BaseComputerAccount GetLocalComputer()
        {
            return DirectoryServicesProvider.GetComputerByName(ComputerInformation.GetCurrentComputerName()).Key;
        }

        // Must not require any network connection
        internal static BaseComputerAccount GetLocalComputerOffline()
        {
            string name = ComputerInformation.GetCurrentComputerName();
            string domainName = ComputerInformation.GetComputerDomainName();

            if (domainName != null)
            {
                string identifier = OfflineDomainInformation.GetCurrentComputerDomainIdentifier();
                string canonicalName = OfflineDomainInformation.GetCurrentComputerCanonocalName();

                if (identifier != null)
                {
                    string domainIdentifier = ComputerInformation.GetComputerParentDomainIdentifier();

                    var domain = new DomainAccount(domainName, new AccountSecurityIdentifier(domainIdentifier), ComputerInformation.GetComputerDomainName());

                    return new DomainComputerAccount(name, new AccountSecurityIdentifier(identifier), domain, canonicalName ?? name);
                }
            }

            string localIdentifier = ComputerInformation.GetComputerIdentifier();
            return new LocalComputerAccount(name, new AccountSecurityIdentifier(localIdentifier));
        }

        // network connection is not required
        internal static BaseUserAccount GetLocallyAvailableUserByIdentifier(AccountSecurityIdentifier identifier)
        {
            Check.ObjectIsNotNull(identifier, "identifier");

            string computerIdentifier = ComputerInformation.GetComputerIdentifier();

            var account = (NTAccount)(new SecurityIdentifier(identifier.Value).Translate(typeof(NTAccount)));
            string userName = NameConversion.SimplifyUserName(account.Value);

            if (identifier.GetParentIdentifier() == new AccountSecurityIdentifier(computerIdentifier))
            {
                BaseComputerAccount computer;

                try
                {
                    computer = GetLocalComputer();
                }
                catch (Exception exception)
                {
                    Log.Write(exception);
                    computer = GetLocalComputerOffline();
                }

                var localComputer = computer as LocalComputerAccount;
                var domainComputer = computer as DomainComputerAccount;

                return (localComputer != null) ?
                    (BaseUserAccount)new LocalUserAccount(userName, identifier, localComputer) :
                    (BaseUserAccount)new DomainComputerUserAccount(userName, identifier, domainComputer);
            }
            else
            {
                string domainName = NameConversion.GetUserDomainName(account.Value);
#warning name is netbios...
                DomainAccount domain;

                try
                {
                    domain = DirectoryServicesProvider.GetDomainByName(domainName);

                    BaseUserAccount domainUser = DirectoryServicesProvider.FindDomainEntriesByIdentity(domain).OfType<DomainUserAccount>().FirstOrDefault(user => user.Identifier == identifier);
                    if (domainUser != null)
                        return domainUser;
                }
                catch (Exception exception)
                {
                    Log.Write(exception);
                }

                string userCanonicalName = OfflineDomainInformation.GetLoggedOnUserCanonocalName(identifier.Value);
                domain = new DomainAccount(domainName, identifier.GetParentIdentifier(), userCanonicalName.Substring(0, userCanonicalName.IndexOf('/')));

                return new DomainUserAccount(userName, identifier, domain, userCanonicalName);
            }
        }

        internal static BaseDomainAccount GetBaseDomainAccountByName(string name, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.StringIsMeaningful(name, "name");

            using (new TimeMeasurement("Getting domain {0}".Combine(name)))
            {
                bool isDomain = DomainTools.IsDomain(name, options);

                Log.Add("Object {0} is domain: {1}".Combine(name, isDomain));

                if (isDomain)
                    return DirectoryServicesProvider.GetDomainByName(name, options);

                return DirectoryServicesProvider.GetComputerByName(name, options).Key;
            }
        }
    }
}
