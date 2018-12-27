using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Implementations
{
    internal sealed class Scheduler : Disposable, IScheduler
    {
        private const int threadJoinTimeout = 5000;

        private readonly Thread mainThread;
        private readonly Thread timerThread;
        private readonly Action action;

        private bool suspended;
        private readonly TimeSpan interval;

        private readonly AutoResetEvent taskIntervalEvent = new AutoResetEvent(false);

        private volatile bool running;
        private volatile bool runAgain;
        
        internal Scheduler(Action action, TimeSpan interval, bool suspended = true)
        {
            Check.ObjectIsNotNull(action, "action");

            this.suspended = suspended;
            this.action = action;
            this.interval = interval;

            timerThread = new Thread(TimerThread)
            {
                Name = "Scheduler Timer Thread",
                IsBackground = true
            };
            timerThread.Start();

            mainThread = new Thread(MainThread)
            {
                Name = "Scheduler Main Thread",
                IsBackground = true
            };
            mainThread.Start();
        }

        protected override void DisposeManaged()
        {
            taskIntervalEvent.Set();

            if (!mainThread.Join(threadJoinTimeout))
            {
                try
                {
                    mainThread.Abort();
                }
                catch (ThreadAbortException)
                {
                    Log.Write("Thread was aborted.");
                    Thread.ResetAbort();
                }
            }

            DisposeSafe(taskIntervalEvent);
        }

        public void RunNow()
        {
            if (!running)
                taskIntervalEvent.Set();
            else
                runAgain = true;
        }

        private void TimerThread()
        {
            while (true)
            {
                Thread.Sleep(interval.ConvertToTimeout());
                if (Disposed)
                    break;
                taskIntervalEvent.Set();
            }
        }

        private void MainThread()
        {
            try
            {
                while (true)
                {
                    running = true;
                    if (suspended)
                        suspended = false;
                    else
                    {
                        try
                        {
                            action();
                        }
                        catch (ThreadAbortException)
                        {
                        }
                        catch (Exception exception)
                        {
                            Log.Write(exception);
                        }
                    }

                    running = false;

                    if (Disposed)
                        break;
                    if (runAgain)
                    {
                        runAgain = false;
                        continue;
                    }

                    taskIntervalEvent.WaitOne(interval.ConvertToTimeout());

                    if (Disposed)
                        break;
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
                Log.Write("Scheduler thread was aborted.");
                return;
            }
        }
    }
}
