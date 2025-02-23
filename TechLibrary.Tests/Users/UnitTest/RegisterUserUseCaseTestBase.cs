using FluentValidation;
using Moq;
using TechLibrary.Application.DTOs.Users.Request;
using TechLibrary.Application.UseCases.Users;
using TechLibrary.Application.Validators;
using TechLibrary.Domain.Interfaces.Repositories;
using TechLibrary.Domain.Interfaces.Services;

namespace TechLibrary.Tests.Users.UnitTest
{
    public abstract class RegisterUserUseCaseTestBase
    {
        protected readonly Mock<IUserRepository> UserRepository;
        protected readonly Mock<IEncryptionService> EncryptionService;
        protected readonly Mock<IJwtService> JwtService;
        protected readonly IValidator<RequestRegisterUserDTO> Validator;
        protected readonly RegisterUserUseCase RegisterUserUseCase;

        public RegisterUserUseCaseTestBase()
        {
            UserRepository = new Mock<IUserRepository>();
            EncryptionService = new Mock<IEncryptionService>();
            JwtService = new Mock<IJwtService>();
            Validator = new RegisterUserValidator();
            RegisterUserUseCase = new RegisterUserUseCase(UserRepository.Object, EncryptionService.Object, JwtService.Object, Validator);
        }
    }
}
