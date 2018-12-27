using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.SystemTools.Tests.Entities
{
    partial class CachedComputerDataTest 
    {
        private sealed class SystemAccountToolsImplementation : ISystemAccountTools
        {
            public BaseDomainAccount GetBaseDomainAccountByName(string name, SystemAccessOptions options = default(SystemAccessOptions))
            {
                throw new NotImplementedException();
            }

            public BaseUserAccount GetUserByIdentifier(AccountSecurityIdentifier identifier)
            {
                throw new NotImplementedException();
            }

            public BaseComputerAccount LocalComputer
            {
                get { return BaseComputerAccountTest.First; }
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
        }

        [TestInitialize]
        public void Init()
        {
            SystemObjects.Singletons.SystemAccountTools.SetTestImplementation(new SystemAccountToolsImplementation());
        }

        private static IEnumerable<CachedComputerData> GetInstances()
        {
            foreach (BaseComputerAccount computer in BaseComputerAccountTest.objects)
            {
                yield return new CachedComputerData(computer, AccountSecurityIdentifierTest.objects.ToReadOnlySet());
            }
        }

        protected override bool SkipSerializationTest()
        {
            return true;
        }
    }
}
