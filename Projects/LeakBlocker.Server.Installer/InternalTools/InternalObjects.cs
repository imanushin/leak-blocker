

using LeakBlocker.Server.Installer.InternalTools;

/*
 * */

//
using System;
using LeakBlocker.Libraries.Common.IoC;

namespace LeakBlocker.Server.Installer.InternalTools
{
    /// <summary>
    /// Набор реализаций Singleton-объектов.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class InternalObjects
    {

        #region IServiceHelper

        ///////////////////////// SINGLETON IServiceHelper

        private static readonly Lazy<ServiceHelper> serviceHelperLazy = new Lazy<ServiceHelper>(() => new ServiceHelper());
        private static readonly Singleton<IServiceHelper> serviceHelper = new Singleton<IServiceHelper>(() => serviceHelperLazy.Value );

        /// <summary>
        /// Реализация типа IServiceHelper
        /// </summary>
        internal static IServiceHelper ServiceHelper
        {
            get
            {
                return Singletons.ServiceHelper.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IServiceHelper
            /// </summary>
            internal static Singleton<IServiceHelper> ServiceHelper
            {
                get
                {
                    return serviceHelper;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IProductRegistrator

        ///////////////////////// SINGLETON IProductRegistrator

        private static readonly Lazy<ProductRegistrator> productRegistratorLazy = new Lazy<ProductRegistrator>(() => new ProductRegistrator());
        private static readonly Singleton<IProductRegistrator> productRegistrator = new Singleton<IProductRegistrator>(() => productRegistratorLazy.Value );

        /// <summary>
        /// Реализация типа IProductRegistrator
        /// </summary>
        internal static IProductRegistrator ProductRegistrator
        {
            get
            {
                return Singletons.ProductRegistrator.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IProductRegistrator
            /// </summary>
            internal static Singleton<IProductRegistrator> ProductRegistrator
            {
                get
                {
                    return productRegistrator;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IFirewallExceptionHelper

        ///////////////////////// SINGLETON IFirewallExceptionHelper

        private static readonly Lazy<FirewallExceptionHelper> firewallExceptionHelperLazy = new Lazy<FirewallExceptionHelper>(() => new FirewallExceptionHelper());
        private static readonly Singleton<IFirewallExceptionHelper> firewallExceptionHelper = new Singleton<IFirewallExceptionHelper>(() => firewallExceptionHelperLazy.Value );

        /// <summary>
        /// Реализация типа IFirewallExceptionHelper
        /// </summary>
        internal static IFirewallExceptionHelper FirewallExceptionHelper
        {
            get
            {
                return Singletons.FirewallExceptionHelper.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IFirewallExceptionHelper
            /// </summary>
            internal static Singleton<IFirewallExceptionHelper> FirewallExceptionHelper
            {
                get
                {
                    return firewallExceptionHelper;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IFileSystemConstants

        ///////////////////////// SINGLETON IFileSystemConstants

        private static readonly Lazy<FileSystemConstants> fileSystemConstantsLazy = new Lazy<FileSystemConstants>(() => new FileSystemConstants());
        private static readonly Singleton<IFileSystemConstants> fileSystemConstants = new Singleton<IFileSystemConstants>(() => fileSystemConstantsLazy.Value );

        /// <summary>
        /// Реализация типа IFileSystemConstants
        /// </summary>
        internal static IFileSystemConstants FileSystemConstants
        {
            get
            {
                return Singletons.FileSystemConstants.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IFileSystemConstants
            /// </summary>
            internal static Singleton<IFileSystemConstants> FileSystemConstants
            {
                get
                {
                    return fileSystemConstants;
                }
            }
        }

        /////////////////////////

        #endregion

    }                                                                                         
}                                                                                             
                                                                                               

