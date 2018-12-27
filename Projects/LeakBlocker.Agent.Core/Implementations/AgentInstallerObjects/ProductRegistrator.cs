using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;
using Microsoft.Win32;

namespace LeakBlocker.Agent.Core.Implementations.AgentInstallerObjects
{
    internal static class ProductRegistrator
    {
        private const string basekey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        private const string baseGuid = "{75DB259F-6ECA-4C68-A259-000000000000}";
        private static readonly string identifier = InitializeIdentifier();

        public static void RegisterProduct()
        {
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(basekey, true))
            using (RegistryKey key = parent.OpenSubKey(identifier, true) ?? parent.CreateSubKey(identifier))
            {
                key.SetValue("DisplayName", AgentServiceStrings.AgentServiceDisplayName.Combine(SharedObjects.Constants.Version.ToString(3)));
                key.SetValue("ApplicationVersion", SharedObjects.Constants.Version.ToString(3));
                key.SetValue("Publisher", CommonStrings.CompanyName);
                key.SetValue("DisplayIcon", AgentObjects.AgentConstants.ServiceModulePath);
                key.SetValue("DisplayVersion", SharedObjects.Constants.Version.ToString(3));
                key.SetValue("HelpLink", "http://www.leakblocker.com");
                key.SetValue("URLInfoAbout", "http://www.leakblocker.com");
                key.SetValue("Contact", "support@deltacorvi.com");
                key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
                key.SetValue("UninstallString", SharedObjects.CommandLine.Create(AgentObjects.AgentConstants.ServiceModulePath, "-i", "-uu"));
                key.SetValue("NoModify", 1);
                key.SetValue("NoRepair", 1);
                key.SetValue("NoRemove", 0);
            }
        }

        public static void UnregisterProduct()
        {
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(basekey, true))
            {
                parent.DeleteSubKey(identifier, false);
            }
        }

        private static string InitializeIdentifier()
        {
            byte[] guid = new Guid(baseGuid).ToByteArray();
            BitConverter.GetBytes((short)SharedObjects.Constants.Version.Build).CopyTo(guid, guid.Length - 2);
            BitConverter.GetBytes((short)SharedObjects.Constants.Version.Minor).CopyTo(guid, guid.Length - 4);
            BitConverter.GetBytes((short)SharedObjects.Constants.Version.Major).CopyTo(guid, guid.Length - 6);
            return new Guid(guid).ToString("B");
        }
    }
}
