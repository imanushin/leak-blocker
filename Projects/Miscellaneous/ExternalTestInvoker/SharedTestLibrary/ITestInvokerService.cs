using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Network;

namespace SharedTestLibrary
{
    [NetworkObject]
    public interface ITestInvokerService : IDisposable
    {
        [OperationContract]
        TestsResultsData RunTests(byte[] fileData, ReadOnlySet<InputFile> inputFiles);
    }
}
