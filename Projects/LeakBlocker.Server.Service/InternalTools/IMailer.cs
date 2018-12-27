using LeakBlocker.Libraries.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.InternalTools
{
    internal interface IMailer
    {
        void SendMessage(EmailSettings config, MailMessage message);
    }
}
