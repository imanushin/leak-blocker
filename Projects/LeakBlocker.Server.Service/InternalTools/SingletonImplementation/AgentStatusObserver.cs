using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.SystemTools;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation
{
    internal sealed class AgentStatusObserver : IAgentStatusObserver
    {
        private static readonly TimeSpan defaultUpdateInterval = TimeSpan.FromMinutes(1);
        private static readonly TimeSpan maxAgentSilence = Timeouts.DelayBetweenCommunications + Timeouts.DelayBetweenCommunications;

        private static readonly object syncRoot = new object();

        private readonly IScheduler scheduler;

        public AgentStatusObserver()
        {
            scheduler = SharedObjects.CreateScheduler(UpdateStatuses, defaultUpdateInterval);
        }

        public void EnqueueObserving()
        {
            scheduler.RunNow();
        }

        private static void UpdateStatuses()
        {
            lock (syncRoot)
            {
                try
                {
                    using (new TimeMeasurement("Update statuses ", true))
                    {
                        ReadOnlySet<BaseComputerAccount> scope = InternalObjects.ScopeManager.CurrentScope();

                        InternalObjects.AgentStatusStore.UpdateFromScope();

                        var computersForInstall = new ConcurrentBag<BaseComputerAccount>();

                        ThreadPoolExtensions.RunAndWait(computer =>
                        {
                            ManagedComputerData data = InternalObjects.AgentStatusStore.GetComputerData(computer);

                            if (UpdateAgentStatus(data, computer))
                                computersForInstall.Add(computer);
                        }, scope);

                        string statuses = string.Join(", ",
                                                      InternalObjects.AgentStatusStore.GetManagedScope()
                                                                     .Select(item => "Computer: {0}; status: {1}".Combine(item.Key.ShortName, item.Value.Status)));

                        Log.Add("Current statuses: " + statuses);

                        if (computersForInstall.Any())
                        {
                            using (new TimeMeasurement("Agent distribution"))
                            {
                                ReadOnlyList<BaseComputerAccount> installQueue = computersForInstall.ToReadOnlyList();

                                ReadOnlySet<BaseComputerAccount> cleanedSetupList =
                                    InternalObjects.AgentsSetupListStorage
                                                   .Current
                                                   .Without(computersForInstall)
                                                   .Intersect(scope)
                                                   .ToReadOnlySet();
                                
                                InternalObjects.AgentsSetupListStorage.SaveIfDifferent(cleanedSetupList);

                                ThreadPoolExtensions.RunAndWait(InternalObjects.AgentManager.InstallAgent, installQueue);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.GetType().Name == "ExpectationViolationException")//Это значит, что какие-то объекты не подменили
                        throw;

                    Log.Write(ex);
                    InternalObjects.AuditItemHelper.ErrorInStatusUpdate(ex.Message);
                }
            }
        }

        private static bool UpdateAgentStatus(ManagedComputerData data, BaseComputerAccount computer)
        {
            if (IsRecentlyUpdated(computer))
                return false;

            switch (data.Status)
            {
                case ManagedComputerStatus.Working:
                    InternalObjects.AuditItemHelper.AgentIsNotResponding(computer);
                    SetNewStatus(computer, ManagedComputerStatus.Unknown); //На следующем такте начнем разъяснительную работу: или выключен, или дейтсвительно агент куада-то делся

                    return false;

                case ManagedComputerStatus.TurnedOff:

                    if (InternalObjects.AgentManager.IsComputerTurnedOn(computer))
                    {
                        //Если вдруг включился, то еще дается время для того, чтобы агент начал работать с сервером
                        SetNewStatus(computer, ManagedComputerStatus.Unknown);
                    }

                    InternalObjects.AgentStatusStore.UpdateLastCommunicationTimeAsCurrent(computer);

                    return false;

                default:
                    if (!InternalObjects.AgentManager.IsComputerTurnedOn(computer))
                    {
                        SetNewStatus(computer, ManagedComputerStatus.TurnedOff);
                        InternalObjects.AgentStatusStore.UpdateLastCommunicationTimeAsCurrent(computer);

                        return false;
                    }

                    if (Equals(InternalObjects.AgentStatusStore.GetLastCommunicationTimeUtc(computer), Time.Unknown)//То есть если он только что попал в список (Time==Unknown) ...
                        && InternalObjects.AgentsSetupListStorage.Current.Contains(computer)) // ... и на нем агент ставился во время предыдущей работы сервиса, то ждем 30 минут, чтобы он одумался
                    {
                        InternalObjects.AgentStatusStore.UpdateLastCommunicationTimeAsCurrent(computer);

                        return false;
                    }

                    SetNewStatus(computer, ManagedComputerStatus.Unknown);

                    return true;
            }
        }

        private static void SetNewStatus(BaseComputerAccount computer, ManagedComputerStatus status)
        {
            var newData = new ManagedComputerData(status, DeviceAccessMap.Empty);

            InternalObjects.AgentStatusStore.SetComputerData(computer, newData);
        }

        private static bool IsRecentlyUpdated(BaseComputerAccount computer)
        {
            return InternalObjects.AgentStatusStore.GetLastCommunicationTimeUtc(computer).Add(maxAgentSilence) > Time.Now;
        }

        public void Dispose()
        {
            Disposable.DisposeSafe(scheduler);
        }
    }
}
