using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ImageGenerator
{
    internal static class StartClass
    {
        [STAThread]
        private static int Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine(@"Please specify argument: path to the project directory");

                    return 2;
                }

                var projectPath = new DirectoryInfo(args[0]);

                string projectName = projectPath.Name;

                string svgImagesPath = Path.Combine(projectPath.FullName, "SvgImages");
                string resultPath = Path.Combine(projectPath.FullName, "Generated");

                string resultXamlTemplatesFile = Path.Combine(resultPath, "SvgImageResources.xaml");
                string resultImageTypesFile = Path.Combine(resultPath, "ImageTypes.cs");

                //Имя ресурса и имя файла
                List<SvgFileData> svgFiles = Directory.EnumerateFiles(svgImagesPath, "*.svg").Select(filePath => new SvgFileData(filePath)).ToList();

                foreach (BaseFileCreator fileCreator in new BaseFileCreator[]
                    {
                        new ResourceFileGenerator(resultXamlTemplatesFile, projectName),
                        new ImageTypesGenerator(resultImageTypesFile, projectName)
                    })
                {
                    string resultFilePath = fileCreator.ResultFilePath;
                    string resultFileEntry = fileCreator.GenerateFileEntry(svgFiles);

                    if (File.Exists(resultFilePath))
                    {
                        string oldData = File.ReadAllText(resultFilePath);

                        if (oldData == resultFileEntry)
                            continue;
                    }

                    File.WriteAllText(resultFilePath, resultFileEntry);
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);

                return 1;
            }

            return 0;
        }
    }
}
