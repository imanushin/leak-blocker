using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Storage.Entities;
using LeakBlocker.Libraries.Storage.InternalTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Storage.Tests
{
    public abstract class BaseDatabaseObjectsTests : BaseStorageTest
    {
        /// <summary>
        /// Only internal use
        /// </summary>
        internal BaseDatabaseObjectsTests()
        {
        }

        internal void TestForInsertAndSelect<TEntity, TDbBaseEntity, TDbEntity>(
            Func<IDatabaseModel, DbSet<TDbBaseEntity>> dbSet,
            IEnumerable<TEntity> entities,
            Func<TEntity, IDatabaseModel, TDbEntity> dbEntityCreator,
            Func<TDbEntity, TEntity> entityCreator)

            where TEntity : BaseEntity
            where TDbBaseEntity : class, IKeyedItem
            where TDbEntity : TDbBaseEntity
        {
            Mocks.ReplayAll();

            int entityNumber = 0;

            foreach (TEntity entity in entities)
            {
                int key;

                using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
                {
                    TDbBaseEntity dbEntity = dbEntityCreator(entity, model);

                    model.SaveChanges();

                    key = dbEntity.Id;
                }

                using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
                {
                    DbSet<TDbBaseEntity> set = dbSet(model);

                    var query = (DbQuery<TDbEntity>)set.OfType<TDbEntity>();

                    var resultDbEntity = query.ToList().First(e => e.Id == key);

                    TEntity targetEntity = entityCreator(resultDbEntity);

                    Assert.AreEqual(entity, targetEntity);
                }

                using (IDatabaseModel model = StorageObjects.CreateDatabaseModel())
                {
                    dbEntityCreator(entity, model);

                    model.SaveChanges();//Тестируем повторное сохранение данных
                }

                Console.WriteLine("Processed {0} entities".Combine(++entityNumber));
            }
        }
    }
}
