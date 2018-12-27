using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation
{
    internal sealed class TemporaryRemoteFileStorage : Disposable
    {
        #region File templates

        private const string task =
            @"<?xml version=""1.0"" encoding=""UTF-16""?>
                <Task version=""1.2"" xmlns=""http://schemas.microsoft.com/windows/2004/02/mit/task"">
                  <Triggers>
                    <RegistrationTrigger>
                      <Enabled>true</Enabled>
                    </RegistrationTrigger>
                  </Triggers>
                  <Principals>
                    <Principal id=""Author"">
                      <UserId>S-1-5-18</UserId>
                      <RunLevel>HighestAvailable</RunLevel>
                    </Principal>
                  </Principals>
                  <Settings>
                    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>
                    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>
                    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>
                    <AllowHardTerminate>true</AllowHardTerminate>
                    <StartWhenAvailable>false</StartWhenAvailable>
                    <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>
                    <IdleSettings>
                      <StopOnIdleEnd>false</StopOnIdleEnd>
                      <RestartOnIdle>false</RestartOnIdle>
                    </IdleSettings>
                    <AllowStartOnDemand>true</AllowStartOnDemand>
                    <Enabled>true</Enabled>
                    <Hidden>true</Hidden>
                    <RunOnlyIfIdle>false</RunOnlyIfIdle>
                    <WakeToRun>false</WakeToRun>
                    <ExecutionTimeLimit>PT1H</ExecutionTimeLimit>
                    <Priority>7</Priority>
                  </Settings>
                  <Actions Context=""Author"">
                    <Exec>
                      <Command>cmd</Command>
                      <Arguments>/c %WINDIR%\{0}.bat task</Arguments>
                    </Exec>
                  </Actions>
                </Task>";

        private const string script =
            @"
                IF ""%1""==""task"" GOTO RUN
                
                ver | find ""5.1""
                if %ERRORLEVEL% == 0 goto RUN
                ver | find ""5.2""
                if %ERRORLEVEL% == 0 goto RUN

                :TASK

                IF EXIST %~dp0{0}.txt DEL /S /Q %~dp0{0}.txt
                schtasks /Create /TN ""INSTALLER_{0}"" /XML %~dp0{0}.XML /F
                :LOOP
                TIMEOUT 5 /NOBREAK
                IF EXIST %~dp0{0}.txt GOTO BREAK
                GOTO LOOP
                :BREAK
                schtasks /Delete /TN ""INSTALLER_{0}"" /F
                GOTO END

                :RUN

                IF EXIST %~dp0{0}.txt DEL /S /Q %~dp0{0}.txt
                SET ERRORLEVEL=
                %~dp0{0}.exe {1}
                echo %ERRORLEVEL% > %~dp0{0}.txt
                GOTO END

                :END
                ";

        #endregion

        private readonly SystemAccessOptions accessOptions;
        private readonly string taskFile;
        private readonly string statusFile;
        private readonly string targetFile;
        private readonly string scriptFile;
        private readonly string keyFile;

        internal string Name
        {
            get;
            private set;
        }

        internal TemporaryRemoteFileStorage(string name, string localFile, string arguments, SystemAccessOptions accessOptions)
        {
            Check.StringIsMeaningful(name, "name");
            Check.StringIsMeaningful(localFile, "localFile");
            Check.StringIsMeaningful(arguments, "arguments");

            this.accessOptions = accessOptions;

            string remotePath = @"\\{0}\Admin$\{1}".Combine(accessOptions.SystemName ?? Environment.MachineName, name);

            Name = name;

            taskFile = remotePath + ".xml";
            statusFile = remotePath + ".txt";
            targetFile = remotePath + ".exe";
            scriptFile = remotePath + ".bat";
            keyFile = remotePath + ".key";

            string scriptData = script.Combine(name, arguments);
            string taskData = task.Combine(name);

            SystemObjects.FileTools.CopyFile(localFile, targetFile, accessOptions);
            SystemObjects.FileTools.WriteFile(scriptFile, Encoding.ASCII.GetBytes(scriptData).ToReadOnlyList(), accessOptions);
            SystemObjects.FileTools.WriteFile(taskFile, Encoding.ASCII.GetBytes(taskData).ToReadOnlyList(), accessOptions);
        }

        internal int ReadStatus()
        {
            IReadOnlyCollection<byte> data = SystemObjects.FileTools.ReadFile(statusFile, accessOptions);
            string exitCodeString = Encoding.ASCII.GetString(data.ToArray()).Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).LastOrDefault() ?? string.Empty;

            int exitCode;
            if (!int.TryParse(exitCodeString, NumberStyles.Any, CultureInfo.InvariantCulture, out exitCode))
            {
                Log.Write("Incorrect exit code '{0}' from file {1}", exitCodeString, statusFile);
                Exceptions.Throw(ErrorMessage.InvalidData, "Unable to retrieve exit code from installation module");
            }
            return exitCode;
        }

        internal IReadOnlyCollection<byte> ReadKey()
        {
            return SystemObjects.FileTools.ReadFile(keyFile, accessOptions);
        }

        protected override void DisposeManaged()
        {
            SharedObjects.ExceptionSuppressor.Run(SystemObjects.FileTools.Delete, taskFile, accessOptions);
            SharedObjects.ExceptionSuppressor.Run(SystemObjects.FileTools.Delete, statusFile, accessOptions);
            SharedObjects.ExceptionSuppressor.Run(SystemObjects.FileTools.Delete, targetFile, accessOptions);
            SharedObjects.ExceptionSuppressor.Run(SystemObjects.FileTools.Delete, scriptFile, accessOptions);
            SharedObjects.ExceptionSuppressor.Run(SystemObjects.FileTools.Delete, keyFile, accessOptions);
        }
    }
}
