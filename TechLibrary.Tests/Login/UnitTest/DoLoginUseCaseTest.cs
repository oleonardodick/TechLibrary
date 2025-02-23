using Moq;
using TechLibrary.Application.DTOs.Login.Request;
using TechLibrary.Application.UseCases.Login;
using TechLibrary.Domain.Entities;
using TechLibrary.Domain.Exceptions;

namespace TechLibrary.Tests.Login.UnitTest
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
            await Assert.ThrowsAsync<InvalidLoginException>(async () => await DoLoginUseCase.ExecuteLoginAsync(request));
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
            await Assert.ThrowsAsync<InvalidLoginException>(async () => await DoLoginUseCase.ExecuteLoginAsync(request));
        }

        [Fact]
        public async Task ShouldLoginSuccessfully()
        {
            //Arrange

            var user = GetValidUser();

            UserRepository
                .Setup(x => x.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            EncryptionService
                .Setup(x => x.Decrypt(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            JwtService
                .Setup(x => x.GenerateToken(It.IsAny<Guid>()))
                .Returns("tokenAcesso");

            var request = new RequestLoginDTO
            {
                Email = "teste@mail.com",
                Password = "987654"
            };

            //Act
            var result = await DoLoginUseCase.ExecuteLoginAsync(request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal("tokenAcesso", result.AccessToken);
        }
    }
}
