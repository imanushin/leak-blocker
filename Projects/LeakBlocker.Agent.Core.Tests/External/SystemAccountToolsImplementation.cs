using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Entities;

namespace LeakBlocker.Agent.Core.Tests.External
{
    internal sealed class SystemAccountToolsImplementation : ISystemAccountTools
    {
        public BaseDomainAccount GetBaseDomainAccountByName(string name, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.StringIsMeaningful(name);

            return DomainAccountTest.First;
        }

        public BaseComputerAccount LocalComputer
        {
            get
            {
                return LocalComputerAccountTest.First;
            }
        }

        public UserContactInformation GetUserContactInformation(LocalUserAccount user)
        {
            throw new NotImplementedException();
        }

        public UserContactInformation GetUserContactInformation(DomainComputerUserAccount user)
        {
            throw new NotImplementedException();
        }

        public UserContactInformation GetUserContactInformation(DomainUserAccount user, Credentials credentials)
        {
            throw new NotImplementedException();
        }

        public BaseUserAccount GetUserByIdentifier(AccountSecurityIdentifier identifier)
        {
            throw new NotImplementedException();
        }
    }
}
