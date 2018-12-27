using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.IoC
{
    /// <summary>
    /// Синглтон с дополнительной функциональностью, позволяющей заменять реализацию интерфейса.
    /// Для Runtime'а - это обычный класс, аналог Singletone'ам.
    /// Для тестов он сбрасывает все инстансы после каждого теста, никогда не создает объект из оригинального класса
    /// </summary>
    /// <typeparam name="TInterface">Interface whose implementation is wrapped by the current instance.</typeparam>
    public sealed class Singleton<TInterface> where TInterface : class
    {
        private readonly object syncRoot = new object();

        private int currentLocalTestId;

        private TInterface targetObject;

        private readonly Func<TInterface> constructor;

        /// <summary>
        /// Конструктор синглтона.
        /// Внутренний объект создается отложенно
        /// </summary>
        /// <param name="constructor">Делегат для создания объекта.</param>
        public Singleton(Func<TInterface> constructor)
        {
            this.constructor = constructor;
        }

        /// <summary>
        /// Выдает объект синглтона.
        /// Отложенно создает сам объект.
        /// В случае работы тестов создание объекта происходит при каждом запуске теста.
        /// </summary>
        public TInterface Instance
        {
            get
            {
                if (TestContextData.IsTestContext)
                {
                    lock (syncRoot)
                    {
                        if (TestContextData.CurrentCommonTestId > currentLocalTestId || targetObject == null)
                            Exceptions.Throw(ErrorMessage.InvalidOperation, "Implementation of {0} was not defined".Combine(typeof(TInterface).Name));

                        return targetObject;
                    }
                }

                if (targetObject != null)
                    return targetObject;

                lock (syncRoot)
                {
                    if (targetObject == null)
                        targetObject = constructor();

                    return targetObject;
                }
            }
        }

        internal void SetTestImplementation(TInterface implementation)
        {
            if (!TestContextData.IsTestContext)
                Exceptions.Throw(ErrorMessage.InvalidOperation, "This method should not be used out of the test environment.");

            targetObject = implementation;

            currentLocalTestId = TestContextData.CurrentCommonTestId;
        }
    }
}
