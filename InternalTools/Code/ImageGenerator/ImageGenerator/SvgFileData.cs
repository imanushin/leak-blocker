using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ImageGenerator
{
    internal sealed class SvgFileData
    {
        public SvgFileData(string svgFilePath)
        {
            ResourceName = Path.GetFileNameWithoutExtension(svgFilePath).Replace(" ", string.Empty).Replace("_", string.Empty);
            SvgFilePath = svgFilePath;
        }

        public string ResourceName
        {
            get;
            private set;
        }

        public string SvgFilePath
        {
            get;
            private set;
        }
    }
}
