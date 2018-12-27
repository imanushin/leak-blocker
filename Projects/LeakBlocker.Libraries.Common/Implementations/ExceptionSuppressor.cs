using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Implementations
{
    internal sealed class ExceptionSuppressor : IExceptionSuppressor
    {
        public void Run(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        public void Run<T1>(Action<T1> action, T1 argument)
        {
            Run(() => action(argument));
        }

        public void Run<T1, T2>(Action<T1, T2> action, T1 argument1, T2 argument2)
        {
            Run(() => action(argument1, argument2));
        }

        public void Run<T1, T2, T3>(Action<T1, T2, T3> action, T1 argument1, T2 argument2, T3 argument3)
        {
            Run(() => action(argument1, argument2, argument3));
        }

        public void Run<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 argument1, T2 argument2, T3 argument3, T4 argument4)
        {
            Run(() => action(argument1, argument2, argument3, argument4));
        }

        public void Run<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, T1 argument1, T2 argument2, T3 argument3, T4 argument4, T5 argument5)
        {
            Run(() => action(argument1, argument2, argument3, argument4, argument5));
        }
    }
}
