using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace LeakBlocker.Libraries.Common.Network.MessageEncoding
{
    internal sealed class SecureBindingElement : MessageEncodingBindingElement
    {
        private static readonly MessageVersion messageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap11, AddressingVersion.WSAddressing10);
        
        private readonly IMessageEncoder encoder;

        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new SecureMessageEncoderFactory(encoder);
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return messageVersion;
            }
            set
            {
            }
        }

        public override BindingElement Clone()
        {
            return new SecureBindingElement(encoder);
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            Check.ObjectIsNotNull(context, "context");

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }

        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            Check.ObjectIsNotNull(context, "context");

            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelFactory<TChannel>();
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            Check.ObjectIsNotNull(context, "context");

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelListener<TChannel>();
        }

        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            Check.ObjectIsNotNull(context, "context");

            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }

        internal SecureBindingElement(IMessageEncoder encoder)
        {
            Check.ObjectIsNotNull(encoder, "encoder");

            this.encoder = encoder;
        }
    }
}
