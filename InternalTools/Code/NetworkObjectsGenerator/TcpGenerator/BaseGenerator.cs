using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace TcpGenerator
{
    internal abstract class BaseGenerator
    {
        protected BaseGenerator(string projectPath, string interfacesContainerName)
        {
            ProjectName = new DirectoryInfo(projectPath).Name;
            ProjectPath = projectPath;
            InterfacesContainerName = interfacesContainerName;
        }

        public abstract string GenerateFile(IEnumerable<Type> types);

        public string ProjectName
        {
            get;
            private set;
        }

        public string ProjectPath
        {
            get;
            private set;
        }

        protected string InterfacesContainerName
        {
            get;
            private set;
        }

        public abstract string FileName
        {
            get;
        }

        protected static string GetStringType(Type parameterType)
        {
            if (typeof(void) == parameterType)
                return "void";

            if (!parameterType.IsGenericType)
                return parameterType.Name;

            string generics = String.Join(", ", parameterType.GenericTypeArguments.Select(GetStringType));

            return String.Format("{0}<{1}>", parameterType.Name.Substring(0, parameterType.Name.IndexOf('`')), generics);
        }

        protected static string GetStringMethodParameters(MethodInfo function)
        {
            return string.Join(", ", function.GetParameters().Select(parameter => GetStringType(parameter.ParameterType) + ' ' + parameter.Name));
        }
    }
}