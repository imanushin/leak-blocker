using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools;

namespace LeakBlocker.Agent.Core.Implementations
{
    internal sealed class VersionIndependentPrivateStorage : IVersionIndependentPrivateStorage
    {
        private static readonly IPrivateRegistryStorage storage = SystemObjects.CreatePrivateRegistryStorage(new Guid(storageIdentifier));

        private const string key = "VersionIndependentStorageKey";
        private const string counter = "VersionIndependentStorageCounter";
        private const string format = "{0}|{1}|{2}|{3}"; // Version|PasswordHash|UninstallString|EncryptionKey
        private const string storageIdentifier = "{00000000-C29A-4C6C-A46C-D428E47336F2}";

        //private const string installLocalCommandLineMode = "-il"; // -pwd <password>
        //private const string installRemoteCommandLineMode = "-ir"; // -pwh <hash> -srv <address> -key <keyrequest>
        //private const string updateRemoteCommandLineMode = "-iu"; // -srv <address> -ver <verification>
        //private const string uninstallLocalCommandLineMode = "-ul"; // -pwd <password>
        private const string uninstallSelfCommandLineMode = "-us"; // -enc <secretkey>
        //private const string changeServerAddressCommandLineMode = "-cs"; // -pwd <password> -srv <address>
        //private const string changeConfigurationCommandLineMode = "-cc"; // -pwd <password> -cfg <file>

        //private const string configurationFileNameCommandLineParameter = "-cfg";
        //private const string serverAddressCommandLineParameter = "-srv";
        //private const string passwordCommandLineParameter = "-pwd";
        //private const string passwordHashCommandLineParameter = "-pwh";
        private const string secretKeyCommandLineParameter = "-enc";
        //private const string verificationKeyCommandLineParameter = "-ver";
        //private const string publicKeyCommandLineParameter = "-key";

        void IVersionIndependentPrivateStorage.SaveData(string hash, string secretKey)
        {
            string file = AgentObjects.AgentConstants.ServiceModuleFolder + SharedObjects.Constants.MainModuleFile;
            string uninstallString = SharedObjects.CommandLine.Create(file, uninstallSelfCommandLineMode, secretKeyCommandLineParameter, secretKey);

            string result = format.Combine(SharedObjects.Constants.VersionString, hash, uninstallString, secretKey);
            storage.SetValue(key, result);
        }

        bool IVersionIndependentPrivateStorage.Empty
        {
            get
            {
                return string.IsNullOrEmpty(storage.GetValue(key));
            }
        }

        string IVersionIndependentPrivateStorage.Version
        {
            get
            {
                string data = storage.GetValue(key);

                return string.IsNullOrWhiteSpace(data) ? string.Empty : data.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0];
            }
        }

        string IVersionIndependentPrivateStorage.PasswordHash
        {
            get
            {
                string data = storage.GetValue(key);

                return string.IsNullOrWhiteSpace(data) ? string.Empty : data.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[1];
            }
        }

        string IVersionIndependentPrivateStorage.UninstallString
        {
            get
            {
                string data = storage.GetValue(key);

                return string.IsNullOrWhiteSpace(data) ? string.Empty : data.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[2];
            }
        }

        string IVersionIndependentPrivateStorage.SecretKey
        {
            get
            {
                string data = storage.GetValue(key);
                
                return string.IsNullOrWhiteSpace(data) ? string.Empty : data.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[3];
            }
        }

        int IVersionIndependentPrivateStorage.InstalledVersionsCounter
        {
            get
            {
                string data = storage.GetValue(counter);
                int value;
                if (!int.TryParse(data, out value))
                {
                    Log.Write("Cannot parse installed version counter {0}.", data);
                    value = 0;
                }
                Log.Write("Querying installed version counter. Value is {0}.", value);
                return value;
            }
        }

        void IVersionIndependentPrivateStorage.IncrementCounter()
        {
            string data = storage.GetValue(counter);
            int value;
            if (!int.TryParse(data, out value))
            {
                Log.Write("Cannot parse installed version counter {0}.", data);
                value = 0;
            }
            value++;
            storage.SetValue(counter, value.ToString(CultureInfo.InvariantCulture));
            Log.Write("Installed version counter was set to {0}", value);
        }

        void IVersionIndependentPrivateStorage.DecrementCounter()
        {
            string data = storage.GetValue(counter);
            int value;
            if (!int.TryParse(data, out value))
            {
                Log.Write("Cannot parse installed version counter {0}.", data);
                value = 0;
            }
            value = Math.Max(0, value - 1);
            storage.SetValue(counter, value.ToString(CultureInfo.InvariantCulture));
            Log.Write("Installed version counter was set to {0}", value);
        }
    }
}
