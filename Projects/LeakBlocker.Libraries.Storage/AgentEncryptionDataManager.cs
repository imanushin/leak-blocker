using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Storage.Entities;
using LeakBlocker.Libraries.Storage.InternalTools;

namespace LeakBlocker.Libraries.Storage
{
    internal sealed class AgentEncryptionDataManager : IAgentEncryptionDataManager
    {
        public void SaveAgent(AgentEncryptionData data)
        {
            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                DbAgentEncryptionData.ConvertFromAgentEncryptionData(data, model);

                model.SaveChanges();
            }
        }

        public ReadOnlySet<AgentEncryptionData> GetAllData()
        {
            using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
            {
                return model.AgentEncryptionDataSet.ToList().Select(item => item.GetAgentEncryptionData()).ToReadOnlySet();
            }
        }
    }
}
