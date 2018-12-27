using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace LeakBlocker.Libraries.Common.Network.MessageEncoding
{
    internal sealed class SecureRemoteBinding : NetTcpBinding
    {
        private readonly IMessageEncoder encoder;

        internal SecureRemoteBinding(IMessageEncoder encoder)
        {
            Check.ObjectIsNotNull(encoder, "encoder");

            this.encoder = encoder;
        }

        public override BindingElementCollection CreateBindingElements()
        {
            return new BindingElementCollection(new BindingElement[]
            {
                new SecureBindingElement(encoder),
                new TcpTransportBindingElement()
            });
        }
    }
}
