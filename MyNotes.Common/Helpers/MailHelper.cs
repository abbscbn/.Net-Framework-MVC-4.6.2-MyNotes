using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace MyNotes.Common.Helpers
{
    public class MailHelper
    {
        private static SmtpClient CreateSmtpClient()
        {
            return new SmtpClient
            {
                Host = ConfigHelper.Get<string>("MailHost"),
                Port = ConfigHelper.Get<int>("MailPort"),
                EnableSsl = ConfigHelper.Get<bool>("MailEnableSsl"),
                Credentials = new NetworkCredential(
                    ConfigHelper.Get<string>("MailUserName"),
                    ConfigHelper.Get<string>("MailPassword")
                )
            };
        }

        // 1️⃣ Tek kişiye mail gönderme
        public static void SendMail(string body, string to, string subject, bool isHtml = true)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(ConfigHelper.Get<string>("MailUserName"));
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = isHtml;

            using (SmtpClient smtp = CreateSmtpClient())
            {
                smtp.Send(mail);
            }
        }

        // 2️⃣ Çoklu kişiye mail gönderme
        public static void SendMail(string body, List<string> toList, string subject, bool isHtml = true)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(ConfigHelper.Get<string>("MailUserName"));

            foreach (string to in toList)
            {
                mail.To.Add(to);
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = isHtml;

            using (SmtpClient smtp = CreateSmtpClient())
            {
                smtp.Send(mail);
            }
        }
    }
}
