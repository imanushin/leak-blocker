
using LeakBlocker.ServerShared.AdminViewCommunication.KeysAgreement;

/*
 * */

//
using System;
using LeakBlocker.Libraries.Common.IoC;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Набор реализаций Singleton-объектов.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class AdminViewCommunicationObjects
    {

        #region ILocalKeysAgreementHelper

        ///////////////////////// SINGLETON ILocalKeysAgreementHelper

        private static readonly Lazy<LocalKeysAgreementHelper> localKeysAgreementHelperLazy = new Lazy<LocalKeysAgreementHelper>(() => new LocalKeysAgreementHelper());
        private static readonly Singleton<ILocalKeysAgreementHelper> localKeysAgreementHelper = new Singleton<ILocalKeysAgreementHelper>(() => localKeysAgreementHelperLazy.Value );

        /// <summary>
        /// Реализация типа ILocalKeysAgreementHelper
        /// </summary>
        public static ILocalKeysAgreementHelper LocalKeysAgreementHelper
        {
            get
            {
                return Singletons.LocalKeysAgreementHelper.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа ILocalKeysAgreementHelper
            /// </summary>
            public static Singleton<ILocalKeysAgreementHelper> LocalKeysAgreementHelper
            {
                get
                {
                    return localKeysAgreementHelper;
                }
            }
        }

        /////////////////////////

        #endregion

    }                                                                                         
}                                                                                             
                                                                                               

