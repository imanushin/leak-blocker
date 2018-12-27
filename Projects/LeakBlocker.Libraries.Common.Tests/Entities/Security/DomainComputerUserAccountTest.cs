using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Security
{
    partial class DomainComputerUserAccountTest
    {
        private static IEnumerable<DomainComputerUserAccount> GetInstances()
        {
            int item = 9564;
            int i = 0;
            foreach (AccountSecurityIdentifier identifier in AccountSecurityIdentifierTest.objects)
            {
                DomainComputerAccount computer = DomainComputerAccountTest.objects.Skip(i).First();

                i++;
                item++;
                yield return new DomainComputerUserAccount(
                    "Domain group " + item,
                    identifier,
                    computer);

            }
        }

        protected override void CheckArgumentExceptionParameter(string expectedParameterName, string actualParameterName)
        {
            if (actualParameterName == "shortName")
                return;

            base.CheckArgumentExceptionParameter(expectedParameterName, actualParameterName);
        }
    }
}
