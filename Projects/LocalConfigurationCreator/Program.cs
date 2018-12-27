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
using LeakBlocker.Libraries.SystemTools.Entities.Management;

namespace LocalConfigurationCreator
{
    internal static class Program
    {
        private static readonly string configurationsFolder = CreateConfigurationsFolder();


        private static void Main()
        {
            try
            {
                CreateReadonlyConfiguration();
            }
            catch (Exception ex)
            {
                File.WriteAllText("error.txt", ex.ToString());

                throw;
            }
        }

        private static BaseComputerAccount LocalComputer
        {
            get
            {
                return DirectoryServicesProvider.GetComputerByName(Environment.MachineName).Key;
            }
        }



        private static void CreateReadonlyConfiguration()
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
                        new[]{LocalComputer}.ToReadOnlySet()
                        ),
                        new ActionData(BlockActionType.ReadOnly, AuditActionType.DeviceAndFiles)
                    ), }.ToReadOnlySet(),
                ReadOnlySet<BaseTemporaryAccessCondition>.Empty);

            string path = Path.Combine(configurationsFolder, "ReadOnlyConfiguration ({0}).lbConfig".Combine(LocalComputer.ShortName));

            using (var file = File.Open(path, FileMode.Create))
            {
                config.SerializeToXml(file);
            }
        }

        private static string CreateConfigurationsFolder()
        {
            string exePath = Process.GetCurrentProcess().MainModule.FileName;

            var currentDir = Path.GetDirectoryName(exePath);

            Check.ObjectIsNotNull(currentDir);

            var result = Path.Combine(currentDir, "Configurations");

            if (!Directory.Exists(result))
                Directory.CreateDirectory(result);

            return result;
        }
    }
}
