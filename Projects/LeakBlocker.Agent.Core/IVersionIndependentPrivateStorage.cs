
using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Agent.Core
{
    internal interface IVersionIndependentPrivateStorage
    {
        bool Empty
        {
            get;
        }

        string Version
        {
            get;
        }

        string PasswordHash
        {
            get;
        }

        string UninstallString
        {
            get;
        }

        string SecretKey
        {
            get;
        }

        int InstalledVersionsCounter
        {
            get;
        }

        void SaveData(string hash, string encryptionKey);
        void IncrementCounter();
        void DecrementCounter();
    }
}
