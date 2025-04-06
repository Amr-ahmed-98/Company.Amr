using Company.Amr.PL.Helpers;

namespace Company.Amr.PL.Helper
{
    public interface IMailService
    {
        public void SendEmail(Email email);
    }
}
