using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;

namespace LeakBlocker.Libraries.Common.Network
{
    /// <summary>
    /// Базовый класс для клиента.
    /// Потокобезопасен
    /// </summary>
    public abstract class BaseClient : Disposable
    {
        private readonly SymmetricEncryptionProvider encryptionProvider;
        private readonly byte[] token;

        /// <summary>
        /// Инициализация.
        /// Shared token - массив, который затем передается серверу
        /// </summary>
        protected BaseClient(SymmetricEncryptionKey key, byte[] sharedToken)
        {
            Check.CollectionIsNotNullOrEmpty(sharedToken, "sharedToken");
            Check.ObjectIsNotNull(key, "key");

            token = new byte[sharedToken.Length + 4];

            byte[] length = BitConverter.GetBytes(sharedToken.Length);

            length.CopyTo(token, 0);
            sharedToken.CopyTo(token, 4);

            encryptionProvider = new SymmetricEncryptionProvider(key);
        }

        /// <summary>
        /// Запрос сервера.
        /// Возврат результата будет только после того, как сервер полностью обработал запрос (и создал ответ)
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]//Тестируется
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]//Высвобождает генерируемый клиент
        protected BinaryReader RequestServer(byte[] methodInfo)
        {
            using (var client = new TcpClient())
            {
                client.Connect(HostName, Port);

                NetworkStream stream = client.GetStream();

                using (var writer = new BinaryWriter(stream))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        writer.Write(Name);

                        writer.Write(token);

                        var encryptedCallData = encryptionProvider.Encrypt(methodInfo);

                        writer.Write(encryptedCallData.Length);//Пишем размер зашифрованного массива
                        writer.Write(encryptedCallData);//По завершению записи запустится метод

                        int resultLength = reader.ReadInt32();
                        byte[] encryptedResult = reader.ReadBytes(resultLength);
                        
                        var resultData = encryptionProvider.Decrypt(encryptedResult);
                        
                        var memoryStream = new MemoryStream(resultData, false);
                        var decryptedReader = new BinaryReader(memoryStream);

                        //Deserializing
                        bool wasSuccess = ObjectFormatter.IsSuccessResult(decryptedReader);
                        if (wasSuccess)
                            return decryptedReader;

                        Exception error = ObjectFormatter.DeserializeException(decryptedReader);

                        throw new InvalidOperationException("Method calling failed. See inner exception for details", error);
                    }
                }
            }

        }

        /// <summary>
        /// Имя контракта
        /// </summary>
        protected abstract string Name
        {
            get;
        }

        /// <summary>
        /// Адрес для соединения
        /// </summary>
        protected abstract string HostName
        {
            get;
        }

        /// <summary>
        /// Адрес для соединения
        /// </summary>
        protected abstract int Port
        {
            get;
        }

        /// <summary>
        /// Высвобождение ресурсов
        /// </summary>
        protected override void DisposeManaged()
        {
            encryptionProvider.Dispose();

            base.DisposeManaged();
        }
    }
}
