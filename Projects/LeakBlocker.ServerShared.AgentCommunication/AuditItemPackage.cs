using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Audit;

namespace LeakBlocker.ServerShared.AgentCommunication
{
    /// <summary>
    /// Container for transferring multiple audit items at once. 
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1035:ICollectionImplementationsHaveStronglyTypedMembers"), Serializable]
    [DataContract(IsReference = true)]
    [DebuggerDisplay("Count = {Count}")]
    public sealed class AuditItemPackage : BaseReadOnlyObject, IReadOnlyCollection<AgentAuditItem>
    {
        private static readonly AuditItemPackage empty = new AuditItemPackage(ReadOnlySet<AgentAuditItem>.Empty);

        /// <summary>
        /// Empty package.
        /// </summary>
        public static AuditItemPackage Empty
        {
            get
            {
                return empty;
            }
        }

        [DataMember]
        private readonly ReadOnlySet<AgentAuditItem> data;

        /// <summary>
        /// Number of items in the package.
        /// </summary>
        public int Count
        {
            get
            {
                return data.Count;
            }
        }

        /// <summary>
        /// Конструктор объекта
        /// </summary>
        public AuditItemPackage(IReadOnlyCollection<AgentAuditItem> data)
        {
            Check.ObjectIsNotNull(data, "data");

            var users = data.Select(item => item.User).ToHashSet();
            var devices = data.Select(item => item.Device).ToHashSet();

            HashSet<AgentAuditItem> allItems = data.Select(item => new AgentAuditItem(item.EventType, item.Time, users.First(user => Equals(user, item.User)),
                item.TextData, item.AdditionalTextData, devices.First(device => Equals(device, item.Device)), item.Configuration)).ToHashSet();

            var mergedItemTypes = new Dictionary<AuditItemType, AuditItemType>
            {
                { AuditItemType.ReadFileBlocked, AuditItemType.ReadFileBlockedMultiple },
                { AuditItemType.ReadFileAllowed, AuditItemType.ReadFileAllowedMultiple },
                { AuditItemType.ReadFileTemporarilyAllowed, AuditItemType.ReadFileTemporarilyAllowedMultiple },
                { AuditItemType.WriteFileBlocked, AuditItemType.WriteFileBlockedMultiple },
                { AuditItemType.WriteFileAllowed, AuditItemType.WriteFileAllowedMultiple },
                { AuditItemType.WriteFileTemporarilyAllowed, AuditItemType.WriteFileTemporarilyAllowedMultiple },
                { AuditItemType.ReadWriteFileBlocked, AuditItemType.ReadWriteFileBlockedMultiple },
                { AuditItemType.ReadWriteFileAllowed, AuditItemType.ReadWriteFileAllowedMultiple },
                { AuditItemType.ReadWriteFileTemporarilyAllowed, AuditItemType.ReadWriteFileTemporarilyAllowedMultiple }
            };

            HashSet<AgentAuditItem> result = allItems.Where(item => !mergedItemTypes.Keys.Contains(item.EventType)).ToHashSet();
            var duplicatedItems = new Queue<AgentAuditItem>(allItems.Without(result));

            var items = new Dictionary<AgentAuditItemData, HashSet<AgentAuditItem>>();

            while (duplicatedItems.Count > 0)
            {
                AgentAuditItem item = duplicatedItems.Dequeue();

                var currentItemData = new AgentAuditItemData(item);

                if (!items.ContainsKey(currentItemData))
                    items[currentItemData] = new HashSet<AgentAuditItem>();
                items[currentItemData].Add(item);
            }

            foreach (KeyValuePair<AgentAuditItemData, HashSet<AgentAuditItem>> currentItem in items)
            {
                Time minTime = currentItem.Value.Select(item => item.Time).Min();
                Time maxTime = currentItem.Value.Select(item => item.Time).Max();

                Time currentTime = minTime;
                while (currentTime <= maxTime)
                {
                    currentTime = currentTime.Add(TimeSpan.FromMinutes(1));

                    ReadOnlySet<AgentAuditItem> mergedItems = currentItem.Value.Where(item => item.Time < currentTime).ToReadOnlySet();

                    AgentAuditItem mergedItem;

                    AgentAuditItem baseItem = mergedItems.First();

                    Time time = mergedItems.Select(item => item.Time).Max();

                    if (mergedItems.Count > 1)
                    {
                        mergedItem = new AgentAuditItem(mergedItemTypes[baseItem.EventType], time, baseItem.User, baseItem.TextData,
                            baseItem.AdditionalTextData, baseItem.Device, baseItem.Configuration, mergedItems.Count);
                    }
                    else
                    {
                        mergedItem = baseItem;
                    }

                    result.Add(mergedItem);

                    mergedItems.ForEach(item => currentItem.Value.Remove(item));
                }
            }

            this.data = result.ToReadOnlySet();
        }

        /// <summary>
        /// Метод для реализации IEnumerable
        /// </summary>
        public IEnumerator<AgentAuditItem> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        #region Equality

        /// <summary>
        /// Returns all object that should be involved in hash code calculation and equalirty checks.
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return data;
        }

        #endregion

        #region Explicit implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        #endregion
    }
}
