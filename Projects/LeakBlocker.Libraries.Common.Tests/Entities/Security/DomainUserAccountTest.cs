﻿using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Security
{
    partial class DomainUserAccountTest
    {
        private static IEnumerable<DomainUserAccount> GetInstances()
        {
            int item = 6532;

            foreach (AccountSecurityIdentifier identifier in AccountSecurityIdentifierTest.objects)
            {
                DomainAccount domain = DomainAccountTest.objects.First();

                item++;
                yield return new DomainUserAccount(
                    "Domain group " + item,
                    identifier,
                    domain,
                    domain + "\\group " + item);
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
