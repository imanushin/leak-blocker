using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Tests
{
    public sealed class FakeAsyncInvoker : IAsyncInvoker
    {
        private readonly Queue<Delegate> actions = new Queue<Delegate>();

        public IWaitHandle Invoke(Action action)
        {
            actions.Enqueue(action);

            return new FakeWakeHandle();
        }

        public IWaitHandle Invoke<T>(Action<T> action, T argument)
        {
            actions.Enqueue(new Action(() => action(argument)));

            return new FakeWakeHandle();
        }

        public IWaitHandle Invoke<T1, T2>(Action<T1, T2> action, T1 argument1, T2 argument2)
        {
            actions.Enqueue(new Action(() => action(argument1, argument2)));

            return new FakeWakeHandle();
        }

        public IWaitHandle Invoke<T1, T2, T3>(Action<T1, T2, T3> action, T1 argument1, T2 argument2, T3 argument3)
        {
            actions.Enqueue(new Action(() => action(argument1, argument2, argument3)));

            return new FakeWakeHandle();
        }

        public void RunAllActions()
        {
            foreach (Delegate action in actions)
            {
                action.DynamicInvoke();
            }
        }

        private sealed class FakeWakeHandle : IWaitHandle
        {
            public void Wait()
            {
                throw new NotImplementedException();//Не придумал, что здесь может быть
            }

            public bool Wait(TimeSpan timeout)
            {
                throw new NotImplementedException();
            }

            public void Abort()
            {
                throw new NotImplementedException();
            }
        }
    }
}
