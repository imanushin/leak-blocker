using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Entities.Management;

namespace LeakBlocker.Libraries.SystemTools.Entities.Implementations
{
    [Serializable]
    internal sealed class LocalDataCache : ILocalDataCache
    {
        private readonly Dictionary<BaseUserAccount, ReadOnlySet<AccountSecurityIdentifier>> users = new Dictionary<BaseUserAccount, ReadOnlySet<AccountSecurityIdentifier>>();
        private CachedComputerData computer = GetComputer();
        private BaseUserAccount consoleUser;

        BaseUserAccount ILocalDataCache.ConsoleUser
        {
            get
            {
                return consoleUser;
            }
        }

        ReadOnlySet<CachedUserData> ILocalDataCache.Users
        {
            get
            {
                return users.Select(pair => new CachedUserData(pair.Key, pair.Value)).ToReadOnlySet();
            }
        }

        CachedComputerData ILocalDataCache.Computer
        {
            get
            {
                return computer;
            }
        }

        private static CachedComputerData GetComputer()
        {
            BaseComputerAccount computer;

            try
            {
                computer = AccountTools.GetLocalComputer();
            }
            catch (Exception exception)
            {
                Log.Write(exception);
                computer = AccountTools.GetLocalComputerOffline();
            }
            
            ReadOnlySet<AccountSecurityIdentifier> groups = ReadOnlySet<AccountSecurityIdentifier>.Empty;

            try
            {
                var domainComputer = computer as DomainComputerAccount;
                if (domainComputer != null)
                {
                    ReadOnlyDictionary<IDomainMember, AccountSecurityIdentifierSet> principals = DirectoryServicesProvider.FindDomainEntriesByIdentity(domainComputer.Parent);
                    DomainComputerAccount computerPrincipal = principals.OfType<DomainComputerAccount>().FirstOrDefault(principal => principal.Identifier == domainComputer.Identifier);

                    if (computerPrincipal != null)
                        groups = principals[computerPrincipal].Identifiers;
                }
            }
            catch (Exception exception)
            {
                Log.Write(exception);
            }

            return new CachedComputerData(computer, groups);
        }

        void ILocalDataCache.Update()
        {
            computer = GetComputer();
            BaseUserAccount newConsoleUser = null;

            BaseUserAccount user;
            ReadOnlySet<AccountSecurityIdentifier> groups = null;

            var computerIdentifier = new AccountSecurityIdentifier(ComputerInformation.GetComputerIdentifier());
            Log.Write("Computer identifier: {0}.", computerIdentifier);
 
            foreach (LocalUserSession currentSession in LocalUserSession.EnumerateLocalSessions().Where(session => session.Supported))
            {
                try
                {  
                    var identifier = new AccountSecurityIdentifier(currentSession.UserIdentifier);

                    if ((identifier.GetParentIdentifier() == computerIdentifier) || (identifier.GetParentIdentifier() == null))
                    {
                        var localComputer = computer.Computer as LocalComputerAccount;
                        var domainComputer = computer.Computer as DomainComputerAccount;

                        user = (localComputer != null) ?
                            (BaseUserAccount)new LocalUserAccount(currentSession.Name, identifier, localComputer) :
                            (BaseUserAccount)new DomainComputerUserAccount(currentSession.Name, identifier, domainComputer);

                        ReadOnlyDictionary<Account, AccountSecurityIdentifierSet> principals = DirectoryServicesProvider.FindLocalEntriesByIdentity(computer.Computer);

                        groups = principals.Where(item => item.Key.Identifier == identifier).Select(item => item.Value.Identifiers).FirstOrDefault();
                        groups = groups ?? ReadOnlySet<AccountSecurityIdentifier>.Empty;
                    }
                    else
                    {
                        DomainAccount domain;
                        if (!string.IsNullOrEmpty(currentSession.NetworkDomainName))
                            domain = new DomainAccount(currentSession.DomainName, identifier.GetParentIdentifier(), currentSession.NetworkDomainName);
                        else if(!string.IsNullOrEmpty(currentSession.UserPrincipalName))
                            domain = new DomainAccount(currentSession.DomainName, identifier.GetParentIdentifier(), NameConversion.GetUserDomainName(currentSession.UserPrincipalName));
                        else
                        {
                            string currentUserCanonicalName = OfflineDomainInformation.GetLoggedOnUserCanonocalName(currentSession.UserIdentifier);
                            if (!string.IsNullOrEmpty(currentUserCanonicalName) && currentUserCanonicalName.Contains('/'))
                                domain = new DomainAccount(currentSession.DomainName, identifier.GetParentIdentifier(), currentUserCanonicalName.Substring(0, currentUserCanonicalName.IndexOf('/')));
                            else
                                domain = DirectoryServicesProvider.GetDomainByName(currentSession.DomainName);
                        }
                        
                        string canonicalName = null;

                        try
                        {
                            ReadOnlyDictionary<IDomainMember, AccountSecurityIdentifierSet> principals = DirectoryServicesProvider.FindDomainEntriesByIdentity(domain);
                            DomainUserAccount userPrincipal = principals.Keys.OfType<DomainUserAccount>().FirstOrDefault(principal => principal.Identifier == identifier);

                            if (userPrincipal == null)
                                Log.Write("User {0} was not found in the domain {1}.", identifier, domain);
                            else
                            {
                                canonicalName = userPrincipal.CanonicalName;
                                groups = principals[userPrincipal].Identifiers;
                            }
                        }
                        catch (Exception exception)
                        {
                            Log.Write(exception);
                        }

                        groups = groups ?? ReadOnlySet<AccountSecurityIdentifier>.Empty;
                        canonicalName = canonicalName ?? OfflineDomainInformation.GetLoggedOnUserCanonocalName(identifier.Value);
                        if(canonicalName == null)
                        {
                            Log.Write("Cannot get canonical name for user {0}.", identifier);
                            canonicalName = currentSession.Name;
                        }

                        user = new DomainUserAccount(currentSession.Name, identifier, domain, canonicalName);

                        users[user] = groups;
                    }

                    if (currentSession.Active)
                        newConsoleUser = user;
                }
                catch (Exception exception)
                {
                    Log.Write(exception);
                }
            }

            consoleUser = newConsoleUser;
        }
    }
}
