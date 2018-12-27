using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Agent.Core
{
    internal interface IStackStorage : IDisposable
    {
        void Write(IReadOnlyCollection<string> data);

        void Read(int sizeLimit, Action<IReadOnlyCollection<string>> processingCallback);

        void Delete();
    }
}
