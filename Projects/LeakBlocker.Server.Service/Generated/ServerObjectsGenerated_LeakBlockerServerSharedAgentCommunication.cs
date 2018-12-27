using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System;
using LeakBlocker.ServerShared.AgentCommunication;

namespace LeakBlocker.Server.Service.Generated
{
    internal abstract class GeneratedAgentControlService : BaseServer
    {       
        private static readonly string name = "AgentControlService_" + SharedObjects.Constants.VersionString;

        protected GeneratedAgentControlService(ISecuritySessionManager securitySessionManager)
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

        protected abstract AgentConfiguration Synchronize(AgentState state);
        protected abstract void SendShutdownNotification();
        protected abstract void SendUninstallNotification();

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected override byte[] ProcessRequest(BinaryReader inputStream)
        {
            var outStream = new MemoryStream();
            
            using (var writer = new BinaryWriter(outStream))
            {
                byte functionIndex = inputStream.ReadByte();
               
                switch (functionIndex)
                {
                    case 1://Synchronize
                        {
                            //Read input parameters
                            var state = ObjectFormatter.Deserialize<AgentState>(inputStream);

                            Check.ObjectIsNotNull(state, "state");
                     
                        
                            //Call function
                            AgentConfiguration result = Synchronize(state);                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeResult(writer, result);                     
                        
                            break;
                        }

                    case 2://SendShutdownNotification
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            SendShutdownNotification();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeVoidResult(writer);                     
                        
                            break;
                        }

                    case 3://SendUninstallNotification
                        {
                            //Read input parameters

                     
                        
                            //Call function
                            SendUninstallNotification();                     
                        
                            //Write output parameters
                            ObjectFormatter.SerializeVoidResult(writer);                     
                        
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
