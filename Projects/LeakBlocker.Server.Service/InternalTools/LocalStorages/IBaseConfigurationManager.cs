using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Server.Service.InternalTools.LocalStorages
{
    internal interface IBaseConfigurationManager<TObject> where TObject : BaseReadOnlyObject
    {
        void Save(TObject obj);

        /// <summary>
        /// Сохраняет, если новый объект не такой же, как и Current
        /// </summary>
        void SaveIfDifferent(TObject obj);

        [CanBeNull]
        TObject Current
        {
            get;
        }
    }
}