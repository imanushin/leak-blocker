using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CommonObjectsGenerator;

namespace EntitiesGenerator
{
    internal sealed class DatabaseModelInterfaceGenerator : BaseGenerator, IFileGenerator
    {
        private const string projectName = "LeakBlocker.Libraries.Storage";
        private const string fileName = "InternalTools\\IDatabaseModel.cs";

        private static readonly string[] cachedObjects = new[] { 
            "AccountSecurityIdentifier", 
            "Account",
            "AgentEncryptionData",
            "DeviceDescription",
            "OrganizationalUnit",
            "Credentials"
        };

        private const string totalTemplate =
@"{0}

namespace {1}.InternalTools
{{
    internal interface IDatabaseModel : IDisposable
    {{
{2}
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
    }}
}}
";

        private const string setMemberTemplate =
@"
        DbSet<Db{0}> {0}Set
        {{
            get;
        }}
";

        private const string cacheMemberTemplate =
@"
        DatabaseCache<{0},Db{0}> {0}Cache
        {{
            get;
        }}
";
        public DatabaseModelInterfaceGenerator(Type baseObjectType)
            : base(baseObjectType)
        {
        }

        public string CreateFileEntry(ISet<Type> types)
        {
            var importedNamespaces = new HashSet<string>(types.Select(type => type.Namespace))
                {
                    "System",
                    "System.Data.Entity",
                    "System.Data.Entity.Infrastructure",
                    "LeakBlocker.Libraries.Storage.Entities",
                };

            List<Type> contextTypes = types.Where(IsParentBaseType).ToList();

            var members = new StringBuilder();

            foreach (Type type in contextTypes)
            {
                members.AppendFormat(setMemberTemplate, type.Name);

                if (IsCachedEntity(type))
                    members.AppendFormat(cacheMemberTemplate, type.Name);
            }

            string namespaces = string.Join(Environment.NewLine, importedNamespaces.Select(name => string.Format("using {0};", name)));

            return string.Format(totalTemplate, namespaces, projectName, members);
        }

        public static bool IsCachedEntity(Type type)
        {
            return cachedObjects.Contains(type.Name);
        }

        public string ProjectName
        {
            get
            {
                return projectName;
            }
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
        }

    }
}
