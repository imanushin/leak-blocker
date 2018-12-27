using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Network
{
    /// <summary>
    /// Сохраняет и загружает объекты из потока
    /// </summary>
    public static class ObjectFormatter
    {
        private static readonly byte[] voidResult = new byte[4];

        private const byte successResult = 1;
        private const byte failResult = 2;

        /// <summary>
        /// Загружает объект из потока
        /// </summary>
        /// <typeparam name="TBaseReadOnlyObject"></typeparam>
        public static TBaseReadOnlyObject Deserialize<TBaseReadOnlyObject>(BinaryReader stream)
            where TBaseReadOnlyObject : BaseReadOnlyObject
        {
            Check.ObjectIsNotNull(stream, "stream");

            int size = stream.ReadInt32();

            Check.IntegerIsGreaterThanZero(size);

            byte[] data = stream.ReadBytes(size);

            return BaseObjectSerializer.DeserializeFromXml<TBaseReadOnlyObject>(data);
        }

        /// <summary>
        /// Загружает int из потока
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int")]//В данном случае имеет смысл
        public static int DeserializeInt(BinaryReader stream)
        {
            Check.ObjectIsNotNull(stream, "stream");

            return stream.ReadInt32();
        }

        /// <summary>
        /// Загружает int из потока
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int")]//В данном случае имеет смысл
        public static byte[] DeserializeByteArray(BinaryReader stream)
        {
            Check.ObjectIsNotNull(stream, "stream");

            int length = stream.ReadInt32();
            return stream.ReadBytes(length);
        }

        /// <summary>
        /// Десериализует исключение
        /// </summary>
        public static Exception DeserializeException(BinaryReader stream)
        {
            Check.ObjectIsNotNull(stream, "stream");

            int size = stream.ReadInt32();

            Check.IntegerIsGreaterThanZero(size);

            byte[] data = stream.ReadBytes(size);//Читаем так, чтобы сериализатор не лез за границу сериализованных данных

            using (var memoryStream = new MemoryStream(data, false))
            {
                var formatter = new BinaryFormatter();

                return (Exception)formatter.Deserialize(memoryStream);
            }
        }

        /// <summary>
        /// Сериализует массив байт.
        /// </summary>
        public static void SerializeParameter(BinaryWriter stream, byte[] inputArray)
        {
            stream.Write(inputArray.Length);
            stream.Write(inputArray);
        }

        /// <summary>
        /// Сохраняет объект в поток
        /// </summary>
        public static void SerializeResult(BinaryWriter stream, string inputObject)
        {
            Check.ObjectIsNotNull(stream, "stream");
            Check.ObjectIsNotNull(inputObject, "inputObject");

            stream.Write(successResult);
            stream.Write(inputObject);
        }

        /// <summary>
        /// Сохраняет объект в поток
        /// </summary>
        public static void SerializeResult(BinaryWriter stream, bool inputObject)
        {
            Check.ObjectIsNotNull(stream, "stream");
            Check.ObjectIsNotNull(inputObject, "inputObject");

            stream.Write(successResult);
            stream.Write(inputObject);
        }

        /// <summary>
        /// Сохраняет объект в поток
        /// </summary>
        /// <typeparam name="TBaseReadOnlyObject"></typeparam>
        public static void SerializeResult<TBaseReadOnlyObject>(BinaryWriter stream, TBaseReadOnlyObject inputObject)
            where TBaseReadOnlyObject : BaseReadOnlyObject
        {
            Check.ObjectIsNotNull(stream, "stream");
            Check.ObjectIsNotNull(inputObject, "inputObject");

            stream.Write(successResult);

            SerializeParameter(stream, inputObject);
        }

        /// <summary>
        /// Сохраняет int в поток
        /// </summary>
        public static void SerializeResult(BinaryWriter stream, int inputObject)
        {
            Check.ObjectIsNotNull(stream, "stream");

            stream.Write(successResult);

            SerializeParameter(stream, inputObject);
        }

        /// <summary>
        /// Сохраняет int в поток
        /// </summary>
        public static void SerializeResult(BinaryWriter stream, int? inputObject)
        {
            Check.ObjectIsNotNull(stream, "stream");

            stream.Write(successResult);

            SerializeParameter(stream, inputObject);
        }

        /// <summary>
        /// Сохраняет объект в поток
        /// </summary>
        /// <typeparam name="TBaseReadOnlyObject"></typeparam>
        public static void SerializeParameter<TBaseReadOnlyObject>(BinaryWriter stream, TBaseReadOnlyObject inputObject)
            where TBaseReadOnlyObject : BaseReadOnlyObject
        {
            Check.ObjectIsNotNull(stream, "stream");
            Check.ObjectIsNotNull(inputObject, "inputObject");

            byte[] data = inputObject.SerializeToXml();

            stream.Write(data.Length);
            stream.Write(data);
        }

        /// <summary>
        /// Сохраняет int в поток
        /// </summary>
        public static void SerializeParameter(BinaryWriter stream, int inputObject)
        {
            Check.ObjectIsNotNull(stream, "stream");

            stream.Write(inputObject);
        }


        /// <summary>
        /// Сохраняет int? в поток
        /// </summary>
        public static void SerializeParameter(BinaryWriter stream, int? inputObject)
        {
            Check.ObjectIsNotNull(stream, "stream");

            stream.Write(inputObject.HasValue);

            if (inputObject.HasValue)
                stream.Write(inputObject.Value);
        }

        /// <summary>
        /// Сохраняет int в поток
        /// </summary>
        public static void SerializeParameter(BinaryWriter stream, string inputObject)
        {
            Check.ObjectIsNotNull(stream, "stream");
            Check.ObjectIsNotNull(inputObject, "inputObject");

            stream.Write(inputObject);
        }

        /// <summary>
        /// Записывает в поток результат, если на выходе только требовалась гарантия завершения работы функции.
        /// </summary>
        public static void SerializeVoidResult(BinaryWriter stream)
        {
            Check.ObjectIsNotNull(stream, "stream");

            stream.Write(successResult);
            stream.Write(voidResult);
        }

        /// <summary>
        /// Записывает в поток результат, если на выходе только требовалась гарантия завершения работы функции.
        /// </summary>
        public static byte[] SerializeErrorResult(Exception error)
        {
            Check.ObjectIsNotNull(error, "error");

            using (var stream = new MemoryStream())
            {
                stream.WriteByte(failResult);

                var formatter = new BinaryFormatter();

                using (var memoryStream = new MemoryStream())
                {
                    formatter.Serialize(memoryStream, error);

                    var length = (int)memoryStream.Length;

                    byte[] errorLength = BitConverter.GetBytes(length);

                    stream.Write(errorLength, 0, errorLength.Length);

                    stream.Write(memoryStream.GetBuffer(), 0, length);
                }

                var totalLength = (int)stream.Length;

                var result = stream.GetBuffer();

                Array.Resize(ref result, totalLength);

                return result;
            }
        }

        /// <summary>
        /// Читает поток и определяет, были ли ошибки в работе метода
        /// true, если метод завершился успешно.
        /// false, если произошла ошибка
        /// </summary>
        public static bool IsSuccessResult(BinaryReader stream)
        {
            Check.ObjectIsNotNull(stream, "stream");

            int result = stream.ReadByte();

            if (result < 0)
                throw new InvalidOperationException("Stream is finished");

            if (result == successResult)
                return true;

            if (result == failResult)
                return false;

            throw new InvalidOperationException("Unknown result: {0}".Combine(result));
        }

        /// <summary>
        /// Десериализация строки
        /// </summary>
        public static string DeserializeString(BinaryReader resultStream)
        {
            return resultStream.ReadString();
        }

        /// <summary>
        /// Десериализация bool выражения
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bool")]//В данном случае нормально
        public static bool DeserializeBool(BinaryReader resultStream)
        {
            return resultStream.ReadBoolean();
        }
    }
}
