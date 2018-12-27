using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;
using Microsoft.Win32;

namespace LeakBlocker.Server.Installer.InternalTools
{
    internal sealed class ProductRegistrator : IProductRegistrator
    {
        private static readonly Guid uninstallGuid = new Guid("{7A87C31D-19A1-4936-ACC0-3E51F715ED7A}");
        private static readonly string guidText = uninstallGuid.ToString("B");

        public void RegisterProduct()
        {
            using (RegistryKey parent = OpenCommonInstallationKey())
            {
                try
                {
                    RegistryKey key = parent.OpenSubKey(guidText, true) ?? parent.CreateSubKey(guidText);

                    Check.ObjectIsNotNull(key);

                    using (key)
                    {
                        string exePath = InternalObjects.FileSystemConstants.UninstallerPath;
                        string version = SharedObjects.Constants.Version.ToString(3);

                        string fullRemoveString = SharedObjects.CommandLine.Create(exePath, "/remove");

                        key.SetValue("DisplayName", CommonStrings.ProductName);
                        key.SetValue("ApplicationVersion", version);
                        key.SetValue("Publisher", CommonStrings.CompanyName);
                        key.SetValue("DisplayIcon", exePath);
                        key.SetValue("DisplayVersion", version);
                        key.SetValue("HelpLink", "http://www.leakblocker.com");
                        key.SetValue("URLInfoAbout", "http://www.leakblocker.com");
                        key.SetValue("Contact", "support@deltacorvi.com");
                        key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                        key.SetValue("UninstallString", fullRemoveString);
                        key.SetValue("NoModify", 1);
                        key.SetValue("NoRepair", 1);
                        key.SetValue("NoRemove", 0);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        "An error occurred writing uninstall information to the registry. The service is fully installed but can only be uninstalled manually through the command line.",
                        ex);
                }
            }
        }

        public void UnregisterProduct()
        {
            using (RegistryKey parent = OpenCommonInstallationKey())
            {
                parent.DeleteSubKey(guidText, false);
            }
        }

        public string GetRemoveString()
        {
            using (RegistryKey parent = OpenCommonInstallationKey())
            {
                using (var innerKey = parent.OpenSubKey(guidText, true))
                {
                    if (innerKey == null)
                        return null;

                    return (string)innerKey.GetValue("UninstallString", null);
                }
            }
        }

        private static RegistryKey OpenCommonInstallationKey()
        {
            RegistryKey result = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true);

            Check.ObjectIsNotNull(result);

            return result;
        }
    }
}
