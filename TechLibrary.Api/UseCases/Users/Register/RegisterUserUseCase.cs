using FluentValidation.Results;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Domain.Entities;
using TechLibrary.Exception;
using TechLibrary.Infrastructure.DataAccess;
using TechLibrary.Infrastructure.Security.Cryptography;
using TechLibrary.Infrastructure.Security.Tokens.Access;

namespace TechLibrary.Api.UseCases.Users.Register
{
    public class RegisterUserUseCase
    {
        private readonly JwtTokenGenerator _tokenGenerator;
        private readonly BCryptAlgorithm _cryptography;
        private readonly TechLibraryDbContext _dbContext;

        public RegisterUserUseCase(JwtTokenGenerator tokenGenerator, BCryptAlgorithm cryptography, TechLibraryDbContext dbContext)
        {
            _tokenGenerator = tokenGenerator;
            _cryptography = cryptography;
            _dbContext = dbContext;
        }
        public ResponseRegisteredUserJson Execute(RequestUserJson request)
        {
            Validate(request);

            var entity = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = _cryptography.HashPassword(request.Password),
            };

            _dbContext.Users.Add(entity);
            _dbContext.SaveChanges();

            return new ResponseRegisteredUserJson 
            {
                Name = entity.Name,
                AccessToken = _tokenGenerator.GenerateToken(entity),
            };
        }

        private void Validate(RequestUserJson request)
        {
            var validator = new RegisterUserValidator();
            var result = validator.Validate(request);

            var existsUserWithEmail = _dbContext.Users.Any(user => user.Email.Equals(request.Email));
            if(existsUserWithEmail) result.Errors.Add(new ValidationFailure("Email", "E-mail já cadastrado."));

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
