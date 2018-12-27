using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.IoC
{
    internal static class TestContextData
    {
        private static readonly bool isTestContext = GetIsTestContext();

        public static bool IsTestContext
        {
            get
            {
                return isTestContext;
            }
        }

        public static void UpdateTestId()
        {
            if (!IsTestContext)
                Exceptions.Throw(ErrorMessage.InvalidOperation, "This method should not be used outside of test environment");

            CurrentCommonTestId++;
        }

        public static int CurrentCommonTestId
        {
            get;
            private set;
        }

        private static bool GetIsTestContext()
        {
            string exeName = Process.GetCurrentProcess().MainModule.ModuleName;

            return exeName.StartsWith("vstest.", StringComparison.OrdinalIgnoreCase) ||
                exeName.StartsWith("mstest", StringComparison.OrdinalIgnoreCase) ||
                exeName.StartsWith("QTAgent", StringComparison.OrdinalIgnoreCase) ||
                exeName.StartsWith("JetBrains.ReSharper.TaskRunner.", StringComparison.OrdinalIgnoreCase);
        }
    }
}
