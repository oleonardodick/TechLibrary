using Moq;
using TechLibrary.Application.UseCases.Login;
using TechLibrary.Domain.Entities;
using TechLibrary.Domain.Interfaces.Repositories;
using TechLibrary.Domain.Interfaces.Services;

namespace TechLibrary.Tests.UnitTests.Login
{
    public abstract class DoLoginUseCaseTestBase
    {
        protected readonly Mock<IUserRepository> UserRepository;
        protected readonly Mock<IJwtService> JwtService;
        protected readonly Mock<IEncryptionService> EncryptionService;
        protected readonly DoLoginUseCase DoLoginUseCase;

        public DoLoginUseCaseTestBase()
        {
            UserRepository = new Mock<IUserRepository>();
            JwtService = new Mock<IJwtService>();
            EncryptionService = new Mock<IEncryptionService>();
            DoLoginUseCase = new DoLoginUseCase(UserRepository.Object, JwtService.Object, EncryptionService.Object);
        }

        protected User GetValidUser()
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Name = "Teste",
                Email = "teste@mail.com",
                Password = "hashedPassword"
            };
        }
    }
}
