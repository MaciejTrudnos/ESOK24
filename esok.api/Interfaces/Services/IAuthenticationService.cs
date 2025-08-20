using esok.api.Data;

namespace esok.api.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<string> CreateToken(ApplicationUser user);
        Task<ApplicationUser> GetUser();
    }
}
