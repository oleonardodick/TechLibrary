using FluentAssertions;
using Moq;
using TechLibrary.Application.DTOs.Login.Request;
using TechLibrary.Application.UseCases.Login;
using TechLibrary.Domain.Entities;
using TechLibrary.Domain.Exceptions;

namespace TechLibrary.Tests.UnitTests.Login
{
    public class DoLoginUseCaseTest : DoLoginUseCaseTestBase
    {
        [Fact]
        public async Task InvalidEmail_ShouldThrownInvalidLoginException()
        {
            //Arrange
            UserRepository
                .Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            var request = new RequestLoginDTO
            {
                Email = "mail@teste.com",
                Password = "123456"
            };

            //Act & Assert
            await FluentActions
                .Awaiting(async () => await DoLoginUseCase.ExecuteLoginAsync(request))
                .Should()
                .ThrowAsync<InvalidLoginException>();

        }

        [Fact]
        public async Task InvalidPassword_ShouldThrownInvalidLoginException()
        {
            //Arrange

            var user = GetValidUser();

            UserRepository
                .Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            EncryptionService
                .Setup(x => x.Decrypt(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            var request = new RequestLoginDTO
            {
                Email = "teste@mail.com",
                Password = "123456"
            };

            //Act & Assert
            await FluentActions
                .Awaiting(async () => await DoLoginUseCase.ExecuteLoginAsync(request))
                .Should()
                .ThrowAsync<InvalidLoginException>();
        }

        [Fact]
        public async Task ShouldLoginSuccessfully()
        {
            //Arrange

            var user = GetValidUser();
            var accessToken = "jwtToken";

            UserRepository
                .Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            EncryptionService
                .Setup(x => x.Decrypt(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            JwtService
                .Setup(x => x.GenerateToken(It.IsAny<Guid>()))
                .Returns(accessToken);

            var request = new RequestLoginDTO
            {
                Email = "teste@mail.com",
                Password = "987654"
            };

            //Act
            var result = await DoLoginUseCase.ExecuteLoginAsync(request);

            //Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(user.Name);
            result.AccessToken.Should().Be(accessToken);
        }
    }
}
