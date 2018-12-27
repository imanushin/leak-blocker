using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Security
{
    partial class DomainComputerAccountTest
    {
        private static IEnumerable<DomainComputerAccount> GetInstances()
        {
            int computerNumber = 542;

            foreach (DomainAccount account in DomainAccountTest.objects)
            {
                computerNumber++;

                string name = "test" + computerNumber;
                string domainIdentifier = "S-1-5-" + computerNumber;

                yield return new DomainComputerAccount(
                    name,
                    new AccountSecurityIdentifier(domainIdentifier), account, "test");
            }
        }

        protected override void CheckArgumentExceptionParameter(string expectedParameterName, string actualParameterName)
        {
            if (actualParameterName == "parentDomain" || actualParameterName == "shortName")
                return;

            base.CheckArgumentExceptionParameter(expectedParameterName, actualParameterName);
        }
    }
}
