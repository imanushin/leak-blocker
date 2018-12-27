using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;

namespace LeakBlocker.Libraries.Common.Network
{
    /// <summary>
    /// Базовый класс для всех генерируемых классов 
    /// </summary>
    public abstract class BaseServer
    {
        private readonly ISecuritySessionManager sessionManager;

        /// <summary>
        /// Initializes the instance.
        /// </summary>
        /// <param name="securitySessionManager">Security manager.</param>
        protected BaseServer(ISecuritySessionManager securitySessionManager)
        {
            sessionManager = securitySessionManager;
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        protected abstract byte[] ProcessRequest(BinaryReader inputStream);

        /// <summary>
        /// Имя контракта, для поддержки работы нескольких серверов на одном ip адресе
        /// </summary>
        protected internal abstract string Name
        {
            get;
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private byte[] SafeProcessRequest(byte[] inputData)
        {
            var stream = new MemoryStream(inputData, false);

            using (var streamReader = new BinaryReader(stream))
            {
                return ProcessRequest(streamReader);
            }
        }

        internal void ProcessNetworkRequest(BinaryReader inputStream, BinaryWriter outputStream)
        {
            int tokenLength = inputStream.ReadInt32();
            byte[] token = inputStream.ReadBytes(tokenLength);

            SymmetricEncryptionProvider encryptionProvider = sessionManager.InitSession(token);

            try
            {
                byte[] outData;

                try
                {
                    int dataLength = inputStream.ReadInt32();
                    byte[] encryptedData = inputStream.ReadBytes(dataLength);

                    byte[] data = encryptionProvider.Decrypt(encryptedData);

                    outData = SafeProcessRequest(data);
                }
                catch (Exception ex)
                {
                    Log.Write(ex);

                    outData = ObjectFormatter.SerializeErrorResult(ex);
                }

                var encryptedResult = encryptionProvider.Encrypt(outData);

                outputStream.Write(encryptedResult.Length);
                outputStream.Write(encryptedResult);
            }
            finally
            {
                try
                {
                    sessionManager.CloseSession();
                }
                catch (Exception ex)
                {
                    Log.Write("Unable to close session:");
                    Log.Write(ex);
                }
            }
        }
    }
}
