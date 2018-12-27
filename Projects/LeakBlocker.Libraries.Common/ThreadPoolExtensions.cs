using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Расширения для стандартного Thread Pool'а.
    /// </summary>
    public static class ThreadPoolExtensions
    {
        static ThreadPoolExtensions()
        {
            ThreadPool.SetMaxThreads(128, 128);
            ThreadPool.SetMinThreads(128, 128);
        }

        /// <summary>
        /// Запускает потоки с набором параметров и ожидает их завершения
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        public static void RunAndWait<TParameter>(Action<TParameter> function, ICollection<TParameter> parameters)
            where TParameter : IComparable
        {
            Check.ObjectIsNotNull(function, "function");
            Check.ObjectIsNotNull(parameters, "parameters");

            if (!parameters.Any())
                return;

            int itemsInQueue = parameters.Count;

            var exceptions = new ConcurrentBag<Exception>();

            using (var waitHandle = new ManualResetEvent(false))
            {
                foreach (TParameter parameter in parameters)
                {
                    TParameter localParameter = parameter;

                    ThreadPool.QueueUserWorkItem(state =>
                        {
                            try
                            {
                                function(localParameter);
                            }
                            catch (Exception ex)
                            {
                                exceptions.Add(ex);
                            }

                            Interlocked.Decrement(ref itemsInQueue);

                            if (itemsInQueue == 0)
                                waitHandle.Set();
                        });
                }

                waitHandle.WaitOne();
            }

            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }

        /// <summary>
        /// Запускает функцию для всех аргументов без ожидания результата
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        public static void RunWithNoWait<TParameter>(Action<TParameter> function, IEnumerable<TParameter> parameters)
        {
            WaitCallback waitCallback = state => function((TParameter)state);

            foreach (TParameter parameter in parameters)
            {
                ThreadPool.QueueUserWorkItem(waitCallback, parameter);
            }
        }
    }
}
