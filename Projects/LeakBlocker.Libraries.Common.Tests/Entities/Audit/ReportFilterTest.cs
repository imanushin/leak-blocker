using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Audit;

namespace LeakBlocker.Libraries.Common.Tests.Entities.Audit
{
    partial class ReportFilterTest
    {
        private static IEnumerable<ReportFilter> GetInstances()
        {
                foreach (bool errors in new[] { true, false })
                {
                    foreach (bool configuration in new[] { true, false })
                    {
                        foreach (bool warnings in new[] { true, false })
                        {
                            foreach (OperationDetail block in EnumHelper<OperationDetail>.Values)
                            {
                                foreach (OperationDetail allow in EnumHelper<OperationDetail>.Values)
                                {
                                    foreach (OperationDetail temporaryAccess in EnumHelper<OperationDetail>.Values)
                                    {
                                        yield return new ReportFilter(
                                                errors,
                                                block,
                                                allow,
                                                temporaryAccess,
                                                configuration,
                                                warnings);
                                    }
                                }
                            }
                        }
                }
            }
        }
    }
}
