using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Server.Service.InternalTools.Extensions;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation
{
    internal sealed class SecurityObjectCache : Disposable, ISecurityObjectCache
    {
        #region UpdateRequest

        private sealed class UpdateRequest
        {
            internal IWaitHandle Handle
            {
                get;
                private set;
            }

            internal IDomainSnapshot Result
            {
                get;
                private set;
            }

            internal UpdateRequest(Credentials credentials, Action<IDomainSnapshot> resultAction)
            {
                Handle = SharedObjects.AsyncInvoker.Invoke(delegate
                {
                    Result = SystemObjects.CreateDomainSnapshot(credentials);
                    resultAction(Result);
                });
            }
        }

        #endregion

        private const int parallelQueryThreads = 16;

        private static readonly string file = SharedObjects.Constants.UserDataFolder + "Domains.bin";

        private static readonly TimeSpan updateInterval = new TimeSpan(1, 0, 0);
        private readonly IScheduler scheduler;

        private readonly object saveSynchronization = new object();

        private readonly ConcurrentDictionary<Guid, UpdateRequest> updateRequests = new ConcurrentDictionary<Guid, UpdateRequest>();
        private readonly ConcurrentDictionary<BaseDomainAccount, IDomainSnapshot> snapshots = new ConcurrentDictionary<BaseDomainAccount, IDomainSnapshot>();

        public event Action Updated;

        public IDomainSnapshot Data
        {
            get
            {
                ThrowIfDisposed();

                using (new TimeMeasurement("Requesting security object cache data", true))
                {
                    IDomainSnapshot result = SystemObjects.CreateEmptyDomainSnapshot();
                    foreach (IDomainSnapshot snapshot in snapshots.Values)
                        result = result.Merge(snapshot);

                    Log.Write("Total objects in cache: {0} computers, {1} domains, {2} groups, {3} users, {4} organizational units.",
                        result.Computers.Count, result.Domains.Count, result.Groups.Count, result.Users.Count, result.OrganizationalUnits.Count);

                    return result;
                }
            }
        }

        internal SecurityObjectCache()
        {
            if (!File.Exists(file))
                Log.Add("Unable to load security object cache from file {0}. File is missing", file);
            else
            {
                try
                {
                    using (Stream fileStream = File.OpenRead(file))
                    {
                        var deserialized = (ReadOnlyDictionary<BaseDomainAccount, IDomainSnapshot>)new BinaryFormatter().Deserialize(fileStream);
                        snapshots.MergeWith(deserialized, true);
                    }
                }
                catch (Exception exception)
                {
                    Log.Write(exception);

                    if (File.Exists(file))
                        File.Delete(file);
                }

                Log.Write("Security object cache was loaded ({0} snapshots).", snapshots.Count);
            }

            scheduler = SharedObjects.CreateScheduler(Update, updateInterval, false);
        }

        public Guid RequestUpdate(Credentials credentials)
        {
            Check.ObjectIsNotNull(credentials, "credentials");

            var request = new UpdateRequest(credentials, delegate(IDomainSnapshot result)
            {
                snapshots[credentials.Domain] = result;
                Save();
            });
            var session = Guid.NewGuid();

            updateRequests.AddOrUpdate(session, request, delegate(Guid existingSession, UpdateRequest existingRequest)
            {
                SharedObjects.ExceptionSuppressor.Run(existingRequest.Handle.Abort);
                return request;
            });

            return session;
        }

        public IDomainSnapshot CheckUpdateRequestResult(Guid session)
        {
            UpdateRequest request = updateRequests.TryGetValue(session);

            if (request == null)
                Exceptions.Throw(ErrorMessage.InvalidOperation, "Session does not exist. Completed requests can be checked only once.");

            if (!request.Handle.Wait(TimeSpan.Zero))
                return null;

            if (request.Result == null)
                Exceptions.Throw(ErrorMessage.Generic, "Request was completed without exceptions, result should be initialized.");

            updateRequests.TryRemove(session);

            return request.Result;
        }

        public BaseDomainAccount GetBaseDomainAccountByNameImmediately(string name, SystemAccessOptions options = default(SystemAccessOptions))
        {
            return SystemObjects.SystemAccountTools.GetBaseDomainAccountByName(name, options);
        }

        protected override void DisposeManaged()
        {
            DisposeSafe(scheduler);
            Save();
        }

        private void Save()
        {
            lock (saveSynchronization)
            {
                using (Stream fileStream = File.Open(file, FileMode.Create))
                {
                    new BinaryFormatter().Serialize(fileStream, snapshots.ToReadOnlyDictionary());
                    Log.Write("Security object cache was saved ({0} snapshots).", snapshots.Count);
                }
            }
        }

        private void Update()
        {
            using (new TimeMeasurement("Updating security object cache"))
            {
#warning Think about how to remove outdated domains

                ReadOnlySet<Credentials> request = StorageObjects.CredentialsManager.GetAllCredentials();

                request.ParallelForEach(delegate(Credentials credentials)
                    {
                        try
                        {
                            snapshots[credentials.Domain] = SystemObjects.CreateDomainSnapshot(credentials);
                            Log.Write("Snapshot for domain {0} was acquired.", credentials.Domain);
                        }
                        catch (Exception exception)
                        {
                            Log.Write("Error during requesting domain snapshot. Credentials: {0}", credentials);
                            Log.Write(exception);

                            WriteErrorToAudit(credentials, exception);
                        }
                    }, parallelQueryThreads);

                Save();

                if (Updated != null)
                    Updated();
            }
        }

        private static void WriteErrorToAudit(Credentials credentials, Exception exception)
        {
            ReadOnlySet<DomainAccount> domains = InternalObjects.ConfigurationStorage.CurrentFullConfiguration.GetAllDomains().ToReadOnlySet();

            var domain = credentials.Domain as DomainAccount;

            if (domain == null)//Пропускаем Local Computers Account
                return;

            if (!domains.Contains(domain))//Пропускаем неактуальные данные
                return;

            InternalObjects.AuditItemHelper.DomainConnectionFailed(domain, credentials.User, exception);
        }
    }
}
