using esok.api.Application.Settings;

namespace esok.api.Interfaces.Application
{
    public interface IAppSettingsProvider
    {
        EmailSettings GetEmailSettings();
        JWTSettings GetJWTSettings();
    }
}
