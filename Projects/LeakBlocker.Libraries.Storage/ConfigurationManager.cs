using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Storage.Entities;
using LeakBlocker.Libraries.Storage.InternalTools;

namespace LeakBlocker.Libraries.Storage
{
    internal sealed class ConfigurationManager : IConfigurationManager
    {
        public void SaveConfiguration(ProgramConfiguration configuration)
        {
            Check.ObjectIsNotNull(configuration, "configuration");

            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                DbProgramConfiguration.ConvertFromProgramConfiguration(configuration, model);

                model.SaveChanges();
            }
        }

        public ProgramConfiguration GetLastConfiguration()
        {
            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                if (!model.ProgramConfigurationSet.Any())
                    return null;

                int maxId = model.ProgramConfigurationSet.Max(item => item.Id);

                DbProgramConfiguration result = model.ProgramConfigurationSet.Find(maxId);

                return result.GetProgramConfiguration();
            }
        }
    }
}
