using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests.Implementations
{
    [TestClass]
    public sealed class AsyncInvokerTest
    {
        [TestMethod]
        public void AsyncInvoke_Success()
        {
            var target = new AsyncInvoker();

            bool flag = false;

            IWaitHandle waiter = target.Invoke(() =>
            {
                Thread.Sleep(1000);
                flag = false;
            });

            flag = true;//Проверяем, что предыдущий метод сразу же вернет управление

            waiter.Wait();

            Assert.IsFalse(flag);

            flag = true;
            waiter.Wait();//Проверяем, что повторный вызов не приведет к ошибке ...
            Assert.IsTrue(flag);// ... и поток не будет запущен повторно
        }

        [TestMethod]
        public void AsyncInvoke_Fail()
        {
            var target = new AsyncInvoker();

            var exception = new Exception();

            IWaitHandle waiter = target.Invoke(() =>
            {
                Thread.Sleep(1000);
                throw exception;
            });

            try
            {
                waiter.Wait();

                Assert.Fail();//Должно выпасть исключение
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual(exception, ex.InnerException);
            }
        }
    }
}
