/*
<#@ template language="c#" hostspecific="true" debug="true" #>
<#@ import namespace="System" #>
<#@ import namespace="System.IO" #>
<#@ import namespace="System.Linq" #>
<#@ import namespace="System.Collections.Generic" #>
<#@ import namespace="CodeGen" #>
<#@ assembly name="$(ProjectDir)..\..\InternalTools\Binaries\CodeGen.dll" #>
<#@ assembly name="EnvDTE" #>
<#@ assembly name="System.Core" #>
 * */

//<# /*
public static class Template
{
    public static void TransformText()
    {
        // */ #><#

        const string headerTemplate =

            "                                                                                               \r\n" +
            "using System;                                                                                  \r\n" +
            "using LeakBlocker.Libraries.Common.IoC;                                                        \r\n" +
            "                                                                                               \r\n" +
            "namespace {0}                                                                                  \r\n" +
            "{{                                                                                             \r\n" +
            "    /// <summary>                                                                              \r\n" +
            "    /// Набор реализаций Singleton-объектов.                                                   \r\n" +
            "    /// </summary>                                                                             \r\n" +
            "    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]                                  \r\n" +
            "    {1} static class {2}                                                                       \r\n" +
            "    {{                                                                                         \r\n";

        const string singletonTemplate =

            "                                                                                               \r\n" +
            "        #region {0}                                                                            \r\n" +
            "                                                                                               \r\n" +
            "        ///////////////////////// SINGLETON {0}                                                \r\n" +
            "                                                                                               \r\n" +
            "        private static readonly Lazy<{4}> {5}Lazy = new Lazy<{4}>(() => new {4}());            \r\n" +
            "        private static readonly {6}<{2}> {5} = new {6}<{2}>(() => {5}Lazy.Value );             \r\n" +
            "                                                                                               \r\n" +
            "        /// <summary>                                                                          \r\n" +
            "        /// Реализация типа {0}                                                                \r\n" +
            "        /// </summary>                                                                         \r\n" +
            "        {1} static {2} {3}                                                                     \r\n" +
            "        {{                                                                                     \r\n" +
            "            get                                                                                \r\n" +
            "            {{                                                                                 \r\n" +
            "                return Singletons.{3}.Instance;                                                \r\n" +
            "            }}                                                                                 \r\n" +
            "        }}                                                                                     \r\n" +
            "                                                                                               \r\n" +
            "        /// <summary>                                                                          \r\n" +
            "        /// Singletons                                                                         \r\n" +
            "        /// </summary>                                                                         \r\n" +
            "        [System.Diagnostics.CodeAnalysis.SuppressMessage(\"Microsoft.Design\", \"CA1034\")]    \r\n" +
            "        internal static partial class Singletons                                                    \r\n" +
            "        {{                                                                                     \r\n" +
            "            /// <summary>                                                                      \r\n" +
            "            /// Реализация типа {0}                                                            \r\n" +
            "            /// </summary>                                                                     \r\n" +
            "            {1} static {6}<{2}> {3}                                                            \r\n" +
            "            {{                                                                                 \r\n" +
            "                get                                                                            \r\n" +
            "                {{                                                                             \r\n" +
            "                    return {5};                                                                \r\n" +
            "                }}                                                                             \r\n" +
            "            }}                                                                                 \r\n" +
            "        }}                                                                                     \r\n" +
            "                                                                                               \r\n" +
            "        /////////////////////////                                                              \r\n" +
            "                                                                                               \r\n" +
            "        #endregion                                                                             \r\n" +
            "                                                                                               \r\n";

        const string factoryTemplate =

            "                                                                                               \r\n" +
            "        #region {0}                                                                            \r\n" +
            "                                                                                               \r\n" +
            "        ///////////////////////// FACTORY {0}                                                  \r\n" +
            "                                                                                               \r\n" +
            "        private static readonly {6}<{2}> {5} = new {6}<{2}>(() => new {4}());                  \r\n" +
            "                                                                                               \r\n" +
            "        /// <summary>                                                                          \r\n" +
            "        /// Реализация типа {0}                                                                \r\n" +
            "        /// </summary>                                                                         \r\n" +
            "        {1} static {2} Create{3}()                                                             \r\n" +
            "        {{                                                                                     \r\n" +
            "            return Factories.{3}.CreateInstance();                                             \r\n" +
            "        }}                                                                                     \r\n" +
            "                                                                                               \r\n" +
            "        /// <summary>                                                                          \r\n" +
            "        /// Factories                                                                          \r\n" +
            "        /// </summary>                                                                         \r\n" +
            "        [System.Diagnostics.CodeAnalysis.SuppressMessage(\"Microsoft.Design\", \"CA1034\")]    \r\n" +
            "        internal static partial class Factories                                                \r\n" +
            "        {{                                                                                     \r\n" +
            "            /// <summary>                                                                      \r\n" +
            "            /// Реализация типа {0}                                                            \r\n" +
            "            /// </summary>                                                                     \r\n" +
            "            {1} static {6}<{2}> {3}                                                            \r\n" +
            "            {{                                                                                 \r\n" +
            "                get                                                                            \r\n" +
            "                {{                                                                             \r\n" +
            "                    return {5};                                                                \r\n" +
            "                }}                                                                             \r\n" +
            "            }}                                                                                 \r\n" +
            "        }}                                                                                     \r\n" +
            "                                                                                               \r\n" +
            "        /////////////////////////                                                              \r\n" +
            "                                                                                               \r\n" +
            "        #endregion                                                                             \r\n" +
            "                                                                                               \r\n";

        const string parameterizedFactoryTemplate =

            "                                                                                               \r\n" +
            "        #region {0}                                                                            \r\n" +
            "                                                                                               \r\n" +
            "        ///////////////////////// FACTORY {0}                                                  \r\n" +
            "                                                                                               \r\n" +
            "        private static readonly {6}<{2}, {9}> {5} = new {6}<{2}, {9}>(({11}) => new {4}({11}));  \r\n" +
            "                                                                                               \r\n" +
            "        /// <summary>                                                                          \r\n" +
            "        /// Реализация типа {0}                                                                \r\n" +
            "        /// </summary>                                                                         \r\n" +
            "        {1} static {2} Create{3}({10})                                              \r\n" +
            "        {{                                                                                     \r\n" +
            "            return Factories.{3}.CreateInstance({11});                                  \r\n" +
            "        }}                                                                                     \r\n" +
            "                                                                                               \r\n" +
            "        /// <summary>                                                                          \r\n" +
            "        /// Factories                                                                          \r\n" +
            "        /// </summary>                                                                         \r\n" +
            "        [System.Diagnostics.CodeAnalysis.SuppressMessage(\"Microsoft.Design\", \"CA1034\")]    \r\n" +
            "        internal static partial class Factories                                                \r\n" +
            "        {{                                                                                     \r\n" +
            "            /// <summary>                                                                      \r\n" +
            "            /// Реализация типа {0}                                                            \r\n" +
            "            /// </summary>                                                                     \r\n" +
            "            {1} static {6}<{2}, {9}> {3}                                                       \r\n" +
            "            {{                                                                                 \r\n" +
            "                get                                                                            \r\n" +
            "                {{                                                                             \r\n" +
            "                    return {5};                                                                \r\n" +
            "                }}                                                                             \r\n" +
            "            }}                                                                                 \r\n" +
            "        }}                                                                                     \r\n" +
            "                                                                                               \r\n" +
            "        /////////////////////////                                                              \r\n" +
            "                                                                                               \r\n" +
            "        #endregion                                                                             \r\n" +
            "                                                                                               \r\n";

        const string footerTemplate =

            "    }}                                                                                         \r\n" +
            "}}                                                                                             \r\n" +
            "                                                                                               \r\n";

        string solutionFile = ((EnvDTE.DTE)((IServiceProvider)Host).GetService(typeof(EnvDTE.DTE))).Solution.FullName;
        CodeGen.TemplateTools.Initialize(objects, Host.TemplateFile, solutionFile);

        var writtenString = string.Format(headerTemplate, CodeGen.TemplateTools.Namespace, CodeGen.TemplateTools.ClassAccessModifier, CodeGen.TemplateTools.ClassName);

        writtenString = string.Join(Environment.NewLine, writtenString.Split('\n').Select(str => str.TrimEnd('\n', '\r', ' ')));

        Write(writtenString);

        foreach (StaticObjectDeclaration instance in objects)
        {
            string modifier = instance.Internal ? "internal" : "public";
            string instanceType = instance.DeclarationType;
            string intefaceName = instance.Interface;
            string className = instance.Implementation;

            string commentInterfaceName = intefaceName.Replace("<", "&lt;").Replace(">", "&gt;");
            string propertyName = instance.PropertyName;
            string privateFieldName = instance.PrivateFieldName;

            if (instance is SingletonDeclaration)
            {
                string singletoneString = string.Format(singletonTemplate, commentInterfaceName, modifier, intefaceName, propertyName, className, privateFieldName, instanceType, CodeGen.TemplateTools.ClassAccessModifier, CodeGen.TemplateTools.GetWrapperAccessModifier<SingletonDeclaration>());

                singletoneString = string.Join(Environment.NewLine, singletoneString.Split('\n').Select(str => str.TrimEnd('\n', '\r', ' ')));

                Write(singletoneString);
            }
            else if (instance is FactoryDeclaration)
            {
                if ((instance as FactoryDeclaration).InitializerTypeNames == null)
                {
                    string factoryString = string.Format(factoryTemplate, commentInterfaceName, modifier, intefaceName, propertyName, className, privateFieldName, instanceType, CodeGen.TemplateTools.ClassAccessModifier, CodeGen.TemplateTools.GetWrapperAccessModifier<FactoryDeclaration>());

                    factoryString = string.Join(Environment.NewLine, factoryString.Split('\n').Select(str => str.TrimEnd('\n', '\r', ' ')));

                    Write(factoryString);
                }
                else
                {
                    string factoryString = string.Format(
                        parameterizedFactoryTemplate, 
                        commentInterfaceName, 
                        modifier, 
                        intefaceName,
                        propertyName, 
                        className,
                        privateFieldName,
                        instanceType,
                        CodeGen.TemplateTools.ClassAccessModifier,
                        CodeGen.TemplateTools.GetWrapperAccessModifier<FactoryDeclaration>(),
                        string.Join(", ", (instance as FactoryDeclaration).InitializerTypeNames.Select(description => description.TypeName)), 
                        string.Join(", ", (instance as FactoryDeclaration).InitializerTypeNames.Select(description => description.TypeName + " " + description.ParameterName + (description.DefaultValue == null ? "" : " = " + description.DefaultValue))),
                        string.Join(", ", (instance as FactoryDeclaration).InitializerTypeNames.Select(description => description.ParameterName)));

                    factoryString = string.Join(Environment.NewLine, factoryString.Split('\n').Select(str => str.TrimEnd('\n', '\r', ' ')));


                    Write(factoryString);
                }
            }
        }

        Write(string.Format(footerTemplate));

        //#><# /*
    }
}
// */ #>
