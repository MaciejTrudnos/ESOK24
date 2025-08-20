using esok.api.Application.Settings;
using esok.api.Interfaces.Application;

namespace esok.api.Application
{
    public class AppSettingsProvider : IAppSettingsProvider
    {
        private readonly IConfiguration _configuration;

        public AppSettingsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EmailSettings GetEmailSettings()
        {
            var emailUserName = _configuration
                .GetValue<string>("EmailSettings:UserName");

            var emailPassword = _configuration
                .GetValue<string>("EmailSettings:Password");

            var host = _configuration
                .GetValue<string>("EmailSettings:Host");

            var port = _configuration
                .GetValue<int>("EmailSettings:Port");


            return new EmailSettings(emailUserName, emailPassword, host, port);
        }

        public JWTSettings GetJWTSettings()
        {
            var key = _configuration
                .GetValue<string>("JWTSettings:Key");

            var issuer = _configuration
                .GetValue<string>("JWTSettings:Issuer");

            var audience = _configuration
                .GetValue<string>("JWTSettings:Audience");

            var durationInMinutes = _configuration
                .GetValue<int>("JWTSettings:DurationInMinutes");

            return new JWTSettings(key, issuer, audience, durationInMinutes);
        }
    }
}
