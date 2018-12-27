using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Server.Service.Generated;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.Network.AdminView
{
    internal sealed class AccountToolsServer : GeneratedAccountTools
    {
        public AccountToolsServer()
            : base((ISecuritySessionManager)InternalObjects.AdminViewSecuritySessionManager)
        {
        }

        protected override ReadOnlySet<string> FindDnsDomains()
        {
            IEnumerable<BaseDomainAccount> domains = InternalObjects.SecurityObjectCache.Data.Domains;

            domains = domains.Union(InternalObjects.SecurityObjectCache.Data.Computers.OfType<LocalComputerAccount>());

            List<string> result = domains.Select(domain => domain.FullName).ToList();

            if (!result.Any())
            {
                string dnsMachineName = Dns.GetHostEntry("localhost").HostName;
                string shortMachineName = Dns.GetHostName();

                result.Add(dnsMachineName);

                if (dnsMachineName.StartsWith(shortMachineName, StringComparison.OrdinalIgnoreCase) && dnsMachineName.Length > shortMachineName.Length)
                {
                    string suffix = dnsMachineName.Substring(shortMachineName.Length + 1);

                    result.Add(suffix);
                }
            }

            return result.ToReadOnlySet();
        }

        protected override ReadOnlySet<Scope> GetAvailableComputerScopes()
        {
            IDomainSnapshot container = InternalObjects.SecurityObjectCache.Data;

            ReadOnlySet<Scope> result =
                container.Computers
                .Union<IScopeObject>(container.Domains)
                .Union(container.Groups.OfType<DomainGroupAccount>())
                .Union(container.OrganizationalUnits)
                .Select(item => new Scope(item)).ToReadOnlySet();

            return result;
        }

        protected override ReadOnlySet<Scope> GetAvailableUserScopes()
        {
            IDomainSnapshot container = InternalObjects.SecurityObjectCache.Data;

            ReadOnlySet<Scope> result =
                container.Users
                .Union<IScopeObject>(container.Groups)
                .Union(container.OrganizationalUnits)
                .Select(item => new Scope(item)).ToReadOnlySet();

            return result;
        }

        protected override ReadOnlySet<ResultComputer> GetComputers(ReadOnlySet<Scope> scopes)
        {
            IEnumerable<ResultComputer> result = scopes.SelectMany(GetComputersFromScope);

            return result.ToReadOnlySet();
        }

        private static IEnumerable<ResultComputer> GetComputersFromScope(Scope scope)
        {
            return
                GetMembersFromComputerScopes(scope).OfType<BaseComputerAccount>().Select(
                    item => new ResultComputer(scope, item));
        }

        private static IEnumerable<object> GetMembersFromComputerScopes(Scope scope)
        {
            Check.ObjectIsNotNull(scope);

            switch (scope.ScopeType)
            {
                case ScopeType.Domain:
                    return InternalObjects.SecurityObjectCache.Data.GetObjectsInDomain((DomainAccount)scope.TargetObject);
                case ScopeType.OU:
                    return InternalObjects.SecurityObjectCache.Data.GetObjectsInOrganizationalUnit((OrganizationalUnit)scope.TargetObject);
                case ScopeType.Computer:
                    return new[] { (BaseComputerAccount)scope.TargetObject };
                case ScopeType.Group:
                    return InternalObjects.SecurityObjectCache.Data.GetObjectsInGroup((DomainGroupAccount)scope.TargetObject);

                default:
                    throw new ArgumentException("Scope {0} is not supported".Combine(scope), "scope");
            }
        }

        protected override ReadOnlySet<BaseComputerAccount> GetAvailableComputers()
        {
            ReadOnlySet<BaseComputerAccount> computers = InternalObjects.SecurityObjectCache.Data.Computers;

            ReadOnlySet<BaseComputerAccount> storedObjects = StorageObjects.AccountManager.GetSavedComputers();

            return computers.Union(storedObjects).ToReadOnlySet();
        }

        protected override ReadOnlySet<BaseUserAccount> GetAvailableUsers()
        {
            ReadOnlySet<BaseUserAccount> users = InternalObjects.SecurityObjectCache.Data.Users;

            ReadOnlySet<BaseUserAccount> storedObjects = StorageObjects.AccountManager.GetSavedUsers();

            return users.Union(storedObjects).ToReadOnlySet();
        }

        protected override string GetPreferableDomain()
        {
            List<string> dnsDomains = FindDnsDomains().ToList();

            dnsDomains.Sort((str1, str2) => str1.Length - str2.Length);//Сортируем так, чтобы имя домена, если оно есть, было бы в начале

            return dnsDomains.First();
        }

        protected override bool IsRequestCompleted(DomainUpdateRequest request)
        {
            return (InternalObjects.SecurityObjectCache.CheckUpdateRequestResult(request.RequestIdentifier) != null);
        }

        protected override UserContactInformation GetCurrentUserInformation()
        {
            var currentUserSid = InternalObjects.AdminViewSecuritySessionManager.CurrentUserIdentifier;

            BaseUserAccount currentUser = InternalObjects.SecurityObjectCache.Data.Users.FirstOrDefault(innerUser => Equals(innerUser.Identifier, currentUserSid));

            if (currentUser == null)
                return UserContactInformation.Empty;

            var domainUser = currentUser as DomainUserAccount;

            if (domainUser != null)
            {
                Credentials credentials = StorageObjects.CredentialsManager.TryGetCredentials(domainUser.Parent);

                if (credentials == null)//Нет в кеше - никуда не лезем, так как пользователь, наверное, еще не созрел для покупки
                    return UserContactInformation.Empty;

                return SystemObjects.SystemAccountTools.GetUserContactInformation(domainUser, credentials);
            }

            return UserContactInformation.Empty;
        }

        protected override DomainUpdateRequest CheckAndSetCredentials(DomainCredentials credentials)
        {
            BaseDomainAccount result = InternalObjects.SecurityObjectCache.GetBaseDomainAccountByNameImmediately(
                 credentials.Domain,
                 new SystemAccessOptions(credentials.FullUserName, credentials.Password, credentials.Domain));

            var initializedCredentials = new Credentials(result, credentials.FullUserName, credentials.Password);
            StorageObjects.CredentialsManager.UpdateCredentials(initializedCredentials);

            Guid session = InternalObjects.SecurityObjectCache.RequestUpdate(initializedCredentials);

            return new DomainUpdateRequest(result, session);
        }
    }
}
