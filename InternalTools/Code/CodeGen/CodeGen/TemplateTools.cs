using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen
{
    public class TemplateTools
    {
        public static string ClassAccessModifier
        {
            get;
            private set;
        }

        public static string ClassName
        {
            get;
            private set;
        }

        public static string Namespace
        {
            get;
            private set;
        }

        private static IEnumerable<StaticObjectDeclaration> objectsCollection;

        public static void Initialize(IEnumerable<StaticObjectDeclaration> objects, string templateFile, string solutionFile)
        {
            string properlyPathToSolutionFolder = new DirectoryInfo(Path.GetDirectoryName(solutionFile)).FullName;//Убираем относительные пути
            string properlyPathToTemplateFile = new FileInfo(templateFile).FullName;


            objectsCollection = objects;

            ClassAccessModifier = "internal";
            foreach (StaticObjectDeclaration instance in objects)
            {
                if (!instance.Internal)
                {
                    ClassAccessModifier = "public";
                    break;
                }
            }

            ClassName = Path.GetFileNameWithoutExtension(properlyPathToTemplateFile);

            if (!properlyPathToSolutionFolder.EndsWith("\\"))
                properlyPathToSolutionFolder += "\\";

            Namespace = Path.GetDirectoryName(properlyPathToTemplateFile).Substring(properlyPathToSolutionFolder.Length).Replace("\\", ".");
        }

        public static string GetWrapperAccessModifier<T>() where T : StaticObjectDeclaration
        {
            return objectsCollection.OfType<T>().Any(item => !item.Internal) ? "public" : "internal";
        }
    }
}
