using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TechLibrary.Domain.Interfaces.Services;

namespace TechLibrary.Infrastructure.Services.Security.Tokens.Configuration
{
    public class JwtKeyProvider : IJwtKeyProvider
    {
        private readonly IConfiguration _configuration;

        public JwtKeyProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public SymmetricSecurityKey GetSigningKey()
        {
            var jwtSettingsSection = _configuration.GetSection("JwtSettings");
            if (!jwtSettingsSection.Exists())
            {
                throw new InvalidOperationException("A configuração 'JwtSettings' não foi encontrada no appsettings.json.");
            }

            var secretKey = jwtSettingsSection.GetValue<string>("SecretKey");
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("A chave secreta do JWT não pode ser nula ou vazia.");
            }

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }

        public void ConfigureJwtBearerOptions(JwtBearerOptions options)
        {
            var key = GetSigningKey();
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key
            };
        }
    }
}
