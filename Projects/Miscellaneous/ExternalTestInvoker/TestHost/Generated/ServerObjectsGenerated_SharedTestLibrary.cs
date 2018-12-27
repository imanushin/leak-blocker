using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System;
using SharedTestLibrary;
using LeakBlocker.Libraries.Common.Collections;

namespace TestHost.Generated
{
    internal abstract class GeneratedTestInvokerService : BaseServer
    {       
        private static readonly string name = "TestInvokerService_" + SharedObjects.Constants.VersionString;

        protected GeneratedTestInvokerService(ISecuritySessionManager securitySessionManager)
            : base(securitySessionManager)
        {
        }

        protected override string Name
        {
            get
            {
                return name;
            }
        }

        protected abstract TestsResultsData RunTests(Byte[] fileData, ReadOnlySet<InputFile> inputFiles);

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override byte[] ProcessRequest(BinaryReader inputStream)
        {
            var outStream = new MemoryStream();
            
            using (var writer = new BinaryWriter(outStream))
            {
                byte functionIndex = inputStream.ReadByte();
               
                switch (functionIndex)
                {
                    case 1://RunTests
                        {
                            //Read input parameters
                            var fileData = ObjectFormatter.DeserializeByteArray(inputStream);
                            var inputFiles = ObjectFormatter.Deserialize<ReadOnlySet<InputFile>>(inputStream);

                            Check.ObjectIsNotNull(fileData, "fileData");
                            Check.ObjectIsNotNull(inputFiles, "inputFiles");
                     
                        
                            //Call function
                            TestsResultsData result = RunTests(fileData, inputFiles);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

            
                    default:
                        throw new InvalidOperationException("Unable to retrive function from index {0}".Combine(functionIndex));
                }

                var totalLength = (int)outStream.Length;
                
                byte[] resultData = outStream.GetBuffer();
                
                Array.Resize(ref resultData, totalLength);
                
                return resultData;               
            }
            
        }

    }

}
