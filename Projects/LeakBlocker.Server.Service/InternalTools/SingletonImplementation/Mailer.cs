using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation
{
    internal sealed class Mailer : IMailer
    {
        public void SendMessage(EmailSettings config, MailMessage message)
        {
            try
            {
                Dns.GetHostEntry(config.SmtpHost);//Проверяем отдельно, так как иначе будет невразумительная ошибка
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ReportStrings.UnableToResolveHost.Combine(config.SmtpHost), ex);
            }

            using (var client = new TcpClient())
            {
                client.Connect(config.SmtpHost, config.Port);//Если не будет коннекта, то вылетит исключение
            }

            using (var client = new SmtpClient(config.SmtpHost))
            {
                client.EnableSsl = config.UseSslConnection;

                if (config.IsAuthenticationEnabled)
                    client.Credentials = new NetworkCredential(config.UserName, config.Password);

                client.Send(message);
            }
        }
    }
}
