using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace WebApplication1.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var newEmail = new MimeMessage();
                newEmail.From.Add(MailboxAddress.Parse( "TuEmail" ));
                newEmail.To.Add(MailboxAddress.Parse(email));
                newEmail.Subject = subject;

                // Crear el cuerpo del correo con formato HTML
                newEmail.Body = new TextPart("html")
                {
                    Text = message
                };

                using var smtp = new SmtpClient();

                // Ignorar la validación del certificado en entornos de desarrollo
                smtp.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

                smtp.Connect(
                    "smtp.gmail.com",
                    Convert.ToInt32(587),
                    SecureSocketOptions.StartTls
                );

                smtp.Authenticate(
                    "TuEmail",
                    "TuContraseña"
                );

                smtp.Send(newEmail);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}