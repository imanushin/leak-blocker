using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests
{
    [TestClass]
    public abstract class ReadOnlyObjectTest : BaseTest
    {
        private static readonly BinaryFormatter binaryFormatter = new BinaryFormatter();
        private const int maxObjectsToTest = 16;

        protected virtual bool SkipTesting
        {
            get
            {
                return false;
            }
        }

        protected virtual void CheckArgumentExceptionParameter(string expectedParameterName, string actualParameterName)
        {
            Assert.AreEqual(expectedParameterName, actualParameterName);
        }

        protected void BaseGetHashCodeTest<T>(ObjectsCache<T> objects)
        {
            if (SkipTesting)
                return;

            foreach (T target in objects)
            {
                Assert.AreEqual(target.GetHashCode(), target.GetHashCode());
            }
        }

        protected void BaseEqualsTest<T>(ObjectsCache<T> objects)
        {
            if (SkipTesting)
                return;

            List<T> items = objects.ToList();//Чтобы был индексер

            for (int currentIndex = 0; currentIndex < items.Count; currentIndex++)
            {
                T currentItem = items[currentIndex];

                for (int otherIndex = 0; otherIndex < items.Count; otherIndex++)
                {
                    T otherItem = items[otherIndex];

                    if (currentIndex == otherIndex)
                    {
                        Assert.AreEqual(currentItem, otherItem);//Равен сам себе
                    }
                    else
                    {
                        Assert.AreNotEqual(currentItem, otherItem);//В обратную сторону тоже будет проверка, но потом
                    }
                }
            }

            if (items.Count <= 1)
                Assert.Inconclusive("Add more objects to check equals method");

            foreach (T target in objects)
            {
                Assert.IsFalse(target.Equals(null));
                Assert.IsFalse(target.Equals("some string"));
            }
        }

        protected virtual bool SkipSerializationTest()
        {
            return false;
        }

        protected void BaseSerializationTest<T>(ObjectsCache<T> objects)
            where T : BaseReadOnlyObject
        {
            if (SkipTesting || SkipSerializationTest())
                return;

            Type targetType = typeof(T);

            List<DataContractAttribute> attributes = targetType.GetCustomAttributes(typeof(DataContractAttribute), false).OfType<DataContractAttribute>().ToList();

            Assert.IsTrue(attributes.Any(), "The type should have {0} for properly serializaton", typeof(DataContractAttribute).Name);

            foreach (DataContractAttribute attribute in attributes)
            {
                Assert.IsTrue(attribute.IsReference, "Readonly type should be references in the serialization form.");
            }

            var needBinarySerializationTest = targetType.GetCustomAttributes(typeof(SerializableAttribute), false).OfType<SerializableAttribute>().Any();

            foreach (T target in objects.Take(maxObjectsToTest))
            {
                using (var stream = new MemoryStream())
                {
                    var serializer = new DataContractSerializer(typeof(T));

                    serializer.WriteObject(stream, target);
                    stream.Position = 0;
                    var fromDataContract = (T)serializer.ReadObject(stream);

                    Assert.AreEqual(target, fromDataContract);
                }

                byte[] xmlData = target.SerializeToXml();

                var baseSerialized = BaseObjectSerializer.DeserializeFromXml<T>(xmlData);

                Assert.AreEqual(target, baseSerialized);

                if (needBinarySerializationTest)
                {
                    using (var stream = new MemoryStream())
                    {
                        binaryFormatter.Serialize(stream, target);
                        stream.Position = 0;
                        var fromDataContract = (T)binaryFormatter.Deserialize(stream);

                        Assert.AreEqual(target, fromDataContract);
                    }
                }
            }
        }

        protected void BaseToStringTest<T>(ObjectsCache<T> objects)
        {
            if (SkipTesting)
                return;

            foreach (T targetObject in objects)
            {
                string result = targetObject.ToString();
                Assert.IsNotNull(result);

                bool containedTemplatedCharacters = result.Contains("{#");

                Assert.IsFalse(containedTemplatedCharacters, "String {0} contains templated characters".Combine(result));
            }
        }

        public sealed class ObjectsCache<T> : IEnumerable<T>
        {
            private readonly Lazy<ReadOnlySet<T>> objects;

            public ObjectsCache(Func<IEnumerable<T>> initializer)
            {
                objects = new Lazy<ReadOnlySet<T>>(() => initializer().ToReadOnlySet());
            }

            public ReadOnlySet<T> Objects
            {
                get
                {
                    return objects.Value;
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                return objects.Value.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return objects.Value.GetEnumerator();
            }
        }
    }
}
