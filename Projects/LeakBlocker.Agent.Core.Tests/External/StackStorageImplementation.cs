using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class StackStorageImplementation : IStackStorage
    {
        readonly List<string> strings = new List<string>();

        public void Write(IReadOnlyCollection<string> data)
        {
            Check.CollectionHasOnlyMeaningfulData(data);

            strings.AddRange(data);
        }

        public void Read(int sizeLimit, Action<IReadOnlyCollection<string>> processingCallback)
        {
            Check.ObjectIsNotNull(processingCallback);
            Check.IntegerIsNotLessThanZero(sizeLimit);

            processingCallback(strings.ToReadOnlySet());
            strings.Clear();
        }

        public void Dispose()
        {
        }


        public void Delete()
        {
           // base.RegisterMethodCall("Delete");
        }
    }
}
