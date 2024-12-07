using ShortLinkGenerator.ViewModels;

using System.Security.Claims;

namespace ShortLinkGenerator.Services
{
    public interface IAccountService
    {
        Task<JwtDto> GenerateToken(string userName);
        Task<UserClaimDto> GetUserFromClaim(ClaimsPrincipal claimsPrincipal);
    }
}
