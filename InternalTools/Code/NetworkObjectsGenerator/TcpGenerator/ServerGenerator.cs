using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TcpGenerator
{
    internal sealed class ServerGenerator : BaseGenerator
    {
        private const string fileTemplate =
@"{0}

namespace {1}.Generated
{{
{2}
}}
";

        private const string classTemplate =
@"    internal abstract class Generated{0} : BaseServer
    {{       
        private static readonly string name = ""{0}_"" + SharedObjects.Constants.VersionString;

        protected Generated{0}(ISecuritySessionManager securitySessionManager)
            : base(securitySessionManager)
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

        private static readonly string memberTemplate =
@"        protected abstract {0} {1}({2});" + Environment.NewLine;

        private const string permanentMemberTemplate =
@"
        [SuppressMessage(""Microsoft.Reliability"", ""CA2000:Dispose objects before losing scope"")]
        protected override byte[] ProcessRequest(BinaryReader inputStream)
        {{
            var outStream = new MemoryStream();
            
            using (var writer = new BinaryWriter(outStream))
            {{
                byte functionIndex = inputStream.ReadByte();
               
                switch (functionIndex)
                {{
{0}            
                    default:
                        throw new InvalidOperationException(""Unable to retrive function from index {{0}}"".Combine(functionIndex));
                }}

                var totalLength = (int)outStream.Length;
                
                byte[] resultData = outStream.GetBuffer();
                
                Array.Resize(ref resultData, totalLength);
                
                return resultData;               
            }}
            
        }}
";

        private const string memberCase =
@"                    case {0}://{4}
                        {{
                            //Read input parameters
{1}                     
                        
                            //Call function
{2}                     
                        
                            //Write output parameters
{3}                     
                        
                            break;
                        }}
";

        public ServerGenerator(string projectPath, string interfacesContainerName)
            : base(projectPath, interfacesContainerName)
        {
        }

        public override string GenerateFile(IEnumerable<Type> types)
        {
            var namespaces = new HashSet<string>();

            namespaces.Add("LeakBlocker.Libraries.Common.Network");
            namespaces.Add("LeakBlocker.Libraries.Common");
            namespaces.Add("System.Diagnostics.CodeAnalysis");
            namespaces.Add("System.IO");
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
            if (!type.IsInterface)
                throw new ArgumentException(string.Format("Type {0} should be interface", type.Name));

            string baseTypeName = type.Name.Substring(1);

            string abstractMembers = CreateAbstractMembers(type, namespaces);
            string permanentMembers = CreatePermanentMembers(type);

            return string.Format(classTemplate, baseTypeName, abstractMembers + permanentMembers);
        }

        private static string CreateAbstractMembers(Type type, HashSet<string> namespaces)
        {
            var result = new StringBuilder();

            foreach (MethodInfo function in type.GetMethods())
            {
                Type returnType = function.ReturnType;

                if (returnType.Namespace != null)
                    namespaces.Add(returnType.Namespace);

                string returnName = returnType == typeof(void) ? "void" : GetStringType(returnType);

                foreach (Type genericTypeArgument in returnType.GenericTypeArguments)
                {
                    if (genericTypeArgument != null && genericTypeArgument.Namespace != null)
                        namespaces.Add(genericTypeArgument.Namespace);
                }

                foreach (ParameterInfo parameter in function.GetParameters())
                {
                    if (parameter.ParameterType.Namespace != null)
                        namespaces.Add(parameter.ParameterType.Namespace);

                    foreach (Type genericTypeArgument in parameter.ParameterType.GenericTypeArguments)
                    {
                        if (genericTypeArgument != null && genericTypeArgument.Namespace != null)
                            namespaces.Add(genericTypeArgument.Namespace);
                    }

                }

                string parameters = GetStringMethodParameters(function);

                result.AppendFormat(memberTemplate, returnName, function.Name, parameters);
            }

            return result.ToString();
        }

        private string CreatePermanentMembers(Type type)
        {
            var result = new StringBuilder();

            int index = 1;

            foreach (MethodInfo method in type.GetMethods())
            {
                result.AppendLine(CreateMemberCase(method, index++));
            }

            return string.Format(permanentMemberTemplate, result);
        }

        private string CreateMemberCase(MethodInfo baseMethod, int index)
        {
            string functionCalling;

            var parameters = new StringBuilder();

            var checkers = new StringBuilder();

            foreach (ParameterInfo parameter in baseMethod.GetParameters())
            {
                string parameterType = GetStringType(parameter.ParameterType);

                string checkString = CheckStringGenerator.CreateCheckString(parameter);

                if (!string.IsNullOrWhiteSpace(checkString))
                {
                    checkers.Append("                            ");
                    checkers.AppendLine(checkString);
                }


                if (TypesFinder.IsReadOnlyType(parameter.ParameterType))
                {
                    parameters.AppendFormat(
                        "                            var {0} = ObjectFormatter.Deserialize<{1}>(inputStream);\n",
                        parameter.Name,
                        parameterType);

                    continue;
                }

                if (parameter.ParameterType == typeof(int))
                {
                    parameters.AppendFormat(
                        "                            var {0} = ObjectFormatter.DeserializeInt(inputStream);\n",
                        parameter.Name);

                    continue;
                }

                if (parameter.ParameterType == typeof(string))
                {
                    parameters.AppendFormat(
                        "                            var {0} = ObjectFormatter.DeserializeString(inputStream);\n",
                        parameter.Name);

                    continue;
                }

                if (parameter.ParameterType == typeof(byte[]))
                {
                    parameters.AppendFormat(
                        "                            var {0} = ObjectFormatter.DeserializeByteArray(inputStream);\n",
                        parameter.Name);

                    continue;
                }

                throw new InvalidOperationException(string.Format("Type {0} is not supported", parameterType));
            }

            parameters.AppendLine();
            parameters.Append(checkers);

            string parametersString = string.Join(", ", baseMethod.GetParameters().Select(parameter => parameter.Name));

            string resultTypeString = GetStringType(baseMethod.ReturnType);

            if (baseMethod.ReturnType == typeof(void))
                functionCalling = string.Format(
                    "                            {0}({1});",
                    baseMethod.Name,
                    parametersString);
            else
                functionCalling = string.Format(
                    "                            {2} result = {0}({1});",
                    baseMethod.Name,
                    parametersString,
                    resultTypeString);

            string writingResult;

            if (typeof(void) == baseMethod.ReturnType)
            {
                writingResult = "                            ObjectFormatter.SerializeVoidResult(writer);";
            }
            else
            {
                writingResult = "                            ObjectFormatter.SerializeResult(writer, result);";
            }

            return string.Format(memberCase, index, parameters, functionCalling, writingResult, baseMethod.Name);
        }

        public override string FileName
        {
            get
            {
                return string.Format("Generated\\ServerObjectsGenerated_{0}.cs", base.InterfacesContainerName.Replace(".", string.Empty));
            }
        }
    }
}
