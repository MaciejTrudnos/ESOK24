using esok.api.Interfaces.Application;
using esok.api.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Web;

namespace esok.api.Services
{
    public class EmailService : IEmailService
    {
        private readonly IAppSettingsProvider _appSettingsProvider;

        public EmailService(IAppSettingsProvider appSettingsProvider)
        {
            _appSettingsProvider = appSettingsProvider;
        }

        public void SendRegisterConfirmEmail(string userEmail, string token)
        {
            var subject = "Aktywacja konta esok24.pl";

            var text = "Dziękujemy za rejestrację w aplikacji esok24.pl <br>"
                        + "Kliknij poniżej w aktywuj konto, aby potwierdzić swój adres email <br><br>"
                        + $"<p><a href=\"https://app.esok24.pl/confirmEmail?token={HttpUtility.UrlEncode(token)}&email={userEmail}\">Aktywuj konto</a></p> <br>"
                        + "Po aktywacji konta będziesz mógł się zalogować.";

            SendEmail(userEmail, subject, text);
        }

        public void SendResetPasswordEmail(string userEmail, string token)
        {
            var subject = "Resetowanie hasła";
           
            var text = "Kliknij poniżej w zmień hasło, aby ustalić nowe hasło w aplikacji esok24.pl <br><br>"
                        + $"<p><a href=\"https://app.esok24.pl/resetPassword?token={HttpUtility.UrlEncode(token)}&email={userEmail}\">Zmień hasło</a></p><br><br>"
                        + "Jeżeli nie chcesz zmieniać swojego hasła po prostu zignoruj tę wiadomość.";

            SendEmail(userEmail, subject, text);
        }

        public void SendRegisterConfirmEmailToEmployee(string userEmail, string token)
        {
            var subject = "Aktywacja konta esok24.pl";

            var text = "Twój adres e-mail został dodany jako pracownik w aplikacji esok24.pl <br>"
                        + "Kliknij poniżej w aktywuj konto, aby potwierdzić swój adres email <br><br>"
                        + $"<p><a href=\"https://app.esok24.pl/confirmEmployee?token={HttpUtility.UrlEncode(token)}&email={userEmail}\">Aktywuj konto</a></p> <br>"
                        + "Po aktywacji konta będziesz mógł się zalogować.";

            SendEmail(userEmail, subject, text);
        }

        private void SendEmail(string userEmail, string subject, string text)
        {
            var emailProvider = _appSettingsProvider
                .GetEmailSettings();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ESOK24", emailProvider.UserName));
            message.To.Add(new MailboxAddress("", userEmail));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = text
            };

            using var client = new SmtpClient();
            client.Connect(emailProvider.Host, emailProvider.Port, SecureSocketOptions.None);

            // Note: only needed if the SMTP server requires authentication
            client.Authenticate(emailProvider.UserName, emailProvider.Password);

            client.Send(message);
            client.Disconnect(true);
        }
    }
}
