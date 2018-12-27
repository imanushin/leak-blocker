using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common.SystemTools
{
    internal sealed class NativeThreadPool : Disposable, IThreadPool
    {
        private readonly ReadOnlySet<Thread> threads;

        private readonly ManualResetEvent waitHandle = new ManualResetEvent(false);

        private readonly ConcurrentQueue<Action> actionsInThread = new ConcurrentQueue<Action>();

        private readonly object syncRoot = new object();

        public NativeThreadPool(int threadCount)
        {
            threads = Enumerable.Repeat(0, threadCount).Select(item => new Thread(ThreadLoop)
            {
                Name = "Native Thread Pool"
            }).ToReadOnlySet();

            threads.ForEach(thread => thread.Start());
        }

        private void ThreadLoop()
        {
            while (!Disposed)
            {
                waitHandle.WaitOne();

                if (Disposed)
                    return;

                while (true)
                {
                    Action action = GetAction();

                    if (action == null)
                        break;

                    SharedObjects.ExceptionSuppressor.Run(action);
                }
            }
        }

        private Action GetAction()
        {
            Action action;

            if (!actionsInThread.TryDequeue(out action))
            {
                lock (syncRoot)
                {
                    if (!actionsInThread.TryDequeue(out action))
                    {
                        waitHandle.Reset();

                        return null;
                    }
                }
            }
            return action;
        }

        public void EnqueueAction(Action action)
        {
            actionsInThread.Enqueue(action);

            lock (syncRoot)
            {
                waitHandle.Set();
            }
        }

        public void RunAndWait(IReadOnlyCollection<Action> actions)
        {
            int runningCount = actions.Count;

            if(runningCount == 0)
                return;

            var exceptions = new ConcurrentBag<Exception>();

            using (var localWaiter = new ManualResetEvent(false))
            {
                foreach (Action action in actions)
                {
                    actionsInThread.Enqueue(() =>
                        {
                            try
                            {
                                action();
                            }
                            catch (Exception ex)
                            {
                                exceptions.Add(ex);
                            }

                            int currentCount = Interlocked.Decrement(ref runningCount);

                            if (currentCount == 0)
                                localWaiter.Set();
                        });
                }

                lock (syncRoot)
                {
                    if (Disposed)
                        return;

                    waitHandle.Set();
                }

                localWaiter.WaitOne();
            }

            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }

        protected override void DisposeManaged()
        {
            Action temp;

            while (actionsInThread.TryDequeue(out temp))//Очистка очереди, ибо нет метода Clear
            {
            }

            lock (syncRoot)
            {
                waitHandle.Set();
            }

            foreach (Thread thread in threads.Where(thread => !thread.Join(10000)))
            {
                try
                {
                    thread.Abort();
                }
                catch (ThreadAbortException)
                {
                    Log.Write("Thread was aborted.");
                    Thread.ResetAbort();
                }
            }

            DisposeSafe(waitHandle);
            base.DisposeManaged();
        }
    }
}
