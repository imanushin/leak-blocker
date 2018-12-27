using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageGenerator
{
    internal abstract class BaseFileCreator
    {
        protected BaseFileCreator(string resultFileName, string projectName)
        {
            ResultFilePath = resultFileName;
            ProjectName = projectName;
        }

        public abstract string GenerateFileEntry(IEnumerable<SvgFileData> mapFromNameToFilePath);

        public string ResultFilePath
        {
            get;
            private set;
        }

        public string ProjectName
        {
            get;
            private set;
        }
    }
}
