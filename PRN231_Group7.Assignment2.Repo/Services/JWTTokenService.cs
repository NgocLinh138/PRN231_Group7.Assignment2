using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN231_Group7.Assignment2.Repo.Services.Interface;
using PRN231_Group7.Assignment2.Repo.Services.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PRN231_Group7.Assignment2.Repo.Services
{
    public class JWTTokenService : IJWTTokenService
    {
        private readonly JWTOption jwtOption = new JWTOption();

        public JWTTokenService(IConfiguration configuration)
        {
            configuration.GetSection(nameof(JWTOption)).Bind(jwtOption);
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SecretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                audience: jwtOption.Audience,
                issuer: jwtOption.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtOption.ExpireMin),
                signingCredentials: signingCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(jwtOption.SecretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // on production set true
                ValidateAudience = false, // on production set true
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),   // Khi len moi truong Product thi nen dat Key vao
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            var jwtSecurityToken = (JwtSecurityToken)securityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
                throw new SecurityTokenException("Invalid Token");

            return principal;
        }
    }
}
