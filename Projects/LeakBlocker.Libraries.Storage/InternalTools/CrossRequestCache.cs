using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;

namespace LeakBlocker.Libraries.Storage.InternalTools
{
    internal sealed class CrossRequestCache : ICrossRequestCache
    {
        private const int maxObjectsCount = 4096;

        private readonly DisplacingCache<BaseEntity, int> idsCache = new DisplacingCache<BaseEntity, int>(maxObjectsCount);

        public void Push(BaseEntity entity, int id)
        {
            idsCache.Push(entity, id);
        }

        public int Get(BaseEntity entity)
        {
            return idsCache.Get(entity);
        }
    }
}