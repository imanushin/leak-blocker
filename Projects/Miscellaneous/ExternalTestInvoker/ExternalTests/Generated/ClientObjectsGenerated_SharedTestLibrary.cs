using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System;
using SharedTestLibrary;
using LeakBlocker.Libraries.Common.Collections;

// ReSharper disable UnusedVariable

namespace ExternalTests.Generated
{
    internal abstract class TestInvokerServiceClientGenerated : BaseClient, ITestInvokerService
    {
        private static readonly string name = "TestInvokerService_" + SharedObjects.Constants.VersionString;

        protected TestInvokerServiceClientGenerated(SymmetricEncryptionKey key, byte[] sharedToken) 
            : base(key, sharedToken)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        TestsResultsData ITestInvokerService.RunTests(Byte[] fileData, ReadOnlySet<InputFile> inputFiles)
        {
            Check.ObjectIsNotNull(fileData, "fileData");
            Check.ObjectIsNotNull(inputFiles, "inputFiles");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)1);//Index of method RunTests
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, fileData);
                ObjectFormatter.SerializeParameter(writer, inputFiles);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<TestsResultsData>(resultStream);           
                }
            }

        }

    }

}

// ReSharper restore UnusedVariable

