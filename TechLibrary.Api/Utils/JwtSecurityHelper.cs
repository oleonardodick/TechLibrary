using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TechLibrary.Api.Utils
{
    public static class JwtSecurityHelper
    {
        public static SymmetricSecurityKey GetSecurityKey (IConfiguration configuration)
        {
            var secretKey = configuration["JwtSettings:SigningKey"];
            if(string.IsNullOrEmpty(secretKey))
                throw new ArgumentException("Chave de assinatura não configurada.");

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }
    }
}
