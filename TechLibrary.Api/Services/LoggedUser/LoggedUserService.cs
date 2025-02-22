using System.IdentityModel.Tokens.Jwt;
using TechLibrary.Domain.Entities;
using TechLibrary.Infrastructure.DataAccess;

namespace TechLibrary.Api.Services.LoggedUser
{
    public class LoggedUserService
    {
        private readonly IHttpContextAccessor _httpContextAcessor;
        private readonly TechLibraryDbContext _dbContext;
        public LoggedUserService(IHttpContextAccessor httpContextAcessor, TechLibraryDbContext dbContext)
        {
            _httpContextAcessor = httpContextAcessor;
            _dbContext = dbContext;
        }

        public User User()
        {
            var context = _httpContextAcessor.HttpContext ?? throw new InvalidOperationException();
            var authentication = context.Request.Headers.Authorization.ToString();
            //faz uma substring, começando a partir da posição 7 e pegando o resto da string
            var token = authentication["Bearer ".Length..].Trim();

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
            var userId = Guid.Parse(identifier);

            return _dbContext.Users.First(user => user.Id == userId);

        }
    }
}
