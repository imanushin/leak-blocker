using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Implementations.AgentDataStorageObjects;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Entities;

namespace LeakBlocker.Agent.Core.Implementations
{
    internal sealed class AgentDataStorage : IAgentDataStorage
    {
        private readonly object synchronization = new object();
        private readonly string file;
        private readonly DataCache data;
        
        private readonly ILocalDataCache localDataCache = SystemObjects.CreateLocalDataCache();

        private volatile bool deleted;

        ProgramConfiguration IAgentDataStorage.Settings
        {
            get
            {
                lock (synchronization)
                {
                    return data.Settings;
                }
            }
            set
            {
                Check.ObjectIsNotNull(value);

                lock (synchronization)
                {
                    if (data.Settings == value)
                    {
                        Log.Write("Settings are the same.");
                        return;
                    }

                    Log.Write("Updating settings.");

                    data.Settings = value;
                    Save();
                }
            }
        }

        ReadOnlySet<CachedUserData> ILocalDataCache.Users
        {
            get
            {
                lock (synchronization)
                {
                    return data.Users.ToReadOnlySet();
                }
            }
        }

        CachedComputerData ILocalDataCache.Computer
        {
            get
            {
                lock (synchronization)
                {
                    return data.Computer ?? localDataCache.Computer;
                }
            }
        }

        BaseUserAccount ILocalDataCache.ConsoleUser
        {
            get
            {
                lock (synchronization)
                {
                    return data.ConsoleUser;
                }
            }
        }

        ReadOnlyDictionary<DeviceDescription, DeviceAccessType> IAgentDataStorage.DeviceStates
        {
            get
            {
                lock (synchronization)
                {
                    return data.DeviceStates.ToReadOnlyDictionary();
                }
            }
        }

        void IAgentDataStorage.SaveDeviceState(DeviceDescription device, DeviceAccessType state)
        {
            Check.ObjectIsNotNull(device, "device");
            Check.EnumerationValueIsDefined(state, "state");

            lock (synchronization)
            {
                if (data.DeviceStates.TryGetValue(device) == state)
                {
                    Log.Write("Device state is the same (State: {0} Device: {1})".Combine(state, device));
                    return;
                }

                Log.Write("Saving device state (State: {0} Device: {1})".Combine(state, device));
                data.DeviceStates[device] = state;
                Save();
            }
        }

        void IAgentDataStorage.Delete()
        {
            lock (synchronization)
            {
                Log.Write("Deleting data storage.");
                SharedObjects.ExceptionSuppressor.Run(() => SystemObjects.FileTools.Delete(file));
                deleted = true;
            }
        }

        internal AgentDataStorage(string file)
        {
            Check.StringIsMeaningful(file, "file");

            this.file = file;
            data = LoadDiskCache(file);

            SharedObjects.ExceptionSuppressor.Run(UpdateData);

            Log.Write("Agent data storage was initialized.");
        }

        void ILocalDataCache.Update()
        {
            using (new TimeMeasurement("Cache update"))
            {
                lock (synchronization)
                {
                    UpdateData();
                    Save();
                }
            }
        }
        
        private void UpdateData()
        {
            using (new TimeMeasurement("Data update"))
            {
                localDataCache.Update();

                data.Computer = localDataCache.Computer;
                data.ConsoleUser = localDataCache.ConsoleUser;
                data.Users.Clear();
                data.Users.AddRange(localDataCache.Users);
            }
        }

        private void Save()
        {
            Log.Write("Saving disk cache.");

            if (deleted)
            {
                Log.Write("Data storage was removed.");
                return;
            }

            try
            {
                var diskCache = new DataDiskCache(data.Users.ToReadOnlySet(), data.DeviceStates.Select(item => new DeviceState(item.Key, item.Value)).ToReadOnlySet(),
                    data.Settings, data.Computer, data.ConsoleUser);
                IReadOnlyCollection<byte> serialized = diskCache.SerializeToXml().ToReadOnlyList();
                SystemObjects.FileTools.WriteFile(file, serialized);
            }
            catch (Exception exception)
            {
                Log.Write(exception);
            }
        }

        private static DataCache LoadDiskCache(string file)
        {
            Log.Write("Loading disk cache.");
            try
            {
                if (!SystemObjects.FileTools.Exists(file))
                {
                    Log.Write("File was not found.");
                    return new DataCache();
                }

                IReadOnlyCollection<byte> serialized = SystemObjects.FileTools.ReadFile(file);
                var diskData = BaseObjectSerializer.DeserializeFromXml<DataDiskCache>(serialized.ToArray());
                return new DataCache(diskData);
            }
            catch (Exception exception)
            {
                Log.Write(exception);
                return new DataCache();
            }
        }
    }
}