using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Сериализует и десериализует ReadOnly объекты.
    /// </summary>
    public static class BaseObjectSerializer
    {
        /// <summary>
        /// Сериализует выбранный объект.
        /// Метод является Extension, чтобы использовать Compile-Time тип, вместо run-time типа. Таким образом, при десериализации непосредственно 
        /// десериализатор получит на вход тот же тип, что и при сериализации.
        /// </summary>
        public static byte[] SerializeToXml<TReadOnlyObject>(this TReadOnlyObject readOnlyObject)
            where TReadOnlyObject : BaseReadOnlyObject
        {
            using (var stream = new MemoryStream())
            {
                SerializeToXml(readOnlyObject, stream);

                return MemoryStreamToBytes(stream);
            }
        }

        /// <summary>
        /// Сериализует выбранный объект.
        /// Метод является Extension, чтобы использовать Compile-Time тип, вместо run-time типа. Таким образом, при десериализации непосредственно 
        /// десериализатор получит на вход тот же тип, что и при сериализации.
        /// </summary>
        public static void SerializeToXml<TReadOnlyObject>(this TReadOnlyObject readOnlyObject, Stream stream)
            where TReadOnlyObject : BaseReadOnlyObject
        {
            Check.ObjectIsNotNull(stream, "stream");

            var serializer = new DataContractSerializer(typeof(TReadOnlyObject));

            serializer.WriteObject(stream, readOnlyObject);
        }

        private static byte[] MemoryStreamToBytes(Stream stream)
        {
            var result = new byte[stream.Length];

            stream.Seek(0, SeekOrigin.Begin);

            stream.Read(result, 0, (int)stream.Length);

            return result;
        }


        /// <summary>
        /// Десериализует объект из буфера
        /// </summary>
        /// <typeparam name="TReadOnlyObject"></typeparam>
        public static TReadOnlyObject DeserializeFromXml<TReadOnlyObject>(byte[] buffer)
            where TReadOnlyObject : BaseReadOnlyObject
        {
            Check.CollectionIsNotNullOrEmpty(buffer, "buffer");

            using (var stream = new MemoryStream(buffer, false))
            {
                return DeserializeFromXml<TReadOnlyObject>(stream);
            }
        }

        /// <summary>
        /// Десериализует объект из потока
        /// </summary>
        /// <typeparam name="TReadOnlyObject"></typeparam>
        public static TReadOnlyObject DeserializeFromXml<TReadOnlyObject>(Stream stream)
            where TReadOnlyObject : BaseReadOnlyObject
        {
            Check.ObjectIsNotNull(stream, "stream");

            var serializer = new DataContractSerializer(typeof(TReadOnlyObject));

            return (TReadOnlyObject)serializer.ReadObject(stream);
        }

    }
}
