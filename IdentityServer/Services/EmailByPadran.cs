using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class EmailByPadran : IEmail
    {
        const string SENDER_EMAIL = "Apptest@padraholding.com";
        const string PASSWORD = "P@ssw0rd";
        const string SMTP = "mail.padraholding.com";

        public GeneralResult SendEmail(string toAddress, string Subject, string msgBody)
        {
            var result = new GeneralResult();
            MailMessage message = new MailMessage(SENDER_EMAIL, toAddress);
            message.Subject = Subject;
            message.Body = msgBody;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient()
            {
                Host = SMTP,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(SENDER_EMAIL, PASSWORD)
            };

            // Credentials are necessary if the server requires the client
            // to authenticate before it will send email on the client's behalf.


            try
            {
                Disable_CertificateValidation();
                client.Send(message);
                result.Message = "Email has been sent";
            }
            catch (Exception ex)
            {
                result.SetError($"Exception caught in CreateTestMessage2(): {ex.ToString()}");
            }
            return result;
        }
        private void Disable_CertificateValidation()
        {
            // Disabling certificate validation can expose you to a man-in-the-middle attack
            // which may allow your encrypted message to be read by an attacker
            // https://stackoverflow.com/a/14907718/740639
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                ) {
                    return true;
                };
        }
    }
}
