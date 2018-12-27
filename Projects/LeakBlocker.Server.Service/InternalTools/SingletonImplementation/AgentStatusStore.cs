using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation
{
    internal sealed class AgentStatusStore : IAgentStatusStore
    {
        private static readonly ManagedComputerData unknownData = new ManagedComputerData(ManagedComputerStatus.Unknown, DeviceAccessMap.Empty);

        private readonly Dictionary<BaseComputerAccount, ManagedComputerData> scope = new Dictionary<BaseComputerAccount, ManagedComputerData>();
        private readonly Dictionary<BaseComputerAccount, Time> lastCommunicationTime = new Dictionary<BaseComputerAccount, Time>();
        private ReadOnlySet<BaseComputerAccount> licensedComputers = null;

        private readonly object syncRoot = new object();

        public void SetComputerData(BaseComputerAccount computer, ManagedComputerData data)
        {
            Check.ObjectIsNotNull(computer, "computer");
            Check.ObjectIsNotNull(data, "data");

            lock (syncRoot)
            {
                ManagedComputerData previousData = scope.TryGetValue(computer);

                if (previousData == null)
                    Log.Add("Computer {0} was added to scope with status {1}", computer, data.Status);

                if (previousData != null && previousData.Status != data.Status)
                    Log.Add("Status of computer {0} changed from {1} to {2}", computer, previousData.Status, data.Status);

                scope[computer] = data;

                if (data.Status == ManagedComputerStatus.Working)
                    InternalObjects.AgentsSetupListStorage.AddAndSave(computer);
            }
        }

        private void UpdateLicenseData()
        {
            List<BaseComputerAccount> computers = scope.Keys.ToList();

            computers.Sort();

            int licensesCount = InternalObjects.LicenseStorage.LicenseCount;

            licensedComputers = computers.Take(licensesCount).ToReadOnlySet();
        }

        public ManagedComputerData GetComputerData(BaseComputerAccount computer)
        {
            lock (syncRoot)
            {
                ManagedComputerData result = scope.TryGetValue(computer);

                if (result == null)
                    throw new InvalidOperationException("Computer {0} is out of the scope".Combine(computer));

                return result;
            }
        }

        public void UpdateFromScope()
        {
            lock (syncRoot)
            {
                ReadOnlySet<BaseComputerAccount> currentScope = InternalObjects.ScopeManager.CurrentScope();
                ReadOnlySet<BaseComputerAccount> oldItems = scope.Keys.ToReadOnlySet();

                ReadOnlySet<BaseComputerAccount> newItems = currentScope.Without(oldItems).ToReadOnlySet();
                ReadOnlySet<BaseComputerAccount> itemsToRemove = oldItems.Without(currentScope).ToReadOnlySet();

                if (itemsToRemove.Any())
                {
                    itemsToRemove.ForEach(computer => scope.TryRemove(computer));

                    string removedComputers = string.Join(", ", itemsToRemove);

                    Log.Add("Computers removed from scope: {0}", removedComputers);
                }

                foreach (BaseComputerAccount computer in newItems)
                {
                    if (!lastCommunicationTime.ContainsKey(computer))
                    {
                        lastCommunicationTime[computer] = Time.Unknown;
                    }

                    SetComputerData(computer, unknownData);
                }

                UpdateLicenseData();
            }
        }

        public bool IsUnderLicense(BaseComputerAccount computer)
        {
            lock (syncRoot)
            {
                if (licensedComputers == null)//То есть мы пока еще не инициализировались, а значит временно говорим, что всем всё можно ...
                    return true;              // ... в любом случае через 15 минут агент опять запросит лицензию и получит уже верные данные

                return licensedComputers.Contains(computer);
            }
        }

        public ReadOnlyDictionary<BaseComputerAccount, ManagedComputerData> GetManagedScope()
        {
            lock (syncRoot)
            {
                return scope.ToReadOnlyDictionary();
            }
        }

        public Time GetLastCommunicationTimeUtc(BaseComputerAccount computer)
        {
            lock (syncRoot)
            {
                Time result = lastCommunicationTime.TryGetValue(computer);

                if (result == null)
                    throw new InvalidOperationException("Computer {0} is out of the scope".Combine(computer));

                return result;
            }
        }

        public void UpdateLastCommunicationTimeAsCurrent(BaseComputerAccount computer)
        {
            lock (syncRoot)
            {
                Time time = Time.Now;

                lastCommunicationTime[computer] = time;
            }
        }

        public void ResetLastCommunicationTime(BaseComputerAccount computer)
        {
            Check.ObjectIsNotNull(computer, "computer");

            lastCommunicationTime[computer] = Time.Unknown;
        }
    }
}
