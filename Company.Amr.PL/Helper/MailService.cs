
using Company.Amr.PL.Helpers;
using Company.Amr.PL.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Company.Amr.PL.Helper
{
    public class MailService(IOptions<MailSettings> _options) : IMailService
    {
       
        public void SendEmail(Email email)
        {
            // build Message
            var mail = new MimeMessage();
            mail.Subject = email.Subject;
            mail.From.Add(new MailboxAddress(_options.Value.DisplayName, _options.Value.Mail));
            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();

            // Establish Connection
            using var smtp = new SmtpClient();
             smtp.Connect(_options.Value.Host, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Value.Mail, _options.Value.Password);
            // Send Mesaage
            smtp.Send(mail);

        }
    }
}
