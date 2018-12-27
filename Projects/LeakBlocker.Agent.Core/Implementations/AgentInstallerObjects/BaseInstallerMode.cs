using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.ProcessTools;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Agent.Core.Implementations.AgentInstallerObjects
{
    internal abstract class BaseInstallerMode
    {
        protected enum InstallerCondition
        {
            PasswordIsCorrect,
            AnyVersionWasInstalled,
            SameVersionIsNotInstalled,
            SameVersionIsInstalled,
            AnyVersionIsNotInstalled,
            VerificationDataIsCorrect,
            SecretKeyIsCorrect
        }

        protected enum InstallerArgument
        {
            Password,
            PasswordHash,
            ServerAddress,
            KeyRequest,
            SecretKey,
            VerificationKey,
            ConfigurationFile,
        }

        private readonly ReadOnlyDictionary<InstallerArgument, string> argumentKeys = new Dictionary<InstallerArgument, string>
        {
            { InstallerArgument.Password, "-pwd" },
            { InstallerArgument.PasswordHash, "-pwh" },
            { InstallerArgument.ServerAddress, "-srv" },
            { InstallerArgument.KeyRequest, "-key" },
            { InstallerArgument.SecretKey, "-enc" },
            { InstallerArgument.VerificationKey, "-ver" },
            { InstallerArgument.ConfigurationFile, "-cfg" },
        }.ToReadOnlyDictionary();

        private readonly Dictionary<InstallerArgument, string> arguments = new Dictionary<InstallerArgument, string>();
        
        internal virtual void Start()
        {
            Log.Write("Type: {0}.".Combine(GetType().Name));
            Log.Write("Parsing parameters.");

            foreach (KeyValuePair<InstallerArgument, string> argument in argumentKeys.Where(argument => Arguments.Contains(argument.Key)))
            {
                if (!SharedObjects.CommandLine.HasValue(argument.Value))
                {
                    Log.Write("Required argument {0} was not specified.".Combine(argument.Key));
                    ThrowException(AgentInstallerStatus.WrongArguments);
                }
                arguments[argument.Key] = SharedObjects.CommandLine.GetValue(argument.Value);
            }

            Log.Write("Checking conditions.");

            foreach (InstallerCondition currentCondition in Conditions)
            {
                switch (currentCondition)
                {
                    case InstallerCondition.PasswordIsCorrect:
                        Log.Write("Checking if password is correct.");
                        if (!VerifyPassword(GetArgument(InstallerArgument.Password)))
                        {
                            ThrowException(AgentInstallerStatus.IncorrectPassword);
                        }
                        break;

                    case InstallerCondition.AnyVersionWasInstalled:
                        Log.Write("Checking if any version was installed.");
                        if (AgentObjects.VersionIndependentPrivateStorage.Empty)
                            ThrowException(AgentInstallerStatus.AgentIsNotInstalled);
                        break;

                    case InstallerCondition.SameVersionIsNotInstalled:
                        Log.Write("Checking if same version is not installed.");
                        if ((AgentObjects.VersionIndependentPrivateStorage.InstalledVersionsCounter > 0) && AgentObjects.VersionIndependentPrivateStorage.Version.Equals(SharedObjects.Constants.VersionString, StringComparison.Ordinal))
                            ThrowException(AgentInstallerStatus.SameVersionAlreadyInstalled);
                        break;

                    case InstallerCondition.SameVersionIsInstalled:
                        Log.Write("Checking if same version is installed.");
                        if ((AgentObjects.VersionIndependentPrivateStorage.InstalledVersionsCounter == 0) || !AgentObjects.VersionIndependentPrivateStorage.Version.Equals(SharedObjects.Constants.VersionString, StringComparison.Ordinal))
                            ThrowException(AgentInstallerStatus.CurrentVersionIsNotInstalled);
                        break;

                    case InstallerCondition.AnyVersionIsNotInstalled:
                        Log.Write("Checking if any version is not installed.");
                        if (AgentObjects.VersionIndependentPrivateStorage.InstalledVersionsCounter != 0)
                            ThrowException(AgentInstallerStatus.AgentAlreadyInstalled);
                        break;

                    case InstallerCondition.VerificationDataIsCorrect:
                        Log.Write("Checking if verification data is correct.");
                        string verificationKey = GetArgument(InstallerArgument.VerificationKey);
                        using (var provider = new SymmetricEncryptionProvider(new SymmetricEncryptionKey(AgentObjects.AgentPrivateStorage.SecretKey)))
                        {
                            byte[] data = Convert.FromBase64String(verificationKey);

                            try
                            {
                                byte[] source = provider.Decrypt(data);

                                var difference = new TimeSpan(Time.Now.Ticks - BitConverter.ToInt64(source, 0));

                                if ((difference < TimeSpan.Zero) || (difference > TimeSpan.FromMinutes(5)))
                                    Exceptions.Throw(ErrorMessage.TimeDifference);
                            }
                            catch
                            {
                                ThrowException(AgentInstallerStatus.IncorrectVerificationKey);
                            }
                        }
                        break;

                    case InstallerCondition.SecretKeyIsCorrect:
                        Log.Write("Checking if secret key is correct.");
                        string secretKey = GetArgument(InstallerArgument.SecretKey);
                        if (!AgentObjects.AgentPrivateStorage.SecretKey.Equals(secretKey))
                            ThrowException(AgentInstallerStatus.SecretKeyIsIncorrect);
                        break;
                }
            }

            Log.Write("Performing required actions.");

            PerformActions();
        }

        protected abstract IEnumerable<InstallerCondition> Conditions
        {
            get;
        }

        protected abstract IEnumerable<InstallerArgument> Arguments
        {
            get;
        }

        protected abstract void PerformActions();

        #region Auxiliary methods

        protected static bool VerifyPassword(string password)
        {
            string hash = CalculateHash(password);
            return string.IsNullOrWhiteSpace(AgentObjects.VersionIndependentPrivateStorage.PasswordHash) ||
                hash.Equals(AgentObjects.VersionIndependentPrivateStorage.PasswordHash, StringComparison.Ordinal);
        }

        protected string GetArgument(InstallerArgument argument)
        {
            string result = arguments.TryGetValue(argument);
            if (string.IsNullOrWhiteSpace(result))
            {
                Log.Write("Requested argument {0} was not found.".Combine(argument));
                ThrowException(AgentInstallerStatus.WrongArguments);
            }
            return result;
        }

        protected static void ThrowException(AgentInstallerStatus status)
        {
            Check.EnumerationValueIsDefined(status, "status");

            var result = new InvalidOperationException("Agent installation failed.");
            result.Data.Add("Status", status);
            throw result;
        }

        private static string CalculateHash(string password)
        {
            using (var provider = new SHA1CryptoServiceProvider())
            {
                return Convert.ToBase64String(provider.ComputeHash(Encoding.Unicode.GetBytes(password)));
            }
        }

        private static KeyValuePair<string, int> SplitServerAddress(string address)
        {
            if (address == null)
                return new KeyValuePair<string, int>(AgentObjects.AgentConstants.StandaloneServerAddress, 0);

            string[] parts = address.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            string hostName = parts[0];
            int port = (parts.Length <= 1) ? SharedObjects.Constants.DefaultTcpPort : int.Parse(parts[1], CultureInfo.InvariantCulture);

            return new KeyValuePair<string, int>(hostName, port);
        }

        #endregion

        #region Actions

        internal static void InstallService()
        {
            using (new TimeMeasurement("Service creation"))
            {
                IWindowsService service = SystemObjects.CreateWindowsService(AgentObjects.AgentConstants.ServiceName);
                service.Create(AgentObjects.AgentConstants.ServiceModulePath);

                service.StartType = ServiceStartType.Automatic;
                service.Description = AgentObjects.AgentConstants.ServiceDescription;
                service.DisplayedName = AgentObjects.AgentConstants.ServiceDisplayedName;
                service.FailureCounterResetInterval = TimeSpan.FromSeconds(1);
                service.SetFailureActions(new[] { ServiceFailureAction.Restart, ServiceFailureAction.Restart, ServiceFailureAction.Restart }.ToReadOnlyList());
                service.SetDependencies(ReadOnlySet<string>.Empty);
                service.LoadOrderGroup = string.Empty;
                service.ErrorControl = ServiceErrorControl.Normal;
                service.CommandLine = SharedObjects.CommandLine.Create(AgentObjects.AgentConstants.ServiceModulePath, AgentObjects.AgentConstants.ServiceCommandLineMode);
                service.FailureCommand = string.Empty;
                service.FailureRebootMessage = string.Empty;
                service.UserName = "LocalSystem";
                service.Password = "";
            }
        }

        internal static void UninstallService()
        {
            using (new TimeMeasurement("Service removal."))
            {
                IWindowsService service = SystemObjects.CreateWindowsService(AgentObjects.AgentConstants.ServiceName);
                service.Delete();
            }
        }

        internal static void StartService()
        {
            using (new TimeMeasurement("Service launch"))
            {
                IWindowsService service = SystemObjects.CreateWindowsService(AgentObjects.AgentConstants.ServiceName);
                service.Start();
            }
        }

        internal static void DeleteFiles()
        {
            using (new TimeMeasurement("File removal"))
            {
                SystemObjects.FileTools.Delete(AgentObjects.AgentConstants.ServiceModulePath);
            }
        }

        internal static void CopyFiles()
        {
            using (new TimeMeasurement("File creation"))
            {
                if (string.Equals(AgentObjects.AgentConstants.ServiceModulePath, SharedObjects.Constants.MainModulePath, StringComparison.OrdinalIgnoreCase))
                {
                    Log.Write("Current executable is already where required.");
                    return;
                }

                SystemObjects.FileTools.CopyFile(SharedObjects.Constants.MainModulePath, AgentObjects.AgentConstants.ServiceModulePath);
            }
        }

        internal static void UninstallPreviousVersion()
        {
            using (new TimeMeasurement("Previous version removal"))
            {
                if (AgentObjects.VersionIndependentPrivateStorage.Empty)
                {
                    Log.Write("Previous version was not found.");
                    return;
                }

                ReadOnlyList<string> parameters = SharedObjects.CommandLine.Split(AgentObjects.VersionIndependentPrivateStorage.UninstallString);
                var processInformation = new ProcessStartInfo
                {
                    FileName = parameters[0],
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = SharedObjects.CommandLine.CreateArguments(parameters.Skip(1).ToList())
                };

                using (new TimeMeasurement("Previous version uninstaller process"))
                using (Process process = Process.Start(processInformation))
                {
                    if (!process.WaitForExit((int)AgentObjects.AgentConstants.InstallerTimeout.TotalMilliseconds))
                    {
                        process.Kill();
                        Exceptions.Throw(ErrorMessage.Timeout, "Process is not responding.");
                    }

                    if (process.ExitCode != (int)AgentInstallerStatus.Success)
                        Exceptions.Throw(ErrorMessage.ProcessTerminatedUnexpectedly, "Previous version uninstaller failed: {0}.".Combine(process.ExitCode));
                }
            }
        }

        protected static void SaveVersionIndependentData()
        {
            using (new TimeMeasurement("Global data initialization"))
            {
                string passwordHash = AgentObjects.AgentPrivateStorage.PasswordHash;
                string secretKey = AgentObjects.AgentPrivateStorage.SecretKey;

                AgentObjects.VersionIndependentPrivateStorage.SaveData(passwordHash, secretKey);
                AgentObjects.VersionIndependentPrivateStorage.IncrementCounter();
            }
        }

        protected void SaveLocalPrivateData()
        {
            using (new TimeMeasurement("Private data (standalone) initialization"))
            {
                string hash = CalculateHash(GetArgument(InstallerArgument.Password));

                AgentObjects.AgentPrivateStorage.SecretKey = SymmetricEncryptionKey.Empty.ToBase64String();
                AgentObjects.AgentPrivateStorage.ServerAddress = AgentObjects.AgentConstants.StandaloneServerAddress;
                AgentObjects.AgentPrivateStorage.ServerPort = 0;
                AgentObjects.AgentPrivateStorage.PasswordHash = hash;
                AgentObjects.AgentPrivateStorage.FirstRun = true;
                AgentObjects.AgentPrivateStorage.Licensed = true;
            }
        }

        protected void SaveRemotePrivateData()
        {
            using (new TimeMeasurement("Private data initialization"))
            {
                string hash = GetArgument(InstallerArgument.PasswordHash);
                string address = GetArgument(InstallerArgument.ServerAddress);
                string keyRequest = GetArgument(InstallerArgument.KeyRequest);

                string hostName = SplitServerAddress(address).Key;
                int port = SplitServerAddress(address).Value;
                var key = SymmetricEncryptionKey.GenerateRandomKey();

                string file = SharedObjects.Constants.MainModuleFolder + Path.GetFileNameWithoutExtension(SharedObjects.Constants.MainModuleFile) + ".key";
                SystemObjects.FileTools.WriteFile(file, new KeyExchangeReply(keyRequest, key).SerializeToXml().ToReadOnlyList());

                AgentObjects.AgentPrivateStorage.SecretKey = key.ToBase64String();
                AgentObjects.AgentPrivateStorage.ServerAddress = hostName;
                AgentObjects.AgentPrivateStorage.ServerPort = port;
                AgentObjects.AgentPrivateStorage.PasswordHash = hash;
                AgentObjects.AgentPrivateStorage.FirstRun = true;
                AgentObjects.AgentPrivateStorage.Licensed = false;
            }
        }

        protected void UpdatePrivateData(bool onlyServerAddress = false)
        {
            using (new TimeMeasurement("Private data update"))
            {
                if (onlyServerAddress)
                    Log.Write("Updating only server address.");

                string address = GetArgument(InstallerArgument.ServerAddress);

                string hostName = SplitServerAddress(address).Key;
                int port = SplitServerAddress(address).Value;
                string key = null;
                string hash = null;

                if (!onlyServerAddress)
                {
                    key = AgentObjects.VersionIndependentPrivateStorage.SecretKey;
                    hash = AgentObjects.VersionIndependentPrivateStorage.PasswordHash;

                    if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(hash))
                        ThrowException(AgentInstallerStatus.PreviousVersionWasNotCorrectlyInstalled);
                }

                AgentObjects.AgentPrivateStorage.ServerAddress = hostName;
                AgentObjects.AgentPrivateStorage.ServerPort = port;
                
                if (onlyServerAddress) 
                    return;

                AgentObjects.AgentPrivateStorage.SecretKey = key;
                AgentObjects.AgentPrivateStorage.PasswordHash = hash;
                AgentObjects.AgentPrivateStorage.FirstRun = true;
            }
        }

        protected static void DeletePrivateData()
        {
            using (new TimeMeasurement("Private data removal"))
            {
                AgentObjects.AgentPrivateStorage.ServerAddress = null;
                AgentObjects.AgentPrivateStorage.ServerPort = 0;

                AgentObjects.AgentPrivateStorage.PasswordHash = null;

                AgentObjects.AgentPrivateStorage.FirstRun = false;
                AgentObjects.AgentPrivateStorage.Licensed = false;

                AgentObjects.AgentPrivateStorage.SecretKey = null;
            }
        }

        #endregion
    }
}
