using System.Net;
using System.Net.Mail;

namespace Company.Amr.PL.Helpers
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            // Mail Server : Gmail 
            // SMTP
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("amrdude98@gmail.com", "kfgqoewsexvsgeoq"); // Sender : the Person / Account that send emails 
                client.Send("amrdude98@gmail.com", email.To, email.Subject, email.Body);



                return true;

            }
            catch (Exception e)
            {

                return false;
            }
        }
    }
}
