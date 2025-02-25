using TechLibrary.Application.DTOs.Users.Request;
using TechLibrary.Domain.Exceptions;
using Moq;
using TechLibrary.Domain.Entities;
using FluentAssertions;

namespace TechLibrary.Tests.UnitTests.Users
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
            exception.Should().NotBeNull();
            qtdErrors.Should().Be(1);
            exception.GetErrorMessages().Should().Contain("O e-mail não é válido.");

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
            exception.Should().NotBeNull();
            qtdErrors.Should().Be(1);
            exception.GetErrorMessages().Should().Contain("O nome é obrigatório.");

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
            exception.Should().NotBeNull();
            qtdErrors.Should().Be(1);
            exception.GetErrorMessages().Should().Contain("A senha é obrigatória.");

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
            exception.Should().NotBeNull();
            qtdErrors.Should().Be(1);
            exception.GetErrorMessages().Should().Contain("A senha deve possuir no mínimo 6 caracteres.");

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
            exception.Should().NotBeNull();
            qtdErrors.Should().BeGreaterThan(1);
            exception.GetErrorMessages().Should().Contain("A senha deve possuir no mínimo 6 caracteres.");
            exception.GetErrorMessages().Should().Contain("O nome é obrigatório.");
            exception.GetErrorMessages().Should().Contain("O e-mail não é válido.");

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
            exception.Should().NotBeNull();
            qtdErrors.Should().Be(1);
            exception.GetErrorMessages().Should().Contain("E-mail já cadastrado.");
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
            var generatedToken = "tokenJWT";
            User userCreated = default!;

            UserRepository
                .Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            UserRepository
                .Setup(x => x.CreateUserAsync(It.IsAny<User>()))
                .Callback<User>(user => userCreated = user);

            EncryptionService
                .Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(encryptedPassword);

            JwtService
                .Setup(x => x.GenerateToken(It.IsAny<Guid>()))
                .Returns(generatedToken);

            //Act
            var result = await RegisterUserUseCase.RegisterUser(request);

            //Assert
            result.Should().NotBeNull();
            request.Name.Should().BeSameAs(result.Name);
            result.AccessToken.Should().Be(generatedToken);
            userCreated.Password.Should().Be(encryptedPassword);
        }
    }
}
