using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.SystemTools;

namespace LeakBlocker.Libraries.Common.Tests.SystemTools
{
    public sealed class FakeThreadPool : IThreadPool
    {
        private readonly ConcurrentQueue<Action> actions = new ConcurrentQueue<Action>();

        public void Dispose()
        {
        }

        public void RunAllActions()
        {
            actions.ForEach(action => action());
        }

        public void EnqueueAction(Action action)
        {
            actions.Enqueue(action);
        }

        public void RunAndWait(IReadOnlyCollection<Action> newActions)
        {
        newActions.ForEach(actions.Enqueue);
        }
    }
}
