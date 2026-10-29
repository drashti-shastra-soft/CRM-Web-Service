using CRMWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebService.Service
{
    public class EmailSenderService : IEmailSenderService
    {
        public void SendEmail(string htmlBody, string toEmail)
        {
            try
            {
                var smtpClient = new SmtpClient("mail.mitsolutions.in")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("mail@mitsolutions.in", "mAiL#M@20!?"),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("mail@mitsolutions.in", "CRM Support"),
                    Body = htmlBody,
                    IsBodyHtml = true
                };

                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                mailMessage.To.Add(toEmail);

                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {

            }
        }
    }
}
