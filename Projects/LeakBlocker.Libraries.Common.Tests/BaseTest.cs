using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Implementations;
using LeakBlocker.Libraries.Common.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Libraries.Common.Tests
{
    [TestClass]
    public abstract class BaseTest
    {
        private static readonly string[] wrongStrings = new[] { string.Empty, "  \n " };

        private MockRepository mocks;

        protected MockRepository Mocks
        {
            get
            {
                if (mocks == null)
                    BaseTestInitialize();

                return mocks;
            }
        }

        [TestInitialize]
        public void BaseTestInitialize()
        {
            if (mocks != null)
                return;

            TestContextData.UpdateTestId();
            mocks = new MockRepository();

            SharedObjects.Singletons.Constants.SetTestImplementation(new ConstantsStub());
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());
        }

        [TestCleanup]
        public void BaseTestCleanup()
        {
            mocks = null;
        }

        #region ParametersVerify

        #region Null Verify

        protected void CheckFirstForNull<T1>(Action<T1> action)
            where T1 : class
        {
            try
            {
                action(null);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }
        }

        protected void CheckFirstForNull<T1, T2>(T2 t2, Action<T1, T2> action)
            where T1 : class
        {
            Action<T1> oneArgumentAction = (t1) => action(t1, t2);

            CheckFirstForNull(oneArgumentAction);
        }

        protected void CheckFirstForNull<T1, T2, T3>(T2 t2, T3 t3, Action<T1, T2, T3> action)
            where T1 : class
        {
            Action<T1> oneArgumentAction = (t1) => action(t1, t2, t3);

            CheckFirstForNull(oneArgumentAction);
        }

        protected void CheckSecondForNull<T1, T2>(T1 t1, Action<T1, T2> action)
            where T2 : class
        {
            Action<T2> oneArgumentAction = (t2) => action(t1, t2);

            CheckFirstForNull(oneArgumentAction);
        }

        protected void CheckSecondForNull<T1, T2, T3>(T1 t1, T3 t3, Action<T1, T2, T3> action)
            where T2 : class
        {
            Action<T2> oneArgumentAction = (t2) => action(t1, t2, t3);

            CheckFirstForNull(oneArgumentAction);
        }

        protected void CheckThirdForNull<T1, T2, T3>(T1 t1, T2 t2, Action<T1, T2, T3> action)
            where T3 : class
        {
            Action<T3> oneArgumentAction = (t3) => action(t1, t2, t3);

            CheckFirstForNull(oneArgumentAction);
        }

        #endregion Null Verify

        #region Empty string checking

        protected void CheckFirstStringForForMeaningful(Action<string> action)
        {
            CheckFirstForNull(action);

            CheckFirst(action, wrongStrings);
        }

        protected void CheckFirstStringForForMeaningful<T2>(Action<string, T2> action, T2 t2)
        {
            CheckFirstForNull(t2, action);

            CheckFirst(action, t2, wrongStrings);
        }

        protected void CheckFirstStringForForMeaningful<T2, T3>(Action<string, T2, T3> action, T2 t2, T3 t3)
        {
            CheckFirstForNull(t2, t3, action);

            CheckFirst(action, t2, t3, wrongStrings);
        }

        #endregion Empty string checking

        #region Collection checking

        protected void CheckFirstCollectionIsGood<T1>(Action<IEnumerable<T1>> action)
            where T1 : class
        {
            CheckFirstForNull(action);

            CheckFirst<IEnumerable<T1>>(action, CreateWrongCollections<T1>());
        }

        protected void CheckFirstCollectionIsGood<T1, T2>(Action<IEnumerable<T1>, T2> action, T2 t2)
             where T1 : class
        {
            CheckFirstForNull(t2, action);

            CheckFirst<IEnumerable<T1>, T2>(action, t2, CreateWrongCollections<T1>());
        }

        protected void CheckFirstCollectionIsGood<T1, T2, T3>(Action<IEnumerable<T1>, T2, T3> action, T2 t2, T3 t3)
              where T1 : class
        {
            CheckFirstForNull(t2, t3, action);

            CheckFirst<IEnumerable<T1>, T2, T3>(action, t2, t3, CreateWrongCollections<T1>());
        }

        private static IEnumerable<T>[] CreateWrongCollections<T>()
            where T : class
        {
            return new[]
                {
                      new List<T>(),
                      new List<T>(){null}
            };
        }

        #endregion Collection checking

        #region Common checking

        protected void CheckFirst<T1>(Action<T1> action, params T1[] wrongArgs)
        {
            foreach (T1 arg in wrongArgs)
            {
                try
                {
                    action(arg);
                }
                catch (Exception ex)
                {
                    CheckArgumentException(ex, arg);
                }
            }
        }

        protected void CheckFirst<T1, T2>(Action<T1, T2> action, T2 t2, params T1[] wrongArgs)
        {
            Action<T1> oneArgumentAction = (t1) => action(t1, t2);

            CheckFirst(oneArgumentAction);
        }

        protected void CheckFirst<T1, T2, T3>(Action<T1, T2, T3> action, T2 t2, T3 t3, params T1[] wrongArgs)
        {
            Action<T1> oneArgumentAction = (t1) => action(t1, t2, t3);

            CheckFirst(oneArgumentAction);
        }

        #endregion

        #endregion

        private static void CheckArgumentException<T>(Exception ex, T arg)
        {
            Assert.IsInstanceOfType(ex, typeof(ArgumentException), string.Format("Value \"{0}\" of type {1} is wrong argument. Argument exception expected", arg, typeof(T)));
        }
    }
}
