﻿using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Security
{
    partial class LocalUserAccountTest
    {
        private static IEnumerable<LocalUserAccount> GetInstances()
        {
            int item = 1972;
            int i = 0;

            foreach (AccountSecurityIdentifier identifier in AccountSecurityIdentifierTest.objects)
            {
                LocalComputerAccount computer = LocalComputerAccountTest.objects.Skip(i % 3).First();

                i++;
                item++;

                yield return new LocalUserAccount(
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
