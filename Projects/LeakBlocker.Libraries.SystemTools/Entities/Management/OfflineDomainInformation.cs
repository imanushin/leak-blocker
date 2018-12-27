using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using LeakBlocker.Libraries.Common;
using Microsoft.Win32;

namespace LeakBlocker.Libraries.SystemTools.Entities.Management
{
    internal static class OfflineDomainInformation
    {
        internal static string GetLoggedOnUserCanonocalName(string identifier)
        {
            Check.ObjectIsNotNull(identifier, "identifier");

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Group Policy\State\" + identifier))
            {
                if (key == null)
                    return null;

                object value = key.GetValue("Distinguished-Name");
                if (value == null)
                    return null;

                string result = null;
                SharedObjects.ExceptionSuppressor.Run(() => result = NameConversion.ConvertToCanonicalName(value.ToString()));
                return result;
            }
        }

        internal static string GetCurrentComputerCanonocalName()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Group Policy\State\Machine"))
            {
                if (key == null)
                    return null;

                object value = key.GetValue("Distinguished-Name");
                if (value == null)
                    return null;

                string result = null;
                SharedObjects.ExceptionSuppressor.Run(() => result = NameConversion.ConvertToCanonicalName(value.ToString()));
                return result;
            }
        }

        internal static string GetCurrentComputerDomainIdentifier()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Group Policy\GroupMembership"))
            {
                if (key == null)
                    return null;

                foreach (string valueName in key.GetValueNames())
                {
                    if (key.GetValueKind(valueName) != RegistryValueKind.String)
                        continue;

                    object value = key.GetValue(valueName);
                    if (value == null)
                        continue;

                    string data = value.ToString();

                    try
                    {
                        var identifier = new SecurityIdentifier(data);

                        if (identifier.AccountDomainSid == null)
                            continue;

                        string[] parts = data.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 0)
                            continue;

                        int relativeIdentifier;
                        if (!int.TryParse(parts[parts.Length - 1], out relativeIdentifier))
                            continue;

                        if (relativeIdentifier < 1000)
                            continue;

                        return data;
                    }
                    catch
                    {
                    }
                }
            }

            return null;
        }
    }
}
