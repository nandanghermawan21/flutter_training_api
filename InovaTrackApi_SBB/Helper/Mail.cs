using System;
using System.Net;
using System.Net.Mail;

namespace InovaTrackApi_SBB.Helper
{
    public class Mail
    {
        public static async System.Threading.Tasks.Task<bool> SendAsync(string subject,
            string body,
            string toAddress,
            string emailAccount,
            string emailHost,
            string emailPassword,
            int emailPort)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailAccount);
                    mail.Subject = subject;
                    mail.IsBodyHtml = true;
                    mail.Body = body;
                    mail.To.Add(new MailAddress(toAddress));

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = emailHost;
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential(emailAccount, emailPassword);
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = emailPort;
                        await smtp.SendMailAsync(mail);
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
