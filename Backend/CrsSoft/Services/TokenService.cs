using CrsSoft.Interfaces;
using CrsSoft.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CrsSoft.Services
{
    public class TokenService : ITokenService
    {
        readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<GenerateTokenResponse> GenerateToken(GenerateTokenRequest request)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var exp = now.AddHours(24);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, request.UserId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, request.UserId.ToString()),
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.Name, request.Email)
            };

            var jwt = new JwtSecurityToken(
                issuer: configuration["AppSettings:ValidIssuer"],
                audience: configuration["AppSettings:ValidAudience"],
                claims: claims,
                notBefore: now,
                expires: exp,
                signingCredentials: creds
            );

            return Task.FromResult(new GenerateTokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                TokenExpireDate = exp
            });
        }
    }
}