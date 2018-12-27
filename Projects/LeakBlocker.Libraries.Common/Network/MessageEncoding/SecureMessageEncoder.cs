using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace LeakBlocker.Libraries.Common.Network.MessageEncoding
{
    internal sealed class SecureMessageEncoder : MessageEncoder
    {
        private static readonly MessageVersion messageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap11, AddressingVersion.WSAddressing10);
        
        private readonly IMessageEncoder encoder;

        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            byte[] encodedData = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, encodedData, 0, encodedData.Length);
            bufferManager.ReturnBuffer(buffer.Array);

            using (var stream = new MemoryStream(encoder.Decode(encodedData)))
            {
                return ReadMessage(stream, int.MaxValue);
            }
        }

        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            using (var stream = new MemoryStream())
            {
                var writer = XmlWriter.Create(stream, new XmlWriterSettings());
                message.WriteMessage(writer);
                writer.Flush();

                var binaryData = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(binaryData, 0, (int)stream.Length);

                byte[] encodedData = encoder.Encode(binaryData);

                byte[] totalBytes = bufferManager.TakeBuffer(encodedData.Length + messageOffset);
                Array.Copy(encodedData, 0, totalBytes, messageOffset, encodedData.Length);

                ArraySegment<byte> byteArray = new ArraySegment<byte>(totalBytes, messageOffset, encodedData.Length);
                return byteArray;
            }
        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            XmlReader reader = XmlReader.Create(stream);
            return Message.CreateMessage(reader, maxSizeOfHeaders, this.MessageVersion);
        }

        public override void WriteMessage(Message message, Stream stream)
        {
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings()))
            {
                message.WriteMessage(writer);
            }
        }

        public override bool IsContentTypeSupported(string contentType)
        {
            return contentType.Equals(ContentType, StringComparison.OrdinalIgnoreCase);
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return messageVersion;
            }
        }

        public override string ContentType
        {
            get { return "{93BF9C26-2459-4F07-89F7-B4ED33530AB6}"; }
        }

        public override string MediaType
        {
            get { return "{4442B06C-D33B-4C59-A591-E90F1A1250AB}"; }
        }

        internal SecureMessageEncoder(IMessageEncoder encoder)
        {
            Check.ObjectIsNotNull(encoder, "encoder");

            this.encoder = encoder;
        }
    }
}
