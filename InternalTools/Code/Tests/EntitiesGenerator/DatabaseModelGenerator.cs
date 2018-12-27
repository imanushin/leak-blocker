using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CommonObjectsGenerator;

namespace EntitiesGenerator
{
    internal sealed class DatabaseModelGenerator : BaseGenerator, IFileGenerator
    {
        private const string projectName = "LeakBlocker.Libraries.Storage";
        private const string fileName = "InternalTools\\DatabaseModelGenerated.cs";

        private const string totalTemplate =
@"{0}

namespace {1}.InternalTools
{{
    internal sealed partial class DatabaseModel : DbContext, IDatabaseModel
    {{
        private bool areSetsInitialized = false;

        private void InitializeSets()
        {{
            if( areSetsInitialized )
                throw new InvalidOperationException( ""Sets had already been initialized"" );

{2}
            areSetsInitialized = true;
        }}
{3}
    }}
}}
";
        private static readonly string initializationTemplate =
@"            {0}Set = Set<Db{0}>();" + Environment.NewLine;


        private const string setMemberTemplate =
@"
        public DbSet<Db{0}> {0}Set
        {{
            get;
            private set;
        }}
";

        private const string cacheMemberTemplate =
@"
        public DatabaseCache<{0},Db{0}> {0}Cache
        {{
            get;
            private set;
        }}
";

        public DatabaseModelGenerator(Type baseObjectType)
            : base(baseObjectType)
        {
        }

        public string CreateFileEntry(ISet<Type> types)
        {
            var importedNamespaces = new HashSet<string>(types.Select(type => type.Namespace))
                {
                    "System",
                    "System.Collections.Generic",
                    "System.Data.Entity",
                    "System.Linq",
                    "LeakBlocker.Libraries.Common",
                    "LeakBlocker.Libraries.Storage.Entities"
                };

            List<Type> contextTypes = types.Where(IsParentBaseType).ToList();

            var initialization = new StringBuilder();
            var members = new StringBuilder();

            foreach (Type type in contextTypes)
            {
                initialization.AppendFormat(initializationTemplate, type.Name);
                members.AppendFormat(setMemberTemplate, type.Name);

                if (DatabaseModelInterfaceGenerator.IsCachedEntity(type))
                {
                    members.AppendFormat(cacheMemberTemplate, type.Name);
                    initialization.AppendFormat(@"            {0}Cache = new DatabaseCache<{0},Db{0}>(Get{0}Finder());", type.Name);
                    initialization.AppendLine();
                }
            }

            string namespaces = string.Join(Environment.NewLine, importedNamespaces.Select(name => string.Format("using {0};", name)));

            return string.Format(totalTemplate, namespaces, projectName, initialization, members);
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
