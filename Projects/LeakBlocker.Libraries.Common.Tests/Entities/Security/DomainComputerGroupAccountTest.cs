using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Security
{
    partial class DomainComputerGroupAccountTest
    {
        private static IEnumerable<DomainComputerGroupAccount> GetInstances()
        {
            int item = 9564;

            int i = 0;
            foreach (AccountSecurityIdentifier identifier in AccountSecurityIdentifierTest.objects)
            {
                DomainComputerAccount computer = DomainComputerAccountTest.objects.Skip(i).First();


                DomainAccount domain = DomainAccountTest.objects.First();

                i++;
                item++;
                yield return new DomainComputerGroupAccount(
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
