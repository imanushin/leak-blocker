
namespace CodeGen
{
    public sealed class SingletonDeclaration : StaticObjectDeclaration
    {
        public SingletonDeclaration(string interfaceName, string implementation, bool isInternal = false, string propertyName = null)
            : base(interfaceName, implementation, isInternal, propertyName)
        {
        }

        public override string DeclarationType
        {
            get
            {
                return "Singleton";
            }
        }
    }
}
