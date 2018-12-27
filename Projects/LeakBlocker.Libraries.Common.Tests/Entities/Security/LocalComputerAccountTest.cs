using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Security
{
    partial class LocalComputerAccountTest
    {
        private static IEnumerable<LocalComputerAccount> GetInstances()
        {
            yield return new LocalComputerAccount(
                "test standalone computer",
                new AccountSecurityIdentifier("s-1-5-6-7"));

            yield return new LocalComputerAccount(
                "test standalone computer",
                new AccountSecurityIdentifier("s-1-5-8-9-125"));

            yield return new LocalComputerAccount(
                "test standalone computer",
                new AccountSecurityIdentifier("s-1-5-8-9-9651"));
        }

        protected override void CheckArgumentExceptionParameter(string expectedParameterName, string actualParameterName)
        {
            if (actualParameterName == "parentDomain" || actualParameterName == "shortName")
                return;

            base.CheckArgumentExceptionParameter(expectedParameterName, actualParameterName);
        }
    }
}
