using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Storage.Entities
{
    internal interface IKeyedItem
    {
        int Id
        {
            get;
            set;
        }
    }
}
