using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.ServerShared.AdminViewCommunication.Tests
{
    partial class EmailSettingsTest
    {
        private static IEnumerable<EmailSettings> GetInstances()
        {
            foreach (string from in new[]{"i@b", string.Empty})
            {
                foreach (string to in new[] { "i@b", string.Empty })
                {
                    foreach (string smtpServer in new[]{"host", string.Empty})
                    {
                        foreach (int port in new[] { 25, 597 })
                        {
                            foreach (string userName in GetStrings())
                            {
                                foreach (string password in GetStrings())
                                {
                                    foreach (bool useSslConnection in new[] {true, false})
                                    {
                                        foreach (bool isAuthenticationEnabled in new[] {true, false})
                                        {
                                            yield return new EmailSettings(from, to, smtpServer, port, useSslConnection, isAuthenticationEnabled, userName, password);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static IEnumerable<string> GetStrings()
        {
            return new[] { string.Empty, "123" };
        }
    }
}
