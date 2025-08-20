using esok.api.Data;
using esok.api.Interfaces.Application;
using esok.api.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace esok.api.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAppSettingsProvider _appSettingsProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public AuthenticationService(
            IAppSettingsProvider appSettingsProvider,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext applicationDbContext)
        {
            _appSettingsProvider = appSettingsProvider;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<string> CreateToken(ApplicationUser user)
        {
            var jwtSettings = _appSettingsProvider
                .GetJWTSettings();

            var issuer = jwtSettings
                .Issuer;

            var audience = jwtSettings
                .Audience;

            var key = Encoding
                .ASCII
                .GetBytes(jwtSettings.Key);

            var durationInMinutes = jwtSettings
                .DurationInMinutes;

            var groupId = await GetUserGroupId(user.Id);

            var role = await _userManager
                .GetRolesAsync(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim("userId", user.Id.ToString()),
                        new Claim("groupId", groupId.ToString()),
                        new Claim("nameSurname", user.NameSurname),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(ClaimTypes.Role, role.Single())
                    }),
                Expires = DateTime.UtcNow.AddMinutes(durationInMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }

        public async Task<ApplicationUser> GetUser()
        {
            var claims = _httpContextAccessor
                .HttpContext
                .User
                .Claims
                .ToList();

            var userId = claims
                .Single(s => s.Type == "userId")
                .Value;

            var user = await _userManager
                .FindByIdAsync(userId);

            var groupId = claims
                .Single(s => s.Type == "groupId")
                .Value;

            user.GroupId = Convert.ToInt32(groupId);
             
            return user;
        }

        private async Task<int> GetUserGroupId(int userId)
        {
            var groupIdByUserGroup = await _applicationDbContext
                .UserGroup
                .Where(x => x.EmployeeId == userId)
                .FirstOrDefaultAsync();

            if (groupIdByUserGroup != null)
                return groupIdByUserGroup.GroupId;

            var groupIdByGroup = await _applicationDbContext
                .Group
                .Where(x => x.ManagerId == userId)
                .FirstOrDefaultAsync();

            return groupIdByGroup.Id;
        }
    }
}
