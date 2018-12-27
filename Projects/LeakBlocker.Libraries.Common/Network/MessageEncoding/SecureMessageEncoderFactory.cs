using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace LeakBlocker.Libraries.Common.Network.MessageEncoding
{
    internal sealed class SecureMessageEncoderFactory : MessageEncoderFactory
    {
        private static readonly MessageVersion messageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap11, AddressingVersion.WSAddressing10);
        
        private readonly IMessageEncoder encoder;

        public override MessageEncoder CreateSessionEncoder()
        {
            return new SecureMessageEncoder(encoder);
        }

        public override MessageEncoder Encoder
        {
            get
            {
                return new SecureMessageEncoder(encoder);
            }
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return messageVersion;
            }
        }

        internal SecureMessageEncoderFactory(IMessageEncoder encoder)
        {
            Check.ObjectIsNotNull(encoder, "encoder");

            this.encoder = encoder;
        }
    }
}
