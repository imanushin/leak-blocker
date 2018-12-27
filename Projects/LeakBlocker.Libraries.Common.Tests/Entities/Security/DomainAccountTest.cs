using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Security
{
    partial class DomainAccountTest
    {
        private static IEnumerable<DomainAccount> GetInstances()
        {
            int item = 125;
            foreach (AccountSecurityIdentifier identifier in AccountSecurityIdentifierTest.objects)
            {
                item++;

                yield return new DomainAccount(
                    "test_account " + item,
                    identifier,
                    "test_domain");
            }
        }

        protected override void CheckArgumentExceptionParameter(string expectedParameterName, string actualParameterName)
        {
            if (actualParameterName == "fullName" || actualParameterName == "shortName")
                return;

            base.CheckArgumentExceptionParameter(expectedParameterName, actualParameterName);
        }

    }
}
