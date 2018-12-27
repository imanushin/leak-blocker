using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Implementations
{
    internal sealed class AsyncInvoker : IAsyncInvoker
    {
        public IWaitHandle Invoke(Action action)
        {
            Check.ObjectIsNotNull(action, "action");

            var exceptionContainer = new ExceptionContainer();

            ThreadStart workAction = () =>
                                    {
                                        try
                                        {
                                            action();
                                        }
                                        catch (Exception ex)
                                        {
                                            exceptionContainer.Error = ex;
                                            Log.Write(ex);
                                        }
                                    };

            var result = new Thread(workAction) { IsBackground = true };

            result.Start();

            return new Waiter(result, exceptionContainer);
        }

        public IWaitHandle Invoke<T>(Action<T> action, T argument)
        {
            Check.ObjectIsNotNull(action, "action");

            return Invoke(() => action(argument));
        }

        public IWaitHandle Invoke<T1, T2>(Action<T1, T2> action, T1 argument1, T2 argument2)
        {
            Check.ObjectIsNotNull(action, "action");

            return Invoke(() => action(argument1, argument2));
        }

        public IWaitHandle Invoke<T1, T2, T3>(Action<T1, T2, T3> action, T1 argument1, T2 argument2, T3 argument3)
        {
            Check.ObjectIsNotNull(action, "action");

            return Invoke(() => action(argument1, argument2, argument3));
        }

        private sealed class ExceptionContainer
        {
            public Exception Error
            {
                get;
                set;
            }
        }

        private sealed class Waiter : IWaitHandle
        {
            private readonly Thread thread;
            private readonly ExceptionContainer error;

            public Waiter(Thread thread, ExceptionContainer container)
            {
                this.thread = thread;
                error = container;
            }

            public void Wait()
            {
                Wait(TimeSpan.MaxValue);
            }

            public bool Wait(TimeSpan timeout)
            {
                if (!thread.Join(timeout.ConvertToTimeout()))
                    return false;

                if (error.Error != null)
                    throw new InvalidOperationException("Execution failed. See inner exception for details", error.Error);

                return true;
            }

            public void Abort()
            {
                if (thread.IsAlive)
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
            }
        }
    }
}
