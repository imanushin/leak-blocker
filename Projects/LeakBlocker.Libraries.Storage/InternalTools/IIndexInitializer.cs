using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Storage.InternalTools
{
    internal interface IIndexInitializer : IDatabaseInitializer<DatabaseModel>
    {
    }
}