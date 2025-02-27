using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TechLibrary.Domain.Exceptions;
using TechLibrary.Domain.Interfaces.Services;

namespace TechLibrary.Infrastructure.Services.Security.Tokens.Configuration
{
    public class JwtAuthenticationConfigurator
    {
        private readonly IJwtKeyProvider _jwtKeyProvider;

        public JwtAuthenticationConfigurator(IJwtKeyProvider jwtKeyProvider)
        {
            _jwtKeyProvider = jwtKeyProvider;
        }

        // Método para configurar as opções do JWT
        public void ConfigureJwtBearerOptions(JwtBearerOptions options)
        {
            var key = _jwtKeyProvider.GetSigningKey();
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    throw new InvalidTokenException();
                }
            };
        }
    }
}
