using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Network;
using LeakBlocker.Server.Service.Generated;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.Network.AdminView
{
    internal sealed class AgentSetupPasswordToolsServer : GeneratedAgentSetupPasswordTools
    {
        public AgentSetupPasswordToolsServer()
            : base((ISecuritySessionManager)InternalObjects.AdminViewSecuritySessionManager)
        {
        }

        protected override AgentSetupPassword GetPassword()
        {
            return InternalObjects.AgentSetupPasswordManager.Current;
        }

        protected override void SendPassword(EmailSettings emailSettings)
        {
            Check.ObjectIsNotNull(emailSettings,"emailSettings");

            Check.ObjectIsNotNull(emailSettings.From);
            Check.ObjectIsNotNull(emailSettings.To);
            Check.ObjectIsNotNull(emailSettings.SmtpHost);

            string password = InternalObjects.AgentSetupPasswordManager.Current.Value;

            using (var mailMessage = new MailMessage(emailSettings.From, emailSettings.To, "Leak Blocker Agent Setup Password", password))
            {
                InternalObjects.Mailer.SendMessage(emailSettings, mailMessage);
            }
        }
    }
}
