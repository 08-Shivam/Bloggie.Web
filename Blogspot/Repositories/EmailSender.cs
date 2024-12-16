using Blogspot.Models.ViewModel.Email;
using System.Net;
using System.Net.Mail;

namespace Blogspot.Repositories
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;

        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<bool> EmailSendAsync(string email, string subject, string message)
        {
            bool status = false;

            try
            {
                GetEmailSettings getEmailSettings = new GetEmailSettings()
                {
                    SecretKey = configuration.GetValue<string>("AppSettings:SecretKey"),
                    From = configuration.GetValue<string>("AppSettings:EmailSettings:From"),
                    SmtpServer = configuration.GetValue<string>("AppSettings:EmailSettings:SmtpServer"),
                    Port = configuration.GetValue<int>("AppSettings:EmailSettings:Port"),
                    EnableSSL = configuration.GetValue<bool>("AppSettings:EmailSettings:EnableSSL")
                };
                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(getEmailSettings.From),
                    Subject = subject,
                    Body = message
                };

                mailMessage.To.Add(email);
                SmtpClient smtpClient = new SmtpClient(getEmailSettings.SmtpServer)
                {
                    Port = getEmailSettings.Port,
                    Credentials = new NetworkCredential(getEmailSettings.From, getEmailSettings.SecretKey),
                    EnableSsl = getEmailSettings.EnableSSL
                };

                await smtpClient.SendMailAsync(mailMessage);
                status = true;
            }
            catch (Exception ex)
            {
                // Consider logging the exception here for debugging
                status = false;
            }
            return status;
        }

    }
}
