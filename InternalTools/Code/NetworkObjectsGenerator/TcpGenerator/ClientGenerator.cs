using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TcpGenerator
{
    internal sealed class ClientGenerator : BaseGenerator
    {
        private const string fileTemplate =
@"{0}

// ReSharper disable UnusedVariable

namespace {1}.Generated
{{
{2}
}}

// ReSharper restore UnusedVariable

";

        private const string classTemplate =
@"    internal abstract class {0}ClientGenerated : BaseClient, {2}
    {{
        private static readonly string name = ""{0}_"" + SharedObjects.Constants.VersionString;

        protected {0}ClientGenerated(SymmetricEncryptionKey key, byte[] sharedToken) 
            : base(key, sharedToken)
        {{
        }}

        protected override string Name
        {{
            get
            {{
                return name;
            }}
        }}

{1}
    }}";

        private const string methodTemplate =
@"        [SuppressMessage(""Microsoft.Reliability"", ""CA2000:Dispose objects before losing scope"", Scope = ""member"")]
        {0} {1}.{2}({3})
        {{
{7}
            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {{
                writer.Write((byte){6});//Index of method {2}
              
                //Serializing
{4}                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {{
{5}           
                }}
            }}

        }}";


        public ClientGenerator(string projectPath, string interfacesContainerName)
            : base(projectPath, interfacesContainerName)
        {
        }

        public override string GenerateFile(IEnumerable<Type> types)
        {
            var namespaces = new HashSet<string>();

            namespaces.Add("LeakBlocker.Libraries.Common.Cryptography");
            namespaces.Add("LeakBlocker.Libraries.Common.Network");
            namespaces.Add("LeakBlocker.Libraries.Common");
            namespaces.Add("System.Diagnostics.CodeAnalysis");
            namespaces.Add("System.IO");
            namespaces.Add("System.Net");
            namespaces.Add("System.Net.Sockets");
            namespaces.Add("System");

            var classes = new StringBuilder();

            foreach (Type type in types)
            {
                string typeEntry = CreateClassEntry(type, namespaces);

                classes.AppendLine(typeEntry);
            }

            string namespacesString = string.Join(Environment.NewLine, namespaces.Select(str => string.Format("using {0};", str)));

            return string.Format(fileTemplate, namespacesString, ProjectName, classes);
        }

        private string CreateClassEntry(Type type, HashSet<string> namespaces)
        {
            string baseTypeName = type.Name.Substring(1);

            var members = new StringBuilder();

            int index = 1;

            foreach (MethodInfo method in type.GetMethods())
            {
                members.AppendLine(GenerateMethodImplementation(method, namespaces, index++));
            }

            if (type.Namespace != null)
            {
                namespaces.Add(type.Namespace);
            }

            return string.Format(classTemplate, baseTypeName, members, GetStringType(type));
        }

        private static string GenerateMethodImplementation(MethodInfo method, HashSet<string> namespaces, int index)
        {
            Type returnType = method.ReturnType;

            if (returnType.Namespace != null)
                namespaces.Add(returnType.Namespace);

            foreach (Type innerType in returnType.GenericTypeArguments)
            {
                namespaces.Add(innerType.Namespace);
            }

            string returnTypeName = GetStringType(returnType);

            string parametersString = GetStringMethodParameters(method);

            var serializing = new StringBuilder();

            var checkers = new StringBuilder();

            foreach (ParameterInfo parameter in method.GetParameters())
            {
                serializing.AppendFormat("                ObjectFormatter.SerializeParameter(writer, {0});", parameter.Name);
                serializing.AppendLine();

                namespaces.Add(parameter.ParameterType.Namespace);

                foreach (Type innerType in parameter.ParameterType.GenericTypeArguments)
                {
                    namespaces.Add(innerType.Namespace);
                }

                string checkString = CheckStringGenerator.CreateCheckString(parameter);

                if (!string.IsNullOrWhiteSpace(checkString))
                {
                    checkers.Append("            ");
                    checkers.AppendLine(checkString);
                }
            }


            string deserializing;

            if (returnType == typeof(void))
            {
                deserializing = string.Empty;
            }
            else
            {
                if (TypesFinder.IsReadOnlyType(returnType))
                {
                    deserializing = string.Format(
                        "                    return ObjectFormatter.Deserialize<{0}>(resultStream);",
                        GetStringType(returnType));

                    namespaces.Add(returnType.Namespace);
                }
                else if (returnType == typeof(bool))
                {
                    deserializing = "                    return ObjectFormatter.DeserializeBool(resultStream);";
                }
                else if (returnType == typeof(int))
                {
                    deserializing = "                    return ObjectFormatter.DeserializeInt(resultStream);";
                }
                else if (returnType == typeof(string))
                {
                    deserializing = "                    return ObjectFormatter.DeserializeString(resultStream);";
                }
                else if (returnType == typeof(byte[]))
                {
                    deserializing = "                    return ObjectFormatter.DeserializeByteArray(resultStream);";
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Type {0} is not supported", returnType));
                }

            }

            return string.Format(
                methodTemplate,
                returnTypeName,
                GetStringType(method.DeclaringType),
                method.Name,
                parametersString,
                serializing,
                deserializing,
                index,
                checkers);
        }

        public override string FileName
        {
            get
            {
                return string.Format("Generated\\ClientObjectsGenerated_{0}.cs", InterfacesContainerName.Replace(".", string.Empty));
            }
        }
    }
}
