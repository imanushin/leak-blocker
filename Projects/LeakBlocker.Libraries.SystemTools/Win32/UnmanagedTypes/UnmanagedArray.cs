using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes
{
    internal sealed class UnmanagedArray<T> : UnmanagedMemory, IList<T>, IList where T : struct
    {
        internal UnmanagedArray(int length)
            : base(GetArraySize(length))
        {
            SLength = Math.Max(0, length);
        }

        internal UnmanagedArray(IntPtr memory, int length)
            : base(memory, GetArraySize(length))
        {
            SLength = Math.Max(0, length);
        }

        internal UnmanagedArray(uint length)
            : base(GetArraySize(unchecked((int)length)))
        {
            SLength = Math.Max(0, unchecked((int)length));
        }

        internal UnmanagedArray(IntPtr memory, uint length)
            : base(memory, GetArraySize(unchecked((int)length)))
        {
            SLength = Math.Max(0, unchecked((int)length));
        }

        internal UnmanagedArray(T[] array)
            : base(GetArraySize(array))
        {
            if (array != null)
                SLength = array.Length;
            Value = array;
        }

        private static int GetArraySize(int length)
        {
            int size = Marshal.SizeOf(typeof(T));
            int result = unchecked(size * length);
            return result;
        }

        private static int GetArraySize(ICollection<T> array)
        {
            int size = Marshal.SizeOf(typeof(T));
            int result = 0;
            if (array != null)
            {
                unchecked
                {
                    result = size * array.Count;
                }
            }
            return result;
        }

        internal int SLength
        {
            get;
            private set;
        }

        internal uint ULength
        {
            get
            {
                return unchecked((uint)SLength);
            }
        }

        internal T[] Value
        {
            get
            {
                lock (Synchronization)
                {
                    var result = new T[SLength];
                    int size = Marshal.SizeOf(typeof(T));
                    if ((SLength > 0) && (Address != IntPtr.Zero))
                    {
                        for (int i = 0; i < SLength; i++)
                            result[i] = (T)Marshal.PtrToStructure(new IntPtr(unchecked(Address.ToInt64() + i * size)), typeof(T));
                    }
                    return result;
                }
            }
            set
            {
                lock (Synchronization)
                {
                    ClearUnsafe();
                    if ((SLength <= 0) || (value == null) || (Address == IntPtr.Zero)) 
                        return;

                    int size = Marshal.SizeOf(typeof(T));
                    for (int i = 0; i < Math.Min(SLength, value.Length); i++)
                    {
                        if (Marshal.SizeOf(value[i]) != size)
                            continue;
                        Marshal.StructureToPtr(value[i], new IntPtr(unchecked(Address.ToInt64() + i * size)), false);
                    }
                }
            }
        }

        public T this[int index]
        {
            get
            {
                return this[unchecked((uint)index)];
            }
            set
            {
                this[unchecked((uint)index)] = value;
            }
        }

        internal T this[uint index]
        {
            get
            {
                T result = default(T);
                int size = Marshal.SizeOf(typeof(T));
                if (Address != IntPtr.Zero)
                    result = (T)Marshal.PtrToStructure(new IntPtr(unchecked(Address.ToInt64() + index * size)), typeof(T));
                return result;
            }
            set
            {
                if (Address == IntPtr.Zero)
                    return;

                int size = Marshal.SizeOf(typeof(T));
                if (Marshal.SizeOf(value) == size)
                    Marshal.StructureToPtr(value, new IntPtr(unchecked(Address.ToInt64() + index * size)), false);
            }
        }

        internal IntPtr GetElementAddress(int index)
        {
            return GetElementAddress(unchecked((uint)index));
        }

        internal IntPtr GetElementAddress(uint index)
        {
            int size = Marshal.SizeOf(typeof(T));
            return new IntPtr(unchecked(Address.ToInt64() + index * size));
        }

        public static implicit operator T[](UnmanagedArray<T> value)
        {
            return (value != null) ? value.Value : new T[0];
        }

        #region IList<T>

        public int IndexOf(T item)
        {
            T[] array = Value;
            for (int i = 0; i < array.Length; i++)
            {
                if (Equals(array[i], item))
                    return i;
            }
            return -1;
        }

        void IList<T>.Insert(int index, T item)
        {
            Exceptions.Throw(ErrorMessage.NotSupported);
        }

        void IList<T>.RemoveAt(int index)
        {
            Exceptions.Throw(ErrorMessage.NotSupported);
        }

        #endregion

        #region ICollection<T>

        public int Count
        {
            get
            {
                return SLength;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        void ICollection<T>.Add(T item)
        {
            Exceptions.Throw(ErrorMessage.NotSupported);
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        bool ICollection<T>.Remove(T item)
        {
            Exceptions.Throw(ErrorMessage.NotSupported);
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Value.CopyTo(array, arrayIndex);
        }

        #endregion

        #region IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            return ((IList<T>)Value).GetEnumerator();
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        #endregion

        #region IList

        bool IList.IsFixedSize
        {
            get
            {
                return true;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (T)value;
            }
        }

        int IList.Add(object value)
        {
            Exceptions.Throw(ErrorMessage.NotSupported);
            return 0;
        }

        bool IList.Contains(object value)
        {
            return Contains((T)value);
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((T)value);
        }

        void IList.Insert(int index, object value)
        {
            Exceptions.Throw(ErrorMessage.NotSupported);
        }

        void IList.Remove(object value)
        {
            Exceptions.Throw(ErrorMessage.NotSupported);
        }

        void IList.RemoveAt(int index)
        {
            Exceptions.Throw(ErrorMessage.NotSupported);
        }

        #endregion

        #region ICollection

        bool ICollection.IsSynchronized
        {
            get
            {
                return true;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return Synchronization;
            }
        }

        public void CopyTo(Array array, int index)
        {
            Value.CopyTo(array, index);
        }

        #endregion
    }
}
