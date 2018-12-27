using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class PrivateRegistryStorageImplementation : IPrivateRegistryStorage
    {
        readonly StringDictionary s = new StringDictionary();

        public Time InstallDate
        {
            get { return new Time(new DateTime(2000, 1, 1, 1, 1, 1, 1)); }
        }

        public string GetValue(string key)
        {
            Check.StringIsMeaningful(key, "key");

            if (s.ContainsKey(key))
                return s[key] ?? string.Empty;
            return string.Empty;
        }

        public void SetValue(string key, string value)
        {
            Check.StringIsMeaningful(key, "key");

            s[key] = value;
        }

        public void DeleteValue(string key)
        {
            Check.StringIsMeaningful(key, "key");
        }
    }
}
