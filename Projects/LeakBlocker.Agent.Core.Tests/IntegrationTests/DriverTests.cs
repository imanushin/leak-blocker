using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.SystemTools.Entities.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests.IntegrationTests
{
    /*
    /// <summary>
    /// Всё тестирование ведется на дисках А (FAT 32) и В (NTFS).
    /// Оба диска должны быть подключены и отформатированы
    /// </summary>
    [TestClass]
    public sealed class DriverTests : BaseTest
    {
        private const string testAgentPassword = "13rbhsg93f-a";

        private static readonly string binariesFolder = GetBinariesFolder();
        private static readonly string configurationsFolder = CreateConfigurationsFolder();


        private static readonly BaseComputerAccount localComputer = AccountTools.GetBaseComputerAccountByName(Environment.MachineName);

        private static readonly string pathToReadOnlyConfiguration = CreatePathToReadonlyConfiguration();

        [TestMethod]
        public void BlockFileTestNtfs()
        {
            Mocks.ReplayAll();

            if (!string.Equals(Environment.MachineName, "123", StringComparison.OrdinalIgnoreCase))
                return;

            RunAgentProcess("-i -il -pwd {0}");
            RunAgentProcess("-i -cc -pwd {{0}} -cfg {0}".Combine(pathToReadOnlyConfiguration));

            try
            {
                File.WriteAllText("e:\\1.txt", "123213123123");
            }
            catch //(Exception ex)
            {

            }

            RunAgentProcess("-i -ul -pwd {0}");
        }


        private static void RunAgentProcess(string parametersTemplate)
        {
            string exePath = Path.Combine(binariesFolder, "LeakBlocker.Agent.exe");

            Process installAgentProcess = Process.Start(exePath, parametersTemplate.Combine(testAgentPassword));

            Check.ObjectIsNotNull(installAgentProcess);

            installAgentProcess.Start();

            installAgentProcess.WaitForExit();
        }

        private static string CreatePathToReadonlyConfiguration()
        {
            var config = new ProgramConfiguration(
                2,
                new[]{new Rule(
                    "rule", 
                    12, 
                    new ComputerListRuleCondition(
                        false, 
                        ReadOnlySet<DomainAccount>.Empty,
                        ReadOnlySet<OrganizationalUnit>.Empty,
                        ReadOnlySet<DomainGroupAccount>.Empty,
                        new[]{localComputer}.ToReadOnlySet()
                        ),
                        new ActionData(BlockActionType.ReadOnly, AuditActionType.DeviceAndFiles)
                    ), }.ToReadOnlySet(),
                ReadOnlySet<BaseTemporaryAccessCondition>.Empty);

            string path = Path.Combine(configurationsFolder, "ReadOnlyConfiguration.lbConfig");

            using (var file = File.Open(path, FileMode.Create))
            {
                config.SerializeToXml(file);
            }

            return path;
        }

        private static string GetBinariesFolder()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            int indexOfRoot = currentDirectory.IndexOf("LeakBlocker\\Projects", StringComparison.OrdinalIgnoreCase);

            string rootDirectory = currentDirectory.Substring(0, indexOfRoot);

            return Path.Combine(rootDirectory, "LeakBlocker\\Projects\\Binaries");
        }


        private static string CreateConfigurationsFolder()
        {
            var result = Path.Combine(binariesFolder, "Configurations");

            if (Directory.Exists(result))
                Directory.Delete(result, true);

            Directory.CreateDirectory(result);

            return result;
        }
    }
     */
}
