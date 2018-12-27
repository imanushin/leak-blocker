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
    public sealed class Factory<TInterface> : BaseFactory<TInterface> where TInterface : class
    {
        /// <summary>
        /// Created a factory.
        /// </summary>
        /// <param name="constructor">Делегат для создания объекта.</param>
        public Factory(Func<TInterface> constructor)
            : base(constructor)
        {
        }

        internal void EnqueueConstructor(Func<TInterface> constructor)
        {
            base.EnqueueConstructor(constructor);
        }
        
        /// <summary>
        /// Создает объект. В случае теста выдает объект из очереди.
        /// </summary>
        public TInterface CreateInstance()
        {
            return base.CreateInstance();
        }
    }

    /// <summary>
    /// Factory for creating implementations of TInterface. Unit tests must queue their own implementations for each created instance.
    /// </summary>
    /// <typeparam name="TInterface">Interface whose implementations should be returned.</typeparam>
    /// <typeparam name="T1">Constructor argument.</typeparam>
    public sealed class Factory<TInterface, T1> : BaseFactory<TInterface> where TInterface : class
    {
        /// <summary>
        /// Created a factory.
        /// </summary>
        /// <param name="constructor">Делегат для создания объекта.</param>
        public Factory(Func<T1, TInterface> constructor)
            : base(constructor)
        {
        }

        internal void EnqueueConstructor(Func<T1, TInterface> constructor)
        {
            base.EnqueueConstructor(constructor);
        }

        /// <summary>
        /// Создает объект. В случае теста выдает объект из очереди.
        /// </summary>
        public TInterface CreateInstance(T1 firstArgument)
        {
            return base.CreateInstance(firstArgument);
        }
    }
    
    /// <summary>
    /// Factory for creating implementations of TInterface. Unit tests must queue their own implementations for each created instance.
    /// </summary>
    /// <typeparam name="TInterface">Interface whose implementations should be returned.</typeparam>
    /// <typeparam name="T1">Constructor argument.</typeparam>
    /// <typeparam name="T2">Constructor argument.</typeparam>
    public sealed class Factory<TInterface, T1, T2> : BaseFactory<TInterface> where TInterface : class
    {
        /// <summary>
        /// Created a factory.
        /// </summary>
        /// <param name="constructor">Делегат для создания объекта.</param>
        public Factory(Func<T1, T2, TInterface> constructor)
            : base(constructor)
        {
        }

        internal void EnqueueConstructor(Func<T1, T2, TInterface> constructor)
        {
            base.EnqueueConstructor(constructor);
        }

        /// <summary>
        /// Создает объект. В случае теста выдает объект из очереди.
        /// </summary>
        public TInterface CreateInstance(T1 firstArgument, T2 secondArgument)
        {
            return base.CreateInstance(firstArgument, secondArgument);
        }
    }

    /// <summary>
    /// Factory for creating implementations of TInterface. Unit tests must queue their own implementations for each created instance.
    /// </summary>
    /// <typeparam name="TInterface">Interface whose implementations should be returned.</typeparam>
    /// <typeparam name="T1">Constructor argument.</typeparam>
    /// <typeparam name="T2">Constructor argument.</typeparam>
    /// <typeparam name="T3">Constructor argument.</typeparam>
    public sealed class Factory<TInterface, T1, T2, T3> : BaseFactory<TInterface> where TInterface : class
    {
        /// <summary>
        /// Created a factory.
        /// </summary>
        /// <param name="constructor">Делегат для создания объекта.</param>
        public Factory(Func<T1, T2, T3, TInterface> constructor)
            : base(constructor)
        {
        }

        internal void EnqueueConstructor(Func<T1, T2, T3, TInterface> constructor)
        {
            base.EnqueueConstructor(constructor);
        }

        /// <summary>
        /// Создает объект. В случае теста выдает объект из очереди.
        /// </summary>
        public TInterface CreateInstance(T1 firstArgument, T2 secondArgument, T3 thirdArgument)
        {
            return base.CreateInstance(firstArgument, secondArgument, thirdArgument);
        }
    }

    /// <summary>
    /// Factory for creating implementations of TInterface. Unit tests must queue their own implementations for each created instance.
    /// </summary>
    /// <typeparam name="TInterface">Interface whose implementations should be returned.</typeparam>
    /// <typeparam name="T1">Constructor argument.</typeparam>
    /// <typeparam name="T2">Constructor argument.</typeparam>
    /// <typeparam name="T3">Constructor argument.</typeparam>
    /// <typeparam name="T4">Constructor argument.</typeparam>
    public sealed class Factory<TInterface, T1, T2, T3, T4> : BaseFactory<TInterface> where TInterface : class
    {
        /// <summary>
        /// Created a factory.
        /// </summary>
        /// <param name="constructor">Делегат для создания объекта.</param>
        public Factory(Func<T1, T2, T3, T4, TInterface> constructor)
            : base(constructor)
        {
        }

        internal void EnqueueConstructor(Func<T1, T2, T3, T4, TInterface> constructor)
        {
            base.EnqueueConstructor(constructor);
        }

        /// <summary>
        /// Создает объект. В случае теста выдает объект из очереди.
        /// </summary>
        public TInterface CreateInstance(T1 firstArgument, T2 secondArgument, T3 thirdArgument, T4 fourthArgument)
        {
            return base.CreateInstance(firstArgument, secondArgument, thirdArgument, fourthArgument);
        }
    }

    /// <summary>
    /// Factory for creating implementations of TInterface. Unit tests must queue their own implementations for each created instance.
    /// </summary>
    /// <typeparam name="TInterface">Interface whose implementations should be returned.</typeparam>
    /// <typeparam name="T1">Constructor argument.</typeparam>
    /// <typeparam name="T2">Constructor argument.</typeparam>
    /// <typeparam name="T3">Constructor argument.</typeparam>
    /// <typeparam name="T4">Constructor argument.</typeparam>
    /// <typeparam name="T5">Constructor argument.</typeparam>
    public sealed class Factory<TInterface, T1, T2, T3, T4, T5> : BaseFactory<TInterface> where TInterface : class
    {
        /// <summary>
        /// Created a factory.
        /// </summary>
        /// <param name="constructor">Делегат для создания объекта.</param>
        public Factory(Func<T1, T2, T3, T4, T5, TInterface> constructor)
            : base(constructor)
        {
        }

        internal void EnqueueConstructor(Func<T1, T2, T3, T4, T5, TInterface> constructor)
        {
            base.EnqueueConstructor(constructor);
        }

        /// <summary>
        /// Создает объект. В случае теста выдает объект из очереди.
        /// </summary>
        public TInterface CreateInstance(T1 firstArgument, T2 secondArgument, T3 thirdArgument, T4 fourthArgument, T5 fifthArgument)
        {
            return base.CreateInstance(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument);
        }
    }

    /// <summary>
    /// Factory for creating implementations of TInterface. Unit tests must queue their own implementations for each created instance.
    /// </summary>
    /// <typeparam name="TInterface">Interface whose implementations should be returned.</typeparam>
    /// <typeparam name="T1">Constructor argument.</typeparam>
    /// <typeparam name="T2">Constructor argument.</typeparam>
    /// <typeparam name="T3">Constructor argument.</typeparam>
    /// <typeparam name="T4">Constructor argument.</typeparam>
    /// <typeparam name="T5">Constructor argument.</typeparam>
    /// <typeparam name="T6">Constructor argument.</typeparam>
    public sealed class Factory<TInterface, T1, T2, T3, T4, T5, T6> : BaseFactory<TInterface> where TInterface : class
    {
        /// <summary>
        /// Created a factory.
        /// </summary>
        /// <param name="constructor">Делегат для создания объекта.</param>
        public Factory(Func<T1, T2, T3, T4, T5, T6, TInterface> constructor)
            : base(constructor)
        {
        }

        internal void EnqueueConstructor(Func<T1, T2, T3, T4, T5, T6, TInterface> constructor)
        {
            base.EnqueueConstructor(constructor);
        }

        /// <summary>
        /// Создает объект. В случае теста выдает объект из очереди.
        /// </summary>
        public TInterface CreateInstance(T1 firstArgument, T2 secondArgument, T3 thirdArgument, 
            T4 fourthArgument, T5 fifthArgument, T6 sixthArgument)
        {
            return base.CreateInstance(firstArgument, secondArgument, thirdArgument, fourthArgument, fifthArgument, sixthArgument);
        }
    }

    /// <summary>
    /// Factory for creating implementations of TInterface. Unit tests must queue their own implementations for each created instance.
    /// </summary>
    /// <typeparam name="TInterface">Interface whose implementations should be returned.</typeparam>
    /// <typeparam name="T1">Constructor argument.</typeparam>
    /// <typeparam name="T2">Constructor argument.</typeparam>
    /// <typeparam name="T3">Constructor argument.</typeparam>
    /// <typeparam name="T4">Constructor argument.</typeparam>
    /// <typeparam name="T5">Constructor argument.</typeparam>
    /// <typeparam name="T6">Constructor argument.</typeparam>
    /// <typeparam name="T7">Constructor argument.</typeparam>
    public sealed class Factory<TInterface, T1, T2, T3, T4, T5, T6, T7> : BaseFactory<TInterface> where TInterface : class
    {
        /// <summary>
        /// Created a factory.
        /// </summary>
        /// <param name="constructor">Делегат для создания объекта.</param>
        public Factory(Func<T1, T2, T3, T4, T5, T6, T7, TInterface> constructor)
            : base(constructor)
        {
        }

        internal void EnqueueConstructor(Func<T1, T2, T3, T4, T5, T6, T7, TInterface> constructor)
        {
            base.EnqueueConstructor(constructor);
        }

        /// <summary>
        /// Создает объект. В случае теста выдает объект из очереди.
        /// </summary>
        public TInterface CreateInstance(T1 firstArgument, T2 secondArgument, T3 thirdArgument,
            T4 fourthArgument, T5 fifthArgument, T6 sixthArgument, T7 seventhArgument)
        {
            return base.CreateInstance(firstArgument, secondArgument, thirdArgument, 
                fourthArgument, fifthArgument, sixthArgument, seventhArgument);
        }
    }
    
    /// <summary>
    /// Factory for creating implementations of TInterface. Unit tests must queue their own implementations for each created instance.
    /// </summary>
    /// <typeparam name="TInterface">Interface whose implementations should be returned.</typeparam>
    /// <typeparam name="T1">Constructor argument.</typeparam>
    /// <typeparam name="T2">Constructor argument.</typeparam>
    /// <typeparam name="T3">Constructor argument.</typeparam>
    /// <typeparam name="T4">Constructor argument.</typeparam>
    /// <typeparam name="T5">Constructor argument.</typeparam>
    /// <typeparam name="T6">Constructor argument.</typeparam>
    /// <typeparam name="T7">Constructor argument.</typeparam>
    /// <typeparam name="T8">Constructor argument.</typeparam>
    public sealed class Factory<TInterface, T1, T2, T3, T4, T5, T6, T7, T8> : BaseFactory<TInterface> where TInterface : class
    {
        /// <summary>
        /// Created a factory.
        /// </summary>
        /// <param name="constructor">Делегат для создания объекта.</param>
        public Factory(Func<T1, T2, T3, T4, T5, T6, T7, T8, TInterface> constructor)
            : base(constructor)
        {
        }

        internal void EnqueueConstructor(Func<T1, T2, T3, T4, T5, T6, T7, T8, TInterface> constructor)
        {
            base.EnqueueConstructor(constructor);
        }

        /// <summary>
        /// Создает объект. В случае теста выдает объект из очереди.
        /// </summary>
        public TInterface CreateInstance(T1 firstArgument, T2 secondArgument, T3 thirdArgument,
            T4 fourthArgument, T5 fifthArgument, T6 sixthArgument, T7 seventhArgument, T8 eighthArgument)
        {
            return base.CreateInstance(firstArgument, secondArgument, thirdArgument, fourthArgument, 
                fifthArgument, sixthArgument, seventhArgument, eighthArgument);
        }
    }

    /// <summary>
    /// Factory for creating implementations of TInterface. Unit tests must queue their own implementations for each created instance.
    /// </summary>
    /// <typeparam name="TInterface">Interface whose implementations should be returned.</typeparam>
    /// <typeparam name="T1">Constructor argument.</typeparam>
    /// <typeparam name="T2">Constructor argument.</typeparam>
    /// <typeparam name="T3">Constructor argument.</typeparam>
    /// <typeparam name="T4">Constructor argument.</typeparam>
    /// <typeparam name="T5">Constructor argument.</typeparam>
    /// <typeparam name="T6">Constructor argument.</typeparam>
    /// <typeparam name="T7">Constructor argument.</typeparam>
    /// <typeparam name="T8">Constructor argument.</typeparam>
    /// <typeparam name="T9">Constructor argument.</typeparam>
    public sealed class Factory<TInterface, T1, T2, T3, T4, T5, T6, T7, T8, T9> : BaseFactory<TInterface> where TInterface : class
    {
        /// <summary>
        /// Created a factory.
        /// </summary>
        /// <param name="constructor">Делегат для создания объекта.</param>
        public Factory(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TInterface> constructor)
            : base(constructor)
        {
        }

        internal void EnqueueConstructor(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TInterface> constructor)
        {
            base.EnqueueConstructor(constructor);
        }

        /// <summary>
        /// Создает объект. В случае теста выдает объект из очереди.
        /// </summary>
        public TInterface CreateInstance(T1 firstArgument, T2 secondArgument, T3 thirdArgument, T4 fourthArgument, 
            T5 fifthArgument, T6 sixthArgument, T7 seventhArgument, T8 eighthArgument, T9 ninthArgument)
        {
            return base.CreateInstance(firstArgument, secondArgument, thirdArgument, fourthArgument,
                fifthArgument, sixthArgument, seventhArgument, eighthArgument, ninthArgument);
        }
    }
}
