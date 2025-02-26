using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TechLibrary.Domain.Interfaces.Services;

namespace TechLibrary.Infrastructure.Services.Security.Tokens.Access
{
    public class JwtService : IJwtService
    {
        private readonly IJwtKeyProvider _jwtKeyProvider;

        public JwtService(IJwtKeyProvider jwtKeyProvider)
        {
            _jwtKeyProvider = jwtKeyProvider;
        }

        public string GenerateToken(Guid userId)
        {
            var key = _jwtKeyProvider.GetSigningKey();
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Expires = DateTime.UtcNow.AddHours(1),
            //    Subject = new ClaimsIdentity(
            //    [
            //        new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            //    ]),
            //    SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            //};
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //return tokenHandler.WriteToken(token);
        }
    }
}
