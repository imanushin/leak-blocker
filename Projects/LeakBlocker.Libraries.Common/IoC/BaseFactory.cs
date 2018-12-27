using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.IoC
{
    /// <summary>
    /// Factory for creating implementations of TInterface. Unit tests must queue their own implementations for each created instance.
    /// </summary>
    /// <typeparam name="TInterface">Interface whose implementations should be returned.</typeparam>
    public abstract class BaseFactory<TInterface> where TInterface : class
    {
        #region TestImplementationData

        private sealed class TestImplementationData
        {
            public TestImplementationData(TInterface instance)
            {
                Instance = instance;
            }

            public TestImplementationData(Delegate creator)
            {
                Creator = creator;
            }

            public TInterface Instance
            {
                get;
                private set;
            }

            public Delegate Creator
            {
                get;
                private set;
            }
        }

        #endregion

        private readonly object syncRoot = new object();
        private readonly Queue<TestImplementationData> testObjectsQueue = new Queue<TestImplementationData>();
        private readonly Delegate constructor;
        private int currentLocalTestId;

        /// <summary>
        /// Created a factory.
        /// </summary>
        /// <param name="constructor">Делегат для создания объекта.</param>
        protected BaseFactory(Delegate constructor)
        {
            this.constructor = constructor;
        }
        
        /// <summary>
        /// Создает объект.
        /// В случае теста выдает объект из очереди.
        /// </summary>
        /// <param name="parameters">Parameters for initializing the instance.</param>
        /// <returns>Object instance.</returns>
        protected TInterface CreateInstance(params object[] parameters)
        {
            if (TestContextData.IsTestContext)
            {
                lock (syncRoot)
                {
                    if (TestContextData.CurrentCommonTestId > currentLocalTestId || testObjectsQueue.Count == 0)
                        Exceptions.Throw(ErrorMessage.InvalidOperation, "Implementation of {0} was not defined".Combine(typeof(TInterface).Name));

                    TestImplementationData data = testObjectsQueue.Dequeue();

                    TInterface result = data.Instance ?? (TInterface)data.Creator.DynamicInvoke(parameters);

                    if (data.Creator != null)
                        testObjectsQueue.Enqueue(data);

                    return result;
                }
            }

            return (TInterface)constructor.DynamicInvoke(parameters);
        }
        
        internal void EnqueueTestImplementation(TInterface value)
        {
            lock (syncRoot)
            {
                if (!TestContextData.IsTestContext)
                    Exceptions.Throw(ErrorMessage.InvalidOperation, "This method should not be used out of the test environment");

                if (TestContextData.CurrentCommonTestId != currentLocalTestId)
                {
                    currentLocalTestId = TestContextData.CurrentCommonTestId;
                    testObjectsQueue.Clear();
                }

                testObjectsQueue.Enqueue(new TestImplementationData(value));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="creator"></param>
        protected void EnqueueConstructor(Delegate creator)
        {
            lock (syncRoot)
            {
                if (!TestContextData.IsTestContext)
                    Exceptions.Throw(ErrorMessage.InvalidOperation, "This method should not be used out of the test environment");

                if (TestContextData.CurrentCommonTestId != currentLocalTestId)
                {
                    currentLocalTestId = TestContextData.CurrentCommonTestId;
                    testObjectsQueue.Clear();
                }

                testObjectsQueue.Enqueue(new TestImplementationData(creator));
            }
        }     
    }
}
