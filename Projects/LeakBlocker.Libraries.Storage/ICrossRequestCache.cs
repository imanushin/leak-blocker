using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;

namespace LeakBlocker.Libraries.Storage
{
    internal interface ICrossRequestCache
    {
        void Push(BaseEntity entity, int id);

        int Get(BaseEntity entity);
    }
}