using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using ShortLinkGenerator.Models;
using ShortLinkGenerator.ViewModels;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShortLinkGenerator.Services
{
    public class AccountServices : IAccountService
    {
        private readonly JwtConfig _appSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountServices(IOptions<JwtConfig> appSettings, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<JwtDto> GenerateToken(string userName)
        {
            try
            {
                //var user = await _userManager.FindByNameAsync(userName);
                var user = await _userManager.FindByNameAsync(userName);

                var roles = await _userManager.GetRolesAsync(user);

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_appSettings.Key);
                var claims = await GetValidClaims(user, roles);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    //Subject = new ClaimsIdentity(new Claim[]
                    //  {
                    // new Claim(ClaimTypes.Name, userName)
                    //  }),
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddYears(_appSettings.DurationInMinutes),
                    Issuer = _appSettings.Issuer,
                    Audience = _appSettings.Audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                //var refreshToken = GenerateRefreshToken();


                var result = new JwtDto()
                {
                    Status = true,
                    Roles = roles.ToList(),
                    //Claims = claims,
                    //User = user,
                    Token = tokenHandler.WriteToken(token)
                };

                return result;
            }
            catch (Exception ex)
            {
                return new JwtDto()
                {
                    Status = false,
                    Message = ex.Message,
                };
            }
        }


        private async Task<List<Claim>> GetValidClaims(ApplicationUser user, IList<string> userRoles)
        {
            IdentityOptions _options = new();
            var claims = new List<Claim>
            {
                new ("Id", user.Id),
                //new Claim("SecStamp", user.SecurityStamp),
                new (JwtRegisteredClaimNames.Email, user.UserName),
                new (JwtRegisteredClaimNames.Sub, user.UserName),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (_options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new (_options.ClaimsIdentity.UserNameClaimType, user.UserName),
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            //var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userClaims);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            return claims;
        }

        //public RefreshToken GenerateRefreshToken()
        //{
        //    var randomNumber = new byte[32];
        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        rng.GetBytes(randomNumber);
        //        return new RefreshToken()
        //        {
        //            ExpireDate = DateTime.UtcNow.AddYears(_appSettings.DurationInMinutes),
        //            Refresh_Token = Convert.ToBase64String(randomNumber) // کاربردی نیست
        //        };
        //    }
        //}

        public async Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_appSettings.Key);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero,
                //ValidIssuer=_appSettings.Issuer,
                //ValidAudience=_appSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }

        public async Task<UserClaimDto> GetUserFromClaim(ClaimsPrincipal claimsPrincipal)
        {
            var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;

            string username = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isBusiness = claimsIdentity.FindFirst("IsBusiness")?.Value;
            var businessId = claimsIdentity.FindFirst("BusinessId")?.Value;

            //var thisUser = await _userManager.FindByNameAsync(username);
            //var thisBusiness = await _businessRepository.GetAsync
            //    (
            //        where: s => s.Id == businessId
            //    );

            return new UserClaimDto()
            {
                Username = username,
            };
        }
    }
}
