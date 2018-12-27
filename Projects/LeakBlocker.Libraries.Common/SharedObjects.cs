
using LeakBlocker.Libraries.Common.CommonInterfaces;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Implementations;
using LeakBlocker.Libraries.Common.License;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Common.SystemTools;
using LeakBlocker.Libraries.Common.Entities.Settings;

/*
 * */

//
using System;
using LeakBlocker.Libraries.Common.IoC;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Набор реализаций Singleton-объектов.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class SharedObjects
    {

        #region IEnvironment

        ///////////////////////// SINGLETON IEnvironment

        private static readonly Lazy<EnvironmentWrapper> environmentLazy = new Lazy<EnvironmentWrapper>(() => new EnvironmentWrapper());
        private static readonly Singleton<IEnvironment> environment = new Singleton<IEnvironment>(() => environmentLazy.Value );

        /// <summary>
        /// Реализация типа IEnvironment
        /// </summary>
        public static IEnvironment Environment
        {
            get
            {
                return Singletons.Environment.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IEnvironment
            /// </summary>
            public static Singleton<IEnvironment> Environment
            {
                get
                {
                    return environment;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IReportCreator

        ///////////////////////// SINGLETON IReportCreator

        private static readonly Lazy<ReportCreator> reportCreatorLazy = new Lazy<ReportCreator>(() => new ReportCreator());
        private static readonly Singleton<IReportCreator> reportCreator = new Singleton<IReportCreator>(() => reportCreatorLazy.Value );

        /// <summary>
        /// Реализация типа IReportCreator
        /// </summary>
        public static IReportCreator ReportCreator
        {
            get
            {
                return Singletons.ReportCreator.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IReportCreator
            /// </summary>
            public static Singleton<IReportCreator> ReportCreator
            {
                get
                {
                    return reportCreator;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IConstants

        ///////////////////////// SINGLETON IConstants

        private static readonly Lazy<Constants> constantsLazy = new Lazy<Constants>(() => new Constants());
        private static readonly Singleton<IConstants> constants = new Singleton<IConstants>(() => constantsLazy.Value );

        /// <summary>
        /// Реализация типа IConstants
        /// </summary>
        public static IConstants Constants
        {
            get
            {
                return Singletons.Constants.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IConstants
            /// </summary>
            public static Singleton<IConstants> Constants
            {
                get
                {
                    return constants;
                }
            }
        }

        /////////////////////////

        #endregion


        #region ICommandLine

        ///////////////////////// SINGLETON ICommandLine

        private static readonly Lazy<CommandLine> commandLineLazy = new Lazy<CommandLine>(() => new CommandLine());
        private static readonly Singleton<ICommandLine> commandLine = new Singleton<ICommandLine>(() => commandLineLazy.Value );

        /// <summary>
        /// Реализация типа ICommandLine
        /// </summary>
        public static ICommandLine CommandLine
        {
            get
            {
                return Singletons.CommandLine.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа ICommandLine
            /// </summary>
            public static Singleton<ICommandLine> CommandLine
            {
                get
                {
                    return commandLine;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IAsyncInvoker

        ///////////////////////// SINGLETON IAsyncInvoker

        private static readonly Lazy<AsyncInvoker> asyncInvokerLazy = new Lazy<AsyncInvoker>(() => new AsyncInvoker());
        private static readonly Singleton<IAsyncInvoker> asyncInvoker = new Singleton<IAsyncInvoker>(() => asyncInvokerLazy.Value );

        /// <summary>
        /// Реализация типа IAsyncInvoker
        /// </summary>
        public static IAsyncInvoker AsyncInvoker
        {
            get
            {
                return Singletons.AsyncInvoker.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IAsyncInvoker
            /// </summary>
            public static Singleton<IAsyncInvoker> AsyncInvoker
            {
                get
                {
                    return asyncInvoker;
                }
            }
        }

        /////////////////////////

        #endregion


        #region ILicenseLinkManager

        ///////////////////////// SINGLETON ILicenseLinkManager

        private static readonly Lazy<LicenseLinkManager> licenseLinkManagerLazy = new Lazy<LicenseLinkManager>(() => new LicenseLinkManager());
        private static readonly Singleton<ILicenseLinkManager> licenseLinkManager = new Singleton<ILicenseLinkManager>(() => licenseLinkManagerLazy.Value );

        /// <summary>
        /// Реализация типа ILicenseLinkManager
        /// </summary>
        public static ILicenseLinkManager LicenseLinkManager
        {
            get
            {
                return Singletons.LicenseLinkManager.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа ILicenseLinkManager
            /// </summary>
            public static Singleton<ILicenseLinkManager> LicenseLinkManager
            {
                get
                {
                    return licenseLinkManager;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IExceptionSuppressor

        ///////////////////////// SINGLETON IExceptionSuppressor

        private static readonly Lazy<ExceptionSuppressor> exceptionSuppressorLazy = new Lazy<ExceptionSuppressor>(() => new ExceptionSuppressor());
        private static readonly Singleton<IExceptionSuppressor> exceptionSuppressor = new Singleton<IExceptionSuppressor>(() => exceptionSuppressorLazy.Value );

        /// <summary>
        /// Реализация типа IExceptionSuppressor
        /// </summary>
        public static IExceptionSuppressor ExceptionSuppressor
        {
            get
            {
                return Singletons.ExceptionSuppressor.Instance;
            }
        }

        /// <summary>
        /// Singletons
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Singletons
        {
            /// <summary>
            /// Реализация типа IExceptionSuppressor
            /// </summary>
            public static Singleton<IExceptionSuppressor> ExceptionSuppressor
            {
                get
                {
                    return exceptionSuppressor;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IBaseNetworkHost

        ///////////////////////// FACTORY IBaseNetworkHost

        private static readonly Factory<IBaseNetworkHost, int, TimeSpan> baseNetworkHost = new Factory<IBaseNetworkHost, int, TimeSpan>((tcpPort, operationTimeout) => new BaseNetworkHost(tcpPort, operationTimeout));

        /// <summary>
        /// Реализация типа IBaseNetworkHost
        /// </summary>
        public static IBaseNetworkHost CreateBaseNetworkHost(int tcpPort, TimeSpan operationTimeout)
        {
            return Factories.BaseNetworkHost.CreateInstance(tcpPort, operationTimeout);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IBaseNetworkHost
            /// </summary>
            public static Factory<IBaseNetworkHost, int, TimeSpan> BaseNetworkHost
            {
                get
                {
                    return baseNetworkHost;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IScheduler

        ///////////////////////// FACTORY IScheduler

        private static readonly Factory<IScheduler, Action, TimeSpan, bool> scheduler = new Factory<IScheduler, Action, TimeSpan, bool>((action, interval, suspended) => new Scheduler(action, interval, suspended));

        /// <summary>
        /// Реализация типа IScheduler
        /// </summary>
        public static IScheduler CreateScheduler(Action action, TimeSpan interval, bool suspended = true)
        {
            return Factories.Scheduler.CreateInstance(action, interval, suspended);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IScheduler
            /// </summary>
            public static Factory<IScheduler, Action, TimeSpan, bool> Scheduler
            {
                get
                {
                    return scheduler;
                }
            }
        }

        /////////////////////////

        #endregion


        #region IThreadPool

        ///////////////////////// FACTORY IThreadPool

        private static readonly Factory<IThreadPool, int> threadPool = new Factory<IThreadPool, int>((threadCount) => new NativeThreadPool(threadCount));

        /// <summary>
        /// Реализация типа IThreadPool
        /// </summary>
        public static IThreadPool CreateThreadPool(int threadCount)
        {
            return Factories.ThreadPool.CreateInstance(threadCount);
        }

        /// <summary>
        /// Factories
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034")]
        internal static partial class Factories
        {
            /// <summary>
            /// Реализация типа IThreadPool
            /// </summary>
            public static Factory<IThreadPool, int> ThreadPool
            {
                get
                {
                    return threadPool;
                }
            }
        }

        /////////////////////////

        #endregion

    }                                                                                         
}                                                                                             
                                                                                               

