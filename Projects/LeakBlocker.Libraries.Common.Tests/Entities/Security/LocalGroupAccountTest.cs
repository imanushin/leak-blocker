using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Security
{
    partial class LocalGroupAccountTest
    {
        private static IEnumerable<LocalGroupAccount> GetInstances()
        {
            int item = 6532;
            int i = 0;


            foreach (AccountSecurityIdentifier identifier in AccountSecurityIdentifierTest.objects)
            {
                LocalComputerAccount computer = LocalComputerAccountTest.objects.Skip(i).First();

                i++;
                item++;
                yield return new LocalGroupAccount(
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
