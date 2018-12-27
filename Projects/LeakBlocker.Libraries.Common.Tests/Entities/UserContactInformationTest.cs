using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;

namespace LeakBlocker.Libraries.Common.Tests.Entities
{
    partial class UserContactInformationTest
    {
        private static IEnumerable<UserContactInformation> GetInstances()
        {
            foreach (string firstName in new[] { string.Empty, "John" })
            {
                foreach (string lastName in new[] { string.Empty, "Smith" })
                {
                    foreach (string company in new[] { string.Empty, "Delta Corvi LLC" })
                    {
                        foreach (string email in new[] {string.Empty, "administrator@deltacorvi.com"})
                        {
                            foreach (string phone in new[] {string.Empty, "+1-951-357-12-64"})
                            {
                                yield return new UserContactInformation(firstName, lastName, company, email, phone);
                            }
                        }
                    }
                }
            }
        }
    }
}
