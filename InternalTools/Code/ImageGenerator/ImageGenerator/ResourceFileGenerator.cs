using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ab2d;

namespace ImageGenerator
{
    internal sealed class ResourceFileGenerator : BaseFileCreator
    {
        private const string viewboxHeader = @"<Viewbox xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" Stretch=""Uniform"">";

        private const string fileTemplate =
@"<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
<!-- ReSharper disable InconsistentNaming -->
{0}
<!-- ReSharper restore InconsistentNaming -->
</ResourceDictionary>";

        public ResourceFileGenerator(string resultFileName, string projectName)
            : base(resultFileName, projectName)
        {
        }

        public override string GenerateFileEntry(IEnumerable<SvgFileData> mapFromNameToFilePath)
        {
            var result = new StringBuilder();

            foreach (var data in mapFromNameToFilePath)
            {
                ReaderSvg.Instance.AutoSize = false;
                ReaderSvg.Instance.SetNameProperty = false;

                using (FileStream stream = File.OpenRead(data.SvgFilePath))
                {
                    ReaderSvg.Instance.Read(stream);
                    string xaml = ReaderSvg.Instance.GetXaml();

                    xaml = ReplaceViewBoxTag(xaml, string.Format(@"<DataTemplate x:Key=""{0}"">", data.ResourceName));
                    xaml = xaml.Replace("</Viewbox>", "</DataTemplate>");

                    result.Append(xaml);
                }
            }

            return string.Format(fileTemplate, result);
        }

        private static string ReplaceViewBoxTag(string text, string replacement)
        {
            while (true)
            {
                int startIndex = text.IndexOf("<Viewbox");
                if(startIndex == -1)
                    return text;

                int endIndex = text.IndexOf(">", startIndex);
                if(endIndex == -1)
                    return text;

                string subString = text.Substring(startIndex, endIndex - startIndex + 1);

                text = text.Replace(subString, replacement);
            }
        }
    }
}
