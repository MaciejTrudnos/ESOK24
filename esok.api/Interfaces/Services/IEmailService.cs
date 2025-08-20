namespace esok.api.Interfaces.Services
{
    public interface IEmailService
    {
        void SendRegisterConfirmEmail(string userEmail, string token);
        void SendResetPasswordEmail(string userEmail, string token);
        void SendRegisterConfirmEmailToEmployee(string userEmail, string token);
    }
}
