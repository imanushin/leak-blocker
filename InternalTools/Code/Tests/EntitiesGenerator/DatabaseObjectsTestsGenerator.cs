using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommonObjectsGenerator;

namespace EntitiesGenerator
{
    internal sealed class DatabaseObjectsTestsGenerator : BaseGenerator, IFileGenerator
    {
        private const string propjectName = "LeakBlocker.Libraries.Storage.Tests";
        private const string className = "DatabaseObjectsTests";

        private const string fileTemplate =
@"using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeakBlocker.Libraries.Storage.Entities;
using LeakBlocker.Libraries.Storage.InternalTools;

{0}

namespace {1}.Entities
{{

    [TestClass]
    public sealed class {2} : BaseDatabaseObjectsTests
    {{
        // ReSharper disable RedundantBaseQualifier

        #region Database insert/select tests

        {3}

        #endregion Database insert/select tests

        // ReSharper restore RedundantBaseQualifier
    }}
}}

";
        private const string insertSelectTestTemplate =
@"
        [TestMethod]
        public void {0}InsertSelect()
        {{
            base.TestForInsertAndSelect(
                    model => model.{1}Set,
                    {0}Test.objects,
                    Db{0}.ConvertFrom{0},
                    dbEntity => dbEntity.Get{0}());
        }}
";

        public DatabaseObjectsTestsGenerator(Type baseObjectType)
            : base(baseObjectType)
        {
        }

        public string CreateFileEntry(ISet<Type> types)
        {
            List<string> namespaceList = types.Select(type => type.Namespace).Distinct().ToList();

            namespaceList = namespaceList.Union(namespaceList.Select(ReadonlyClassesFinder.ConvertToTestNamespace)).ToList();

            namespaceList = namespaceList.Select(item => string.Format("using {0};", item)).ToList();

            namespaceList.Sort();

            string namespaces = string.Join(Environment.NewLine, namespaceList);

            string databaseInsertSelect = GetDatabaseInsertSelectTests(types);

            return string.Format(fileTemplate, namespaces, propjectName, className, databaseInsertSelect);
        }

        private string GetDatabaseInsertSelectTests(IEnumerable<Type> types)
        {
            var result = new StringBuilder();

            foreach (Type type in types)
            {
                result.AppendFormat(insertSelectTestTemplate, type.Name, GetBaseEntityType(type).Name);
            }

            return result.ToString();
        }

        public string ProjectName
        {
            get
            {
                return propjectName + "\\Entities";
            }
        }

        public string FileName
        {
            get
            {
                return className + ".cs";
            }
        }
    }
}
