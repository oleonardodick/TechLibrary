using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace TechLibrary.Domain.Interfaces.Services
{
    public interface IJwtKeyProvider
    {
        SymmetricSecurityKey GetSigningKey();
        void ConfigureJwtBearerOptions(JwtBearerOptions option);
    }
}
