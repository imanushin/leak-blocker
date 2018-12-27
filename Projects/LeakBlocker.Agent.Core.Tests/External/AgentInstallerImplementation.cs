using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class AgentInstallerImplementation : BaseTestImplementation, IAgentInstaller
    {
        public string CalculatePasswordHash(string password)
        {
            Check.StringIsMeaningful(password);
            return "_" + password;
        }

        public int Start()
        {
            base.RegisterMethodCall("Start");
            return 0;
        }

        public bool WaitForInstaller(TimeSpan timeout)
        {
            base.RegisterMethodCall("WaitForInstaller", timeout);
            return true;
        }

        public void UninstallSelf()
        {
            base.RegisterMethodCall("UninstallSelf");
        }

        public void InstallFileSystemDriver()
        {
            base.RegisterMethodCall("InstallFileSystemDriver");
        }

        public void RemoveFileSystemDriver()
        {
            base.RegisterMethodCall("RemoveFileSystemDriver");
        }
    }
}
