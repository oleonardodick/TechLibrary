using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using TechLibrary.Application.DTOs.Error.Response;
using TechLibrary.Application.DTOs.Login.Request;
using TechLibrary.Application.DTOs.Users.Response;
using TechLibrary.Infrastructure.Services.Security.Cryptography;
using TechLibrary.Tests.Utils.Users;

namespace TechLibrary.Tests.IntegrationTests.Login
{
    public class LoginControllerTest : IntegrationTestBase
    {
        private EncryptionService _encryptionService;
        public LoginControllerTest(IntegrationTestFactory factory) : base(factory) 
        {
            _encryptionService = new EncryptionService();
        }

        [Theory]
        [InlineData("invalid@mail.com","validPassword")]
        [InlineData("valid@mail.com", "invalidPassword")]
        public async Task ShouldReturnInvalidEmailOrPassword(string email, string password)
        {
            //Arange
            var user = FakeDataUsers.FakeUserWithSpecificValues(email: "valid@mail.com", password: _encryptionService.Encrypt("validPassword"));

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            RequestLoginDTO request = new RequestLoginDTO
            {
                Email = email,
                Password = password
            };

            //Act
            var response = await _client.PostAsJsonAsync("/login",request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseErrorMessagesDTO>(content);

            result.Should().NotBeNull();
            result.Errors.Should().Contain("E-mail e/ou senha inválidos.");
        }

        [Fact]
        public async Task ShouldReturnOkAndTheUserToken()
        {
            //Arrange
            var user = FakeDataUsers.FakeUserWithSpecificValues(email: "valid@mail.com", password: _encryptionService.Encrypt("validPassword"));

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            RequestLoginDTO request = new RequestLoginDTO
            {
                Email = "valid@mail.com",
                Password = "validPassword"
            };

            //Act
            var response = await _client.PostAsJsonAsync("/login", request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseRegisteredUserDTO>(content);

            result.Should().NotBeNull();
            result.Name.Should().Be(user.Name);
            result.AccessToken.Should().NotBeNullOrEmpty();
        }
    }
}
