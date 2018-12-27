using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Resources;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation
{
    internal sealed class ReportMailer : IReportMailer
    {
        private readonly IScheduler scheduler;

        private static readonly Guid reportStorage = new Guid("E4B49192-676E-4F69-89B1-E3A5DBEA72A8");
        private const string lastDateKey = "lastDate";

        public ReportMailer()
        {
            scheduler = SharedObjects.CreateScheduler(SendReportMail, TimeSpan.FromMinutes(20));
        }

        public void StartObserving()
        {
            scheduler.RunNow();
        }

        public void SendTestReport(ReportConfiguration config)
        {
            Check.ObjectIsNotNull(config, "config");

            Time endTime = Time.Now;
            Time startTime = endTime.Add(-TimeSpan.FromDays(1));

            CreateReportAndSendEmail(config, startTime, endTime);
        }

        private static void CreateReportAndSendEmail(ReportConfiguration config, Time startTime, Time endTime)
        {
            ReadOnlyList<AuditItem> items = StorageObjects.AuditItemsManager.GetItems(config.Filter, startTime, endTime);

            string messageText = SharedObjects.ReportCreator.CreateReportHtml(items, startTime, endTime);

            using (var message = new MailMessage(config.Email.From, config.Email.To, ReportStrings.ReportsTitle, messageText))
            {
                InternalObjects.Mailer.SendMessage(config.Email, message);
            }
        }

        /// <summary>
        /// Задача: отправить отчет за последние сутки, данные в отчете должны быть строго до 03-00
        /// </summary>
        private static void SendReportMail()
        {
            if(InternalObjects.ConfigurationStorage.Current == null)//Пока ничего не настроили: не о чем и отчитываться
                return;

            IPrivateRegistryStorage registryStorage = SystemObjects.CreatePrivateRegistryStorage(reportStorage);

            string savedDate = registryStorage.GetValue(lastDateKey);

            DateTime endTime = DateTime.Now.Date.AddHours(3);//Отправляем всё до трёх ночи сегодняшнего числа

            if (endTime > DateTime.Now)//Рано еще: аудит не готов
                return;

            DateTime startTime;

            if (string.IsNullOrWhiteSpace(savedDate))
            {
                startTime = endTime.Subtract(TimeSpan.FromDays(1)); //24 часа назад

                Log.Add("Reports::First calling of mailer");
            }
            else
            {
                startTime = DateTime.Parse(savedDate, CultureInfo.InvariantCulture);

                Log.Add("Reports::Previous run time: {0}", startTime);
            }

            if (startTime.AddDays(1) > endTime)//Рано: сутки не прошли
                return;

            ReportConfiguration config = InternalObjects.ReportConfigurationStorage.Current;

            if (config == null)
            {
                Log.Add("Reports::Reports does not configured, returning");

                InternalObjects.AuditItemHelper.ReportsDoesNotConfigured();

                registryStorage.SetValue(lastDateKey, endTime.ToString(CultureInfo.InvariantCulture));

                return;
            }

            if (!config.AreEnabled)
            {
                Log.Add("Reports::Reports are disabled, returning");

                registryStorage.SetValue(lastDateKey, endTime.ToString(CultureInfo.InvariantCulture));

                return;
            }

            try
            {
                CreateReportAndSendEmail(config, new Time(startTime), new Time(endTime));

                Log.Add("Reports::Successfully sent reports from {0} to {1}", startTime, endTime);
            }
            catch (Exception ex)
            {
                InternalObjects.AuditItemHelper.SendingReportFailed(ex.GetExceptionMessage());

                throw;//Шедулер за нас отпишется, что тут что-то упало
            }

            registryStorage.SetValue(lastDateKey, endTime.ToString(CultureInfo.InvariantCulture));
        }

    }
}
