using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Storage.Entities;

namespace LeakBlocker.Libraries.Storage.InternalTools
{
    internal sealed class DatabaseCache<TBaseEntity, TDbBaseEntity>
        where TDbBaseEntity : class, IKeyedItem
        where TBaseEntity : BaseEntity
    {
        private readonly Dictionary<TBaseEntity, TDbBaseEntity> entityMap = new Dictionary<TBaseEntity, TDbBaseEntity>();
        private readonly Func<TBaseEntity, TDbBaseEntity> entityFinder;

        private readonly ICrossRequestCache idsCache = StorageObjects.CrossRequestCache;

        public DatabaseCache(
            Func<TBaseEntity, TDbBaseEntity> entityFinder)
        {
            this.entityFinder = entityFinder;
        }

        [CanBeNull]
        public TDbEntity Get<TDbEntity, TEntity>(TEntity entity, DbSet<TDbBaseEntity> entitySet)
            where TDbEntity : TDbBaseEntity
            where TEntity : TBaseEntity
        {
            TDbBaseEntity baseResult = entityMap.TryGetValue(entity);

            if (baseResult != null)
            {
                return (TDbEntity)baseResult;
            }

            int id = idsCache.Get(entity);

            TDbBaseEntity resultFromDb;

            if (id > 0)
            {
                resultFromDb = entitySet.Find(id);
            }
            else
            {
                resultFromDb = entityFinder(entity);

                if (resultFromDb != null)
                    idsCache.Push(entity, resultFromDb.Id);
            }

            entityMap[entity] = resultFromDb;

            return (TDbEntity)resultFromDb;
        }

        public void Add<TDbEntity, TEntity>(TEntity entity, TDbEntity dbEntity)
            where TDbEntity : TDbBaseEntity
            where TEntity : TBaseEntity
        {
            entityMap[entity] = dbEntity;
            idsCache.Push(entity, dbEntity.Id);
        }
    }
}
