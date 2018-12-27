﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestsGenerator
{
    internal interface ITestGenerator
    {
        string FileName
        {
            get;
        }

        string GetFileEntry(Assembly targetAssembly);
    }
}
