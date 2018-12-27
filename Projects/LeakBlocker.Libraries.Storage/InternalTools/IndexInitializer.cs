using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Storage.InternalTools
{
    internal sealed class IndexInitializer : IIndexInitializer
    {
        private static readonly string createIndexQueryTemplate = "CREATE INDEX SimpleIndex{0} ON {1} ({2});" + Environment.NewLine;
        private static readonly string createUniqueIndexQueryTemplate = "CREATE UNIQUE INDEX UniqueIndex{0} ON {1} ({2});" + Environment.NewLine;

        public void InitializeDatabase(DatabaseModel context)
        {
            var simpleIndexes = new Dictionary<string, IEnumerable<string>>()
                             {
                                { "DbAuditItems", new[] { "TimeTicks", "User_Id", "Computer_Id", "Device_Id" }},
                             };

            var uniqueIndexes = new Dictionary<string, IEnumerable<string>>()
                             {
                                { "DbDeviceDescriptions", new[] { "Identifier" }},
                                { "DbAccounts", new[] { "Identifier_Id" }},
                                { "DbCredentials", new[] { "Domain_Id" }},
                                { "DbOrganizationalUnits", new[] { "CanonicalName_Id" }},
                             };

            var query = new StringBuilder();

            foreach (string entityName in simpleIndexes.Keys)
            {
                foreach (string column in simpleIndexes[entityName])
                {
                    query.AppendFormat(createIndexQueryTemplate, column, entityName, column);
                }
            }

            foreach (string entityName in uniqueIndexes.Keys)
            {
                foreach (string column in uniqueIndexes[entityName])
                {
                    query.AppendFormat(createUniqueIndexQueryTemplate, column, entityName, column);
                }
            }


            string queryText = query.ToString();

            Log.Add("Audit items index creation:");
            Log.Add(queryText);

            if (context.Database.CreateIfNotExists())
            {
                context.Database.ExecuteSqlCommand(queryText);
            }
        }
    }
}
