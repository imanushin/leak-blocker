using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Network;

namespace SharedTestLibrary
{
    public sealed class SessionManager : ISecuritySessionManager
    {
        private static readonly SessionManager sessionManager = new SessionManager();

        private readonly SymmetricEncryptionProvider provider = new SymmetricEncryptionProvider(SymmetricEncryptionKey.Empty);

        public SymmetricEncryptionProvider InitSession(byte[] token)
        {
            return provider;
        }

        public void CloseSession()
        {
        }

        public static SessionManager Instance
        {
            get
            {
                return sessionManager;
            }
        }
    }
}
