using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Server.Installer.InternalTools
{
    internal interface IProductRegistrator
    {
        void RegisterProduct();

        void UnregisterProduct();

        [CanBeNull]
        string GetRemoveString();
    }
}
