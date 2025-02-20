using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Infrastructure.Security.Cryptography;
using TechLibrary.Api.Infrastructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Login.DoLogin
{
    public class DoLoginUseCase
    {
        private readonly JwtTokenGenerator _tokenGenerator;
        private readonly BCryptAlgorithm _cryptography;
        private readonly TechLibraryDbContext _dbContext;

        public DoLoginUseCase(JwtTokenGenerator tokenGenerator, BCryptAlgorithm cryptography, TechLibraryDbContext dbContext)
        {
            _tokenGenerator = tokenGenerator;
            _cryptography = cryptography;
            _dbContext = dbContext;
        }

        public ResponseRegisteredUserJson Execute(RequestLoginJson request)
        {
            var user = _dbContext.Users.FirstOrDefault(user => user.Email == request.Email);
            if (user is null)
            {
                throw new InvalidLoginException();
            }

            var passwordIsValid = _cryptography.VerifyPassword(request.Password, user);

            if (passwordIsValid == false) throw new InvalidLoginException();

            return new ResponseRegisteredUserJson 
            {
                Name = user.Name,
                AccessToken = _tokenGenerator.GenerateToken(user),
            };
        }
    }
}
