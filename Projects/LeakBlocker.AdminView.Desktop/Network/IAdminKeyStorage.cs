using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Cryptography;

namespace LeakBlocker.AdminView.Desktop.Network
{
    internal interface IAdminKeyStorage
    {
        byte[] CreateUserToken();

        SymmetricEncryptionKey EncryptionKey
        {
            get;
        }
    }
}
