using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Storage.InternalTools
{
    internal sealed class DatabaseInitializer : IDatabaseInitializer
    {
        private string databasePassword;

        public string DatabasePassword
        {
            get
            {
                Check.ObjectIsNotNull(databasePassword);

                return databasePassword;
            }
            set
            {
                Check.ObjectIsNotNull(value,"value");

                if(databasePassword != null)
                    throw new InvalidOperationException("The database was already initialized");

                databasePassword = value;

                if (databasePassword.Length > 40)
                    databasePassword = databasePassword.Substring(0, 40);
            }
        }
    }
}
