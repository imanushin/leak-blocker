using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace SharedTestLibrary
{
    [DataContract]
    public sealed class TestsResultsData : BaseReadOnlyObject
    {
        public TestsResultsData(ReadOnlyList<byte> testResultsFileEntry)
        {
            TestResultsFileEntry = testResultsFileEntry;
        }

        [DataMember]
        public ReadOnlyList<byte> TestResultsFileEntry;

        protected override IEnumerable<object> GetInnerObjects()
        {
           yield return TestResultsFileEntry;
        }
    }
}
