
using LeakBlocker.AdminView.Desktop.Common.Collections;

/*
 * */

//
using System;
using LeakBlocker.Libraries.Common.IoC;

namespace LeakBlocker.AdminView.Desktop.Common.Collections
{
    /// <summary>
    /// Набор реализаций Singleton-объектов.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static class CollectionContainer
    {

        #region ComputersCollection

        ///////////////////////// SINGLETON ComputersCollection

        private static readonly Lazy<ComputersCollection> availableComputersLazy = new Lazy<ComputersCollection>(() => new ComputersCollection());
        private static readonly Singleton<ComputersCollection> availableComputers = new Singleton<ComputersCollection>(() => availableComputersLazy.Value );

        /// <summary>
        /// Реализация типа ComputersCollection
        /// </summary>
        internal static ComputersCollection AvailableComputers
        {
            get
            {
                return Singletons.AvailableComputers.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа ComputersCollection
            /// </summary>
            internal static Singleton<ComputersCollection> AvailableComputers
            {
                get
                {
                    return availableComputers;
                }
            }
        }

        /////////////////////////

        #endregion


        #region UsersCollection

        ///////////////////////// SINGLETON UsersCollection

        private static readonly Lazy<UsersCollection> availableUsersLazy = new Lazy<UsersCollection>(() => new UsersCollection());
        private static readonly Singleton<UsersCollection> availableUsers = new Singleton<UsersCollection>(() => availableUsersLazy.Value );

        /// <summary>
        /// Реализация типа UsersCollection
        /// </summary>
        internal static UsersCollection AvailableUsers
        {
            get
            {
                return Singletons.AvailableUsers.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа UsersCollection
            /// </summary>
            internal static Singleton<UsersCollection> AvailableUsers
            {
                get
                {
                    return availableUsers;
                }
            }
        }

        /////////////////////////

        #endregion


        #region AvailableUserScopesCollection

        ///////////////////////// SINGLETON AvailableUserScopesCollection

        private static readonly Lazy<AvailableUserScopesCollection> availableUserScopesLazy = new Lazy<AvailableUserScopesCollection>(() => new AvailableUserScopesCollection());
        private static readonly Singleton<AvailableUserScopesCollection> availableUserScopes = new Singleton<AvailableUserScopesCollection>(() => availableUserScopesLazy.Value );

        /// <summary>
        /// Реализация типа AvailableUserScopesCollection
        /// </summary>
        internal static AvailableUserScopesCollection AvailableUserScopes
        {
            get
            {
                return Singletons.AvailableUserScopes.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа AvailableUserScopesCollection
            /// </summary>
            internal static Singleton<AvailableUserScopesCollection> AvailableUserScopes
            {
                get
                {
                    return availableUserScopes;
                }
            }
        }

        /////////////////////////

        #endregion


        #region AvailableComputerScopesCollection

        ///////////////////////// SINGLETON AvailableComputerScopesCollection

        private static readonly Lazy<AvailableComputerScopesCollection> availableComputerScopesLazy = new Lazy<AvailableComputerScopesCollection>(() => new AvailableComputerScopesCollection());
        private static readonly Singleton<AvailableComputerScopesCollection> availableComputerScopes = new Singleton<AvailableComputerScopesCollection>(() => availableComputerScopesLazy.Value );

        /// <summary>
        /// Реализация типа AvailableComputerScopesCollection
        /// </summary>
        internal static AvailableComputerScopesCollection AvailableComputerScopes
        {
            get
            {
                return Singletons.AvailableComputerScopes.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа AvailableComputerScopesCollection
            /// </summary>
            internal static Singleton<AvailableComputerScopesCollection> AvailableComputerScopes
            {
                get
                {
                    return availableComputerScopes;
                }
            }
        }

        /////////////////////////

        #endregion


        #region ServerDevices

        ///////////////////////// SINGLETON ServerDevices

        private static readonly Lazy<ServerDevices> serverDevicesLazy = new Lazy<ServerDevices>(() => new ServerDevices());
        private static readonly Singleton<ServerDevices> serverDevices = new Singleton<ServerDevices>(() => serverDevicesLazy.Value );

        /// <summary>
        /// Реализация типа ServerDevices
        /// </summary>
        internal static ServerDevices ServerDevices
        {
            get
            {
                return Singletons.ServerDevices.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа ServerDevices
            /// </summary>
            internal static Singleton<ServerDevices> ServerDevices
            {
                get
                {
                    return serverDevices;
                }
            }
        }

        /////////////////////////

        #endregion


        #region AuditDevicesCollection

        ///////////////////////// SINGLETON AuditDevicesCollection

        private static readonly Lazy<AuditDevicesCollection> auditDevicesCollectionLazy = new Lazy<AuditDevicesCollection>(() => new AuditDevicesCollection());
        private static readonly Singleton<AuditDevicesCollection> auditDevicesCollection = new Singleton<AuditDevicesCollection>(() => auditDevicesCollectionLazy.Value );

        /// <summary>
        /// Реализация типа AuditDevicesCollection
        /// </summary>
        internal static AuditDevicesCollection AuditDevicesCollection
        {
            get
            {
                return Singletons.AuditDevicesCollection.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа AuditDevicesCollection
            /// </summary>
            internal static Singleton<AuditDevicesCollection> AuditDevicesCollection
            {
                get
                {
                    return auditDevicesCollection;
                }
            }
        }

        /////////////////////////

        #endregion

    }                                                                                         
}                                                                                             
                                                                                               

