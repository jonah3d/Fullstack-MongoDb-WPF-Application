using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace StoreFrontUi.Utils
{
    public class EmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _senderEmail;
        private readonly string _senderPassword;
        private readonly bool _enableSsl;
        public EmailSender(string smtpServer, int smtpPort, string senderEmail, string senderPassword, bool enableSsl = true)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _senderEmail = senderEmail;
            _senderPassword = senderPassword;
            _enableSsl = enableSsl;
        }
        public async Task<bool> SendInvoiceEmailAsync(string recipientEmail, string subject, string body, string attachmentPath)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(_senderEmail);
                    mail.To.Add(recipientEmail);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    
                    if (File.Exists(attachmentPath))
                    {
                        Attachment attachment = new Attachment(attachmentPath);
                        mail.Attachments.Add(attachment);
                    }
                    else
                    {
                        MessageBox.Show($"Attachment file not found: {attachmentPath}");
                        return false;
                    }

                    using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                    {
                        smtpClient.EnableSsl = _enableSsl;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(_senderEmail, _senderPassword);

                        await smtpClient.SendMailAsync(mail);
                    }
                }

                MessageBox.Show($"Email sent successfully to {recipientEmail}");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
