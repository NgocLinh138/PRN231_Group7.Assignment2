using System.Security.Claims;

namespace PRN231_Group7.Assignment2.Repo.Services.Interface
{
    public interface IJWTTokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
