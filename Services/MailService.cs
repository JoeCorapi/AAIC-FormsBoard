using FormsBoard.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace FormsBoard.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailConfig;
        public MailService(MailSettings mailConfig)
        {
            _mailConfig = mailConfig;
        }

        public void SendEmail(string Subject, string HTMLBody, IEnumerable<string> EmailList)
        {
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(_mailConfig.FromEmail);
                    foreach (string email in EmailList)
                    {
                        message.To.Add(new MailAddress(email));
                    }
                    message.Subject = Subject;
                    message.IsBodyHtml = true;
                    message.Body = HTMLBody;

                    using (SmtpClient smtp = new SmtpClient(_mailConfig.Host, _mailConfig.Port))
                    {
                        smtp.Credentials = new NetworkCredential(_mailConfig.Username, _mailConfig.Password);
                        //smtp.EnableSsl = false;

                        // If Debug mode enablse SSL for GMail testing
                        smtp.EnableSsl = true;

                        smtp.UseDefaultCredentials = false;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Timeout = 30000;
                        smtp.Send(message);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void SendEmailAttachment(string Subject, string HTMLBody, byte[] data, string fileName, string mediaType, IEnumerable<string> EmailList)
        {
            try
            {
                using (var stream = new MemoryStream(data))
                        using (MailMessage message = new MailMessage())
                        {
                            message.From = new MailAddress(_mailConfig.FromEmail);
                            foreach (string email in EmailList)
                            {
                                message.To.Add(new MailAddress(email));
                            }
                            message.Subject = Subject;
                            message.IsBodyHtml = true;
                            message.Body = HTMLBody;

                            stream.Position = 0;
                            message.Attachments.Add(new Attachment(stream, fileName, mediaType));

                            using (SmtpClient smtp = new SmtpClient(_mailConfig.Host, _mailConfig.Port))
                            {
                                smtp.Credentials = new NetworkCredential(_mailConfig.Username, _mailConfig.Password);
                                smtp.EnableSsl = false;
        #if DEBUG
                                // If Debug mode enablse SSL for GMail testing
                                smtp.EnableSsl = true;
        #endif
                                smtp.UseDefaultCredentials = false;
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smtp.Timeout = 30000;
                                smtp.Send(message);
                            }
                        }
                }
            catch
            {
                throw;
            }
        }
    }
}
