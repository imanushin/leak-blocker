using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageGenerator
{
    internal sealed class ImageTypesGenerator : BaseFileCreator
    {
        private const string fileTemplate =
@"using System;
using System.Windows;
using System.Windows.Controls;
using LeakBlocker.AdminView.Desktop.Images;

namespace {0}.Generated
{{
{1}
}}";

        private const string classTemplate =
@"
    internal sealed class {0} : BaseGeneratedImage
    {{
        public static DataTemplate ImageTemplate
        {{
            get
            {{
                return (DataTemplate)Application.Current.Resources[""{0}""];
            }}
        }}

        public {0}()
            :base(ImageTemplate)
        {{
        }}
    }}";

        public ImageTypesGenerator(string resultFileName, string projectName)
            : base(resultFileName, projectName)
        {
        }

        public override string GenerateFileEntry(IEnumerable<SvgFileData> mapFromNameToFilePath)
        {
            var result = new StringBuilder();

            foreach (SvgFileData data in mapFromNameToFilePath)
            {
                result.AppendFormat(classTemplate, data.ResourceName);
                result.AppendLine();
            }

            return string.Format(fileTemplate, ProjectName, result);
        }
    }
}
