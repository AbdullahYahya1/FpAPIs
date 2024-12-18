using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Business.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _Configuration;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration Configuration, ILogger<EmailSender> logger)
        {
            _Configuration = Configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                _logger.LogInformation("Preparing to send email...");

                var emailSettings = _Configuration.GetSection("EmailSettings");
                var mail = emailSettings["Username"];
                var pass = emailSettings["Password"];
                var host = emailSettings["Host"];
                var port = int.Parse(emailSettings["Port"]);

                _logger.LogInformation($"Using SMTP host: {host}, port: {port}, user: {mail}");

                var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(mail, pass),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(mail),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                _logger.LogInformation($"Sending email to {email}...");
                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while sending email to {email}.");
            }
        }
    }



}
