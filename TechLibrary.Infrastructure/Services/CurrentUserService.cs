using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using TechLibrary.Infrastructure.DataAccess;
using TechLibrary.Domain.Interfaces.Services;

namespace TechLibrary.Infrastructure.Services.CurrentUserService
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAcessor;
        public Guid UserId { get; private set; }
        public CurrentUserService(IHttpContextAccessor httpContextAcessor, TechLibraryDbContext dbContext)
        {
            _httpContextAcessor = httpContextAcessor;
            InitializeUser();
        }

        private void InitializeUser() 
        {
            var context = _httpContextAcessor.HttpContext;
            if (context == null)
                throw new InvalidOperationException("Contexto HTTP não disponível.");
            var authentication = context.Request.Headers.Authorization.ToString();
            if(string.IsNullOrEmpty(authentication))
                throw new InvalidOperationException("Token de autenticação não encontrado.");
            var token = authentication["Bearer ".Length..].Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
            UserId = Guid.Parse(identifier);
        }
    }
}
