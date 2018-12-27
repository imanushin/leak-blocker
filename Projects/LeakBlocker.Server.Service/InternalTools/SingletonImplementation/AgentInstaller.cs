using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation
{
    internal sealed class AgentInstaller : IAgentInstaller
    {
        AgentInstallerStatus IAgentInstaller.Install(BaseComputerAccount computer, Credentials credentials)
        {
            using (new ExceptionNotifier("Agent setup on computer {0}", computer))
            using (new TimeMeasurement("Agent setup on computer {0}".Combine(computer)))
            {
                var request = new KeyExchangeRequest();
                string agentArguments = PrepareAgentArguments(request);

                if (computer == SystemObjects.SystemAccountTools.LocalComputer)
                    return StartLocalInstaller(computer, request, agentArguments);

                Log.Write("Using credentials: {0}.", credentials);
                var accessOptions = new SystemAccessOptions(credentials.User, credentials.Password, computer.FullName);
                var scope = InitializeManagementScope(computer, accessOptions);

                Version version = SystemObjects.Prerequisites.GetRemoteSystemVersion(accessOptions);
                bool is64 = SystemObjects.FileTools.Exists(@"\\{0}\Admin$\SysWOW64\kernel32.dll".Combine(computer.FullName), accessOptions);

                if (version.Major * 10 + version.Minor < 51)
                    Exceptions.Throw(ErrorMessage.OldSystemVersion);
                if (!CheckUpdates(scope, version))
                    Exceptions.Throw(ErrorMessage.ThrowDirectly, AgentInstallerStrings.ErrorMessageServicePack);

                bool requireNetFramework = !SystemObjects.FileTools.Exists(@"\\{0}\Admin$\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll".Combine(computer.FullName), accessOptions);
                bool requireWindowsImagingComponent32 = requireNetFramework && !is64 && !SystemObjects.FileTools.Exists(@"\\{0}\Admin$\System32\Windowscodecs.dll".Combine(computer.FullName), accessOptions);
                bool requireWindowsImagingComponent64 = requireNetFramework && is64 && !SystemObjects.FileTools.Exists(@"\\{0}\Admin$\System32\Windowscodecs.dll".Combine(computer.FullName), accessOptions);

                if (requireWindowsImagingComponent32)
                {
                    using (var remoteFiles = new TemporaryRemoteFileStorage("WIC", SharedObjects.Constants.MainModuleFolder + @"..\..\Downloads\wic32.exe",
                         @"/quiet /log:""%ALLUSERSPROFILE%\Delta Corvi\Logs\WICSetup_{0}.log""".Combine(Time.Now), accessOptions))
                    {
                        StartProcessAndWait(remoteFiles, scope, TimeSpan.FromMinutes(15));
                        int status = remoteFiles.ReadStatus();
                        CheckSetupResult(AgentInstallerStrings.ModuleNameWindowsImagingComponent, status);
                    }
                }

                if (requireWindowsImagingComponent64)
                {
                    using (var remoteFiles = new TemporaryRemoteFileStorage("WIC", SharedObjects.Constants.MainModuleFolder + @"..\..\Downloads\wic64.exe",
                         @"/quiet /log:""%ALLUSERSPROFILE%\Delta Corvi\Logs\WICSetup_{0}.log""".Combine(Time.Now), accessOptions))
                    {
                        StartProcessAndWait(remoteFiles, scope, TimeSpan.FromMinutes(15));
                        int status = remoteFiles.ReadStatus();
                        CheckSetupResult(AgentInstallerStrings.ModuleNameWindowsImagingComponent, status);
                    }
                }

                if (requireNetFramework)
                {
                    using (var remoteFiles = new TemporaryRemoteFileStorage("NetFx40", SharedObjects.Constants.MainModuleFolder + @"..\..\Downloads\netfx40.exe",
                         @"/q /log ""%ALLUSERSPROFILE%\Delta Corvi\Logs\NetFrameworkSetup_{0}.log""".Combine(Time.Now), accessOptions))
                    {
                        StartProcessAndWait(remoteFiles, scope, TimeSpan.FromMinutes(45));
                        int status = remoteFiles.ReadStatus();
                        CheckSetupResult(AgentInstallerStrings.ModuleNameNetFramework, status);
                    }
                }

                using (var remoteFiles = new TemporaryRemoteFileStorage("LeakBlockerAgentInstaller", SharedObjects.Constants.MainModuleFolder + @"LeakBlocker.Agent.Distributive.exe",
                    agentArguments, accessOptions))
                {
                    StartProcessAndWait(remoteFiles, scope, TimeSpan.FromMinutes(15));
                    var status = (AgentInstallerStatus)remoteFiles.ReadStatus();

                    if (status == AgentInstallerStatus.Success)
                    {
                        IReadOnlyCollection<byte> data = remoteFiles.ReadKey();
                        KeyExchangeReply reply = BaseObjectSerializer.DeserializeFromXml<KeyExchangeReply>(data.ToArray());
                        InternalObjects.AgentKeyManager.AddAgentKey(computer, request.Decrypt(reply));
                    }

                    return status;
                }
            }
        }

        private static AgentInstallerStatus StartLocalInstaller(BaseComputerAccount computer, KeyExchangeRequest request, string agentArguments)
        {
            string keyFile = SharedObjects.Constants.MainModuleFolder + @"LeakBlocker.Agent.Distributive.key";

            try
            {
                Log.Write("Starting local process");
                AgentInstallerStatus result = StartProcess(SharedObjects.Constants.MainModuleFolder + @"LeakBlocker.Agent.Distributive.exe", agentArguments);

                if (result == AgentInstallerStatus.Success)
                {
                    IReadOnlyCollection<byte> data = SystemObjects.FileTools.ReadFile(keyFile);
                    KeyExchangeReply reply = BaseObjectSerializer.DeserializeFromXml<KeyExchangeReply>(data.ToArray());
                    InternalObjects.AgentKeyManager.AddAgentKey(computer, request.Decrypt(reply));
                }

                return result;
            }
            finally
            {
                SharedObjects.ExceptionSuppressor.Run(SystemObjects.FileTools.Delete, keyFile, default(SystemAccessOptions));
            }
        }

        private static ManagementScope InitializeManagementScope(BaseComputerAccount computer, SystemAccessOptions accessOptions)
        {
            var scope = new ManagementScope(@"\\{0}\root\CIMV2".Combine(computer.FullName));
            if (computer != SystemObjects.SystemAccountTools.LocalComputer)
            {
                scope.Options.Impersonation = ImpersonationLevel.Impersonate;
                scope.Options.Username = accessOptions.UserName;
                scope.Options.Password = accessOptions.Password;
                scope.Options.EnablePrivileges = true;
            }
            return scope;
        }

        private static string PrepareAgentArguments(string keyRequest)
        {
            using (var provider = new SHA1CryptoServiceProvider())
            {
                string hostName = Dns.GetHostEntry("localhost").HostName;
                string passwordHash = Convert.ToBase64String(provider.ComputeHash(Encoding.Unicode.GetBytes(InternalObjects.AgentSetupPasswordManager.Current.Value)));

                return "-i -ir -pwh {0} -srv {1}:{2} -key {3}".Combine(passwordHash, hostName, SharedObjects.Constants.DefaultTcpPort, keyRequest);
            }
        }

        private static void StartProcessAndWait(TemporaryRemoteFileStorage files, ManagementScope scope, TimeSpan timeout)
        {
            using (var classObject = new ManagementClass(scope, new ManagementPath("Win32_Process"), null))
            using (ManagementBaseObject inputParameters = classObject.GetMethodParameters("Create"))
            {
                var exitedProcesses = new ConcurrentBag<uint>();
                bool handleEvents = true;
                uint? processIdentifier = null;

                IWaitHandle waitHandle = SharedObjects.AsyncInvoker.Invoke(delegate
                {
                    using (var watcher = new ManagementEventWatcher(scope, new WqlEventQuery("SELECT * FROM WIN32_ProcessStopTrace")))
                    {
                        watcher.Options.Timeout = timeout;
                        while (handleEvents)
                        {
                            ManagementBaseObject waitResult = watcher.WaitForNextEvent();
                            var currentProcessIdentifier = (uint)waitResult["ProcessId"];
                            exitedProcesses.Add(currentProcessIdentifier);
                            if (processIdentifier.HasValue && exitedProcesses.Contains(processIdentifier.Value))
                                break;
                        }
                    }
                });

                inputParameters["CommandLine"] = @"cmd /c %WINDIR%\{0}.bat".Combine(files.Name);

                using (ManagementBaseObject outputParams = classObject.InvokeMethod("Create", inputParameters, null))
                {
                    if (outputParams == null)
                        Exceptions.Throw(ErrorMessage.InvalidOperation, "Win32_Process Create returned null.");

                    var returnCode = (uint)outputParams.SystemProperties["ReturnValue"].Value;
                    CheckReturnCode(returnCode);

                    processIdentifier = (uint)outputParams["ProcessId"];

                    waitHandle.Wait(timeout);
                    handleEvents = false;

                    if (!exitedProcesses.Contains(processIdentifier.Value))
                        Exceptions.Throw(ErrorMessage.Timeout);

                    //SharedObjects.ExceptionSuppressor.Run(delegate
                    //{
                    //    var selectQuery = new SelectQuery("Select * from Win32_Process Where ProcessId = " + processIdentifier);
                    //    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, selectQuery))
                    //    {
                    //        foreach (ManagementObject queryResult in searcher.Get())
                    //        {
                    //            using (queryResult)
                    //            {
                    //                queryResult.InvokeMethod("Terminate", null);
                    //            }
                    //        }
                    //    }
                    //});
                }
            }
        }

        private static AgentInstallerStatus StartProcess(string file, string arguments)
        {
            var processStartInfo = new ProcessStartInfo(file, arguments);

            using (var process = Process.Start(processStartInfo))
            {
                if (!process.WaitForExit(TimeSpan.FromMinutes(15).ConvertToTimeout()))
                    Exceptions.Throw(ErrorMessage.Timeout);

                return (AgentInstallerStatus)process.ExitCode;
            }
        }

        private static void CheckReturnCode(uint code)
        {
            switch (code)
            {
                case 0:
                    return;
                case 2:
                    Exceptions.Throw(ErrorMessage.AccessDenied, "Win32_Process Create returned 2.");
                    return;
                case 3:
                    Exceptions.Throw(ErrorMessage.InsufficientPrivileges, "Win32_Process Create returned 3.");
                    return;
                case 8:
                    Exceptions.Throw(ErrorMessage.Generic, "Win32_Process Create returned 8.");
                    return;
                case 9:
                    Exceptions.Throw(ErrorMessage.PathNotFound, "Win32_Process Create returned 9.");
                    return;
                case 21:
                    Exceptions.Throw(ErrorMessage.IncorrectParameter, "Win32_Process Create returned 21.");
                    return;
                default:
                    Exceptions.Throw(ErrorMessage.Generic, "Win32_Process Create returned {0}.".Combine(code));
                    return;
            }
        }

        private static bool CheckUpdates(ManagementScope scope, Version systemVersion)
        {
            if (systemVersion.Major >= 6)
                return true;

            using (var searcher = new ManagementObjectSearcher(scope, new ObjectQuery("SELECT * FROM Win32_OperatingSystem")))
            {
                foreach (ManagementObject searchResult in searcher.Get())
                {
                    using (searchResult)
                    {
                        return (systemVersion.Minor + (ushort)searchResult.Properties["ServicePackMajorVersion"].Value) >= 4;
                    }
                }
            }
            Exceptions.Throw(ErrorMessage.NotFound, "Cannot get service pack version.");
            return false;
        }

        private static void CheckSetupResult(string moduleName, int exitCode)
        {
            if (exitCode != 0)
                Exceptions.Throw(ErrorMessage.ThrowDirectly, new InvalidOperationException(AgentInstallerStrings.TemplateError.Combine(moduleName, exitCode)));
        }
    }
}
