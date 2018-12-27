using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace LeakBlocker.Libraries.Common
{
    /// <summary>
    /// Расширения для класса исключений
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Выдает Message от самого вложенного исключения
        /// </summary>
        public static string GetExceptionMessage(this Exception exception)
        {
            if (exception == null)
                return string.Empty;

            while (exception.InnerException != null)
                exception = exception.InnerException;

            return exception.Message.Trim();
        }

        /// <summary>
        /// True if an exception is thrown. Allows to check in finally blocks and Dispose methods if an exception was thrown.
        /// </summary>
        public static bool UnhandledExceptionContext
        {
            get
            {
                return (Marshal.GetExceptionCode() != 0);
            }
        }
    }

    /// <summary>
    /// Writes the specified message if an unhandled exception is thrown in the wrapped block
    /// </summary>
    public sealed class ExceptionNotifier : IDisposable
    {
        private readonly string message;

        /// <summary>
        /// Creates an instance.
        /// </summary>
        public ExceptionNotifier(string format, params object[] arguments)
        {
            this.message = "An exception was thrown. Operation description: " + ((format == null) ? string.Empty : format.Combine(arguments)) + ".";
        }

        /// <summary>
        /// Do not call ma
        /// </summary>
        public void Dispose()
        {
            if (ExceptionExtensions.UnhandledExceptionContext)
                Log.Write(message);
        }
    }
}
