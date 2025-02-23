using TechLibrary.Application.DTOs.Users.Request;
using TechLibrary.Domain.Exceptions;
using Moq;
using TechLibrary.Domain.Entities;

namespace TechLibrary.Tests.Users.UnitTest
{
    public class RegisterUserUseCaseTest : RegisterUserUseCaseTestBase
    {
        [Fact]
        public async Task InvalidEmail_ShouldThrownErrorOnValidationException()
        {
            //Arrange
            var request = new RequestRegisterUserDTO
            {
                Email = "invalidEmail",
                Name = "Teste",
                Password = "123456"
            };

            //Act
            var result = RegisterUserUseCase.RegisterUser(request);
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => result);
            var qtdErrors = exception.GetErrorMessages().Count;

            //Assert
            Assert.NotNull(exception);
            Assert.Equal(1, qtdErrors);
            Assert.Contains("O e-mail não é válido.", exception.GetErrorMessages());

        }

        [Fact]
        public async Task InvalidName_ShouldThrownErrorOnValidationException()
        {
            //Arrange
            var request = new RequestRegisterUserDTO
            {
                Email = "teste@mail.com",
                Name = "",
                Password = "123456"
            };

            //Act
            var result = RegisterUserUseCase.RegisterUser(request);
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => result);
            var qtdErrors = exception.GetErrorMessages().Count;

            //Assert
            Assert.NotNull(exception);
            Assert.Equal(1, qtdErrors);
            Assert.Contains("O nome é obrigatório.", exception.GetErrorMessages());

        }

        [Fact]
        public async Task PasswordNotProvided_ShouldThrownErrorOnValidationException()
        {
            //Arrange
            var request = new RequestRegisterUserDTO
            {
                Email = "teste@mail.com",
                Name = "teste",
                Password = ""
            };

            //Act
            var result = RegisterUserUseCase.RegisterUser(request);
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => result);
            var qtdErrors = exception.GetErrorMessages().Count;

            //Assert
            Assert.NotNull(exception);
            Assert.Equal(1, qtdErrors);
            Assert.Contains("A senha é obrigatória.", exception.GetErrorMessages());

        }

        [Fact]
        public async Task InvalidPassword_ShouldThrownErrorOnValidationException()
        {
            //Arrange
            var request = new RequestRegisterUserDTO
            {
                Email = "teste@mail.com",
                Name = "teste",
                Password = "1234"
            };

            //Act
            var result = RegisterUserUseCase.RegisterUser(request);
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => result);
            var qtdErrors = exception.GetErrorMessages().Count;

            //Assert
            Assert.NotNull(exception);
            Assert.Equal(1, qtdErrors);
            Assert.Contains("A senha deve possuir no mínimo 6 caracteres.", exception.GetErrorMessages());

        }

        [Fact]
        public async Task MoreThanOneError_ShouldThrownErrorOnValidationException()
        {
            //Arrange
            var request = new RequestRegisterUserDTO
            {
                Email = "invalidEmail",
                Name = "",
                Password = "1234"
            };

            //Act
            var result = RegisterUserUseCase.RegisterUser(request);
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => result);
            var qtdErrors = exception.GetErrorMessages().Count;

            //Assert
            Assert.NotNull(exception);
            Assert.Equal(3, qtdErrors);
            Assert.Contains("A senha deve possuir no mínimo 6 caracteres.", exception.GetErrorMessages());
            Assert.Contains("O nome é obrigatório.", exception.GetErrorMessages());
            Assert.Contains("O e-mail não é válido.", exception.GetErrorMessages());

        }

        [Fact]
        public async Task EmailAlreadyExists_ShouldThrownErrorOnValidationException()
        {
            //Arrange
            var request = new RequestRegisterUserDTO
            {
                Email = "mail@test.com",
                Name = "teste",
                Password = "123456"
            };

            UserRepository
                .Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());

            //Act
            var result = RegisterUserUseCase.RegisterUser(request);
            var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(() => result);
            var qtdErrors = exception.GetErrorMessages().Count;

            //Assert
            Assert.NotNull(exception);
            Assert.Equal(1, qtdErrors);
            Assert.Contains("E-mail já cadastrado.", exception.GetErrorMessages());
        }

        [Fact]
        public async Task ShouldRegisterTheUser()
        {
            //Arrange
            var request = new RequestRegisterUserDTO
            {
                Email = "mail@test.com",
                Name = "teste",
                Password = "123456"
            };

            var encryptedPassword = "encryptedPassword";

            UserRepository
                .Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            UserRepository
                .Setup(x => x.CreateUserAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            EncryptionService
                .Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(encryptedPassword);

            JwtService
                .Setup(x => x.GenerateToken(It.IsAny<Guid>()))
                .Returns("tokenAcesso");

            //Act
            var result = await RegisterUserUseCase.RegisterUser(request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(request.Name, result.Name);
            Assert.Equal("tokenAcesso", result.AccessToken);

            UserRepository.Verify(x => x.CreateUserAsync(It.Is<User>(user =>
                user.Email == request.Email &&
                user.Name == request.Name &&
                user.Password == encryptedPassword
            )), Times.Once);
        }
    }
}
