using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using CommonObjectsGenerator;
using EntitiesGenerator.AdditionalHelpers;

namespace EntitiesGenerator
{
    internal sealed class DatabaseObjectsGenerator : BaseGenerator, IFileGenerator
    {
        private const string projectName = "LeakBlocker.Libraries.Storage";
        private const string fileName = "InternalEntites.cs";

        private const string totalTemplate =
@"{0}

namespace {1}.Entities
{{
// ReSharper disable UnusedMember.Global
// ReSharper disable AnnotationRedundanceAtValueType
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedParameter.Global
// ReSharper disable MemberCanBeInternal
// ReSharper disable RedundantCast
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable EmptyConstructor.Global
// ReSharper disable PublicConstructorInAbstractClass.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable ClassCanBeSealed.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable VirtualMemberNeverOverriden.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
{2}
// ReSharper restore ClassNeverInstantiated.Global
// ReSharper restore MemberCanBePrivate.Global
// ReSharper restore VirtualMemberNeverOverriden.Global
// ReSharper restore MemberCanBeProtected.Global
// ReSharper restore ClassCanBeSealed.Global
// ReSharper restore ClassWithVirtualMembersNeverInherited.Global
// ReSharper restore PublicConstructorInAbstractClass.Global
// ReSharper restore EmptyConstructor.Global
// ReSharper restore PartialTypeWithSinglePart
// ReSharper restore UnusedAutoPropertyAccessor.Global
// ReSharper restore RedundantCast
// ReSharper restore MemberCanBeInternal
// ReSharper restore UnusedParameter.Global
// ReSharper restore RedundantNameQualifier
// ReSharper restore AnnotationRedundanceAtValueType
// ReSharper restore UnusedMember.Global
}}
";

        private const string ClassTemplate =
@"
    /// <summary>
    /// Type for properly database working
    /// </summary>
    public{3} partial class Db{0} : {1}
    {{
{4}

{2}

{5}

{6}

{7}
    }}
";

        private const string staticInitializerTemplate =
@"        internal static Db{0} ConvertFrom{0}({0} entity, IDatabaseModel model)
        {{
{1}
        }}
";

        private const string baseUpdateTemplate =
@"         [SuppressMessage(""Microsoft.Usage"", ""CA1801:ReviewUnusedParameters"", MessageId = ""model"")]
        internal void Update{0}({0} entity, IDatabaseModel model)
        {{
            Check.ObjectIsNotNull(entity, ""entity"");

            WasUpdated = true;
{1}
        }}";

        private const string overrideUpdateTemplate =
@"        internal void Update{0}({1} baseEntity, IDatabaseModel model)
        {{            
            var entity = ({0})baseEntity;
            Update{3}(entity, model);
{2}
        }}";


        private const string memberTemplate =
@"        private {0} inner{1};

        /// <summary>
        /// Property from <see cref=""{3}""/>
        /// </summary>
{2}
        public virtual {0} {1}
        {{
            get
            {{
                return inner{1};
            }}
            set
            {{
                inner{1} = value;

                CachedItem = null;
            }}
        }}
";

        private const string protectedMemberTemplate =
@"        /// <summary>
        /// Property from <see cref=""{3}""/>
        /// </summary>
        {2}
        protected {0} {1}
        {{
            get;
            set;
        }}
";


        private const string simpleItemAssignment =
@"            {0} = entity.{0};";

        private const string entityItemAssignment =
@"            {0} = Db{1}.ConvertFrom{1}(entity.{0}, model);";

        private const string entityItemCanBuNullAssignment =
@"            {0} = entity.{0} == null ? null : Db{1}.ConvertFrom{1}(entity.{0}, model);";

        private const string entityCollectionItemAssignment =
@"            {0} = entity.{0}.Select( child=> Db{1}.ConvertFrom{1}(child, model)).ToList();";

        private const string primitiveCollectionItemAssignment =
@"            {0} = entity.{0}.ToList();";


        public DatabaseObjectsGenerator(Type baseObjectType)
            : base(baseObjectType)
        {
        }

        public string CreateFileEntry(ISet<Type> types)
        {
            var importedNamespaces = new HashSet<string>(types.Select(type => type.Namespace))
                {
                    "System.Collections.Generic",
                    "System.Linq",
                    "LeakBlocker.Libraries.Common",
                    "LeakBlocker.Libraries.Storage.Entities",
                    "LeakBlocker.Libraries.Storage.InternalTools",
                    "System.Diagnostics.CodeAnalysis",
                    "System.ComponentModel.DataAnnotations",
                    "System.ComponentModel.DataAnnotations.Schema"
                };

            var result = new StringBuilder();

            foreach (Type type in types)
            {
                string derrivedInterfaces = GetDerrivedInterfaces(type);
                string members = CreateMembers(type, importedNamespaces, types);
                string modifier = type.IsAbstract ? " abstract" : string.Empty;
                string updateMethod = GenerateUpdateMethod(type, types);
                string staticInitializer = GenerateClassConstructor(type, types);
                string typeOfMap = GenerateOfTypeMap(type, types);
                string cachedGetItems = CachedGetGenerator.GenerateCachedGetItems(type, BaseObjectType);

                result.AppendFormat(ClassTemplate, type.Name, derrivedInterfaces, members, modifier, updateMethod, staticInitializer, typeOfMap, cachedGetItems);
            }

            string namespaces = string.Join(Environment.NewLine, importedNamespaces.Select(name => string.Format("using {0};", name)));

            return string.Format(totalTemplate, namespaces, projectName, result);
        }

        private string GenerateOfTypeMap(Type type, IEnumerable<Type> allTypes)
        {
            if (!IsParentBaseType(type) || type.IsSealed)
                return string.Empty;

            var result = new StringBuilder();

            const string functionTemplate =
@"        internal static IQueryable<Db{0}> OfRuntimeType(IQueryable<Db{0}> baseQuery, {0} entity)
        {{
{1}
            throw new InvalidOperationException( ""Type {{0}} isn't supported"".Combine(entity.GetType()));
        }}";

            const string ifTemplate =
@"            if(entity is {0})
                return baseQuery.OfType<Db{0}>();";

            foreach (Type sealedType in DerrivedSealedTypes(type, allTypes))
            {
                result.AppendFormat(ifTemplate, sealedType.Name);
                result.AppendLine();
                result.AppendLine();
            }

            return string.Format(functionTemplate, type.Name, result);
        }

        private string CreateMembers(Type type, ISet<string> importedNamespaces, ICollection<Type> allTypes)
        {
            var result = new StringBuilder();

            foreach (PropertyInfo property in ObjectProperties(type))
            {
                importedNamespaces.Add(property.PropertyType.Namespace);

                string attributes = string.Join(
                    Environment.NewLine,
                    GetPropertyAttributes(property).Select(attr => string.Format("        [{0}]", attr)));

                var typeName = GetPropertyType(allTypes, property.PropertyType, importedNamespaces);

                if (property.PropertyType.IsEnum)
                    result.AppendFormat(CustomDbFields.EnumMemberTemplate, typeName, property.Name, attributes, type.Name);
                else if (property.PropertyType.Name == "Time")
                    result.AppendFormat(CustomDbFields.TimeMemberTemplate, property.Name, attributes, type.Name);
                else
                    result.AppendFormat(memberTemplate, typeName, property.Name, attributes, type.Name);
            }

            if (IsParentBaseType(type))
            {
                result.AppendFormat(
                    memberTemplate,
                    "int",
                    "Id",
                    @"        [Key,Required,SuppressMessage(""Microsoft.Security"", ""CA2119:SealMethodsThatSatisfyPrivateInterfaces"")]",
                    type.Name);

                result.AppendFormat(protectedMemberTemplate, "bool", "WasUpdated", "[NotMapped]", type.Name);
            }
            result.Append(GenerateCreateEntityMethod(type));

            return result.ToString();
        }

        private static IEnumerable<string> GetPropertyAttributes(PropertyInfo property)
        {
            List<string> result = property.GetCustomAttributes()
                .Where(attr => attr.GetType() != typeof(DataMemberAttribute))
                .Select(AttributeToString)
                .Select(name => name.Replace("Attribute", string.Empty))
                .Where(name => "DataMember" != name).ToList();

            if (IsCollection(property.PropertyType))
            {
                result.Add(@"SuppressMessage(""Microsoft.Usage"", ""CA2227:CollectionPropertiesShouldBeReadOnly"")");
                result.Add(@"SuppressMessage(""Microsoft.Design"", ""CA1002:DoNotExposeGenericLists"")");
            }

            result.Sort((str1, str2) => string.Compare(str1, str2, StringComparison.InvariantCultureIgnoreCase));

            return result;
        }

        private static string AttributeToString(Attribute attribute)
        {
            if (attribute.GetType() == typeof(StringLengthAttribute))
                return string.Format("StringLength({0})", ((StringLengthAttribute)attribute).MaximumLength);

            return attribute.GetType().Name;
        }

        private string GenerateUpdateMethod(Type type, ICollection<Type> allTypes)
        {
            var result = new StringBuilder();

            foreach (PropertyInfo property in ObjectProperties(type))
            {
                result.AppendLine(GetAssignment(property, allTypes));
            }

            if (IsParentBaseType(type))
                return string.Format(baseUpdateTemplate, type.Name, result);

            return string.Format(overrideUpdateTemplate, type.Name, GetBaseEntityType(type).Name, result, type.BaseType.Name);
        }

        private static IEnumerable<PropertyInfo> ObjectProperties(Type type)
        {
            return type.GetProperties()
                .Where(property => property.GetMethod.IsPublic)
                .Where(prop => prop.CanRead)
                .Where(prop => prop.CanWrite)
                .Where(prop => !prop.GetMethod.IsStatic);
        }

        private string GenerateClassConstructor(Type type, IEnumerable<Type> allTypes)
        {
            var result = new StringBuilder();

            bool isCached = DatabaseModelInterfaceGenerator.IsCachedEntity(GetBaseEntityType(type));

            if (isCached)
            {
                const string getFromCacheTemplate =
@"            Db{0} cachedResult = model.{1}Cache.Get<Db{0}, {0}>(entity, model.{1}Set);
            if( cachedResult != null )
            {{
                if( !cachedResult.WasUpdated )
                {{
                    cachedResult.Update{0}(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }}

                return cachedResult;
            }}";

                result.AppendFormat(getFromCacheTemplate, type.Name, GetBaseEntityType(type).Name);

                result.AppendLine();
                result.AppendLine();
            }

            if (type.IsSealed)
                result.Append(GenerateSealedClassConstructor(type, isCached));
            else
                result.Append(GenerateAbstractClassConstructor(type, allTypes));

            return string.Format(staticInitializerTemplate, type.Name, result);
        }

        private string GenerateSealedClassConstructor(Type type, bool isCached)
        {
            const string template =
@"            var result = model.{1}Set.Create<Db{0}>();
            result.Update{0}(entity,model);
            model.{1}Set.Add(result);
{2}
            return result;";

            const string addToCacheTemplate =
@"            model.{0}Cache.Add(entity, result);";

            string addToCache = isCached ? string.Format(addToCacheTemplate, GetBaseEntityType(type).Name) : string.Empty;

            return string.Format(template, type.Name, GetBaseEntityType(type).Name, addToCache);
        }

        private static string GenerateAbstractClassConstructor(Type type, IEnumerable<Type> allTypes)
        {
            string ifTemplate =
@"            var entityAs{0} = entity as {0};

            if (entityAs{0} != null)
                return Db{0}.ConvertFrom{0}(entityAs{0}, model);" + Environment.NewLine + Environment.NewLine;

            var result = new StringBuilder();

            foreach (Type otherType in DerrivedSealedTypes(type, allTypes))
            {
                result.AppendFormat(ifTemplate, otherType.Name);
            }

            result.Append(@"            throw new InvalidOperationException(""Unable to convert entity {0}"".Combine(entity));");

            return result.ToString();
        }

        private static IEnumerable<Type> DerrivedSealedTypes(Type type, IEnumerable<Type> allTypes)
        {
            return allTypes.Where(otherType => otherType.IsSubclassOf(type)).Where(otherType => !otherType.IsAbstract);
        }

        private string GenerateCreateEntityMethod(Type type)
        {
            const string abstractTemplate = @"        internal abstract {0} ForceGet{0}();";

            const string overrideTemplate =
@"        [NotNull]
        internal override {1} ForceGet{1}()
        {{
            return ForceGet{0}();
        }}";

            string result = string.Empty;

            if (type.IsAbstract)
                result = string.Format(abstractTemplate, type.Name) + Environment.NewLine;

            if (!IsParentBaseType(type))
                result += string.Format(overrideTemplate, type.Name, type.BaseType.Name) + Environment.NewLine;

            return result;
        }

        private string GetAssignment(PropertyInfo property, ICollection<Type> allTypes)
        {
            Type propertyType = property.PropertyType;

            if (allTypes.Contains(propertyType))
            {
                string template = entityItemAssignment;

                if (property.CustomAttributes.Any(attr => attr.AttributeType.Name == ("CanBeNullAttribute")))
                    template = entityItemCanBuNullAssignment;

                return string.Format(template, property.Name, propertyType.Name, GetBaseEntityType(propertyType).Name);
            }

            if (IsCollection(propertyType))
            {
                Type targetType = propertyType.GenericTypeArguments.First();

                string template = entityCollectionItemAssignment;

                if (targetType.IsPrimitive)
                    template = primitiveCollectionItemAssignment;

                return string.Format(
                    template,
                    property.Name,
                    targetType.Name);
            }

            return string.Format(simpleItemAssignment, property.Name);
        }

        private static bool IsCollection(Type propertyType)
        {
            return propertyType.Name.StartsWith("ReadOnlyCollection") || propertyType.Name.StartsWith("ReadOnlySet") || propertyType.Name.StartsWith("ReadOnlyList");
        }

        private static string GetPropertyType(ICollection<Type> allTypes, Type propertyType, ISet<string> importedNamespaces)
        {
            if (allTypes.Contains(propertyType))
                return "Db" + propertyType.Name;

            Type genericType = propertyType.GenericTypeArguments.FirstOrDefault();

            string genericSuffix = string.Empty;

            if (genericType != null)
            {
                importedNamespaces.Add(genericType.Namespace);

                string genericTypeName = genericType.Name;

                if (allTypes.Contains(genericType))
                    genericTypeName = "Db" + genericType.Name;

                genericSuffix = string.Format("<{0}>", genericTypeName);
            }

            return ConvertPropertyType(propertyType.Name) + genericSuffix;
        }

        private string GetDerrivedInterfaces(Type type)
        {
            if (IsParentBaseType(type))
                return "IKeyedItem";

            Type baseType = type.BaseType;

            return "Db" + baseType.Name;
        }

        private static string ConvertPropertyType(string originalName)
        {
            switch (originalName)
            {
                case "ReadOnlyList`1":
                case "ReadOnlySet`1":
                case "ReadOnlyCollection`1":
                    return "List";

                default:
                    return originalName;
            }
        }

        public string ProjectName
        {
            get
            {
                return projectName + "\\Entities";
            }
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
        }

    }
}
