using LeakBlocker.Libraries.Common.Cryptography;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Libraries.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System;
using LeakBlocker.ServerShared.AgentCommunication;

// ReSharper disable UnusedVariable

namespace LeakBlocker.Agent.Core.Generated
{
    internal abstract class AgentControlServiceClientGenerated : BaseClient, IAgentControlService
    {
        private static readonly string name = "AgentControlService_" + SharedObjects.Constants.VersionString;

        protected AgentControlServiceClientGenerated(SymmetricEncryptionKey key, byte[] sharedToken) 
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
        AgentConfiguration IAgentControlService.Synchronize(AgentState state)
        {
            Check.ObjectIsNotNull(state, "state");

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)1);//Index of method Synchronize
              
                //Serializing
                ObjectFormatter.SerializeParameter(writer, state);
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
                    return ObjectFormatter.Deserialize<AgentConfiguration>(resultStream);           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        void IAgentControlService.SendShutdownNotification()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)2);//Index of method SendShutdownNotification
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
           
                }
            }

        }
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member")]
        void IAgentControlService.SendUninstallNotification()
        {

            var callStream = new MemoryStream();

            using(var writer = new BinaryWriter(callStream))
            {
                writer.Write((byte)3);//Index of method SendUninstallNotification
              
                //Serializing
                         
                byte[] callInfo = callStream.GetBuffer();
              
                Array.Resize(ref callInfo, (int)callStream.Length);
              
                using(BinaryReader resultStream = RequestServer(callInfo))
                {
           
                }
            }

        }

    }

}

// ReSharper restore UnusedVariable

