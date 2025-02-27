using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using TechLibrary.Application.DTOs.Error.Response;
using TechLibrary.Application.DTOs.Login.Request;
using TechLibrary.Application.DTOs.Users.Response;
using TechLibrary.Infrastructure.Services.Security.Cryptography;
using TechLibrary.Tests.Utils.Books;
using TechLibrary.Tests.Utils.Users;

namespace TechLibrary.Tests.IntegrationTests.Checkouts
{
    public class CheckoutsControllerTest : IntegrationTestBase
    {
        private EncryptionService _encryptionService;
        public CheckoutsControllerTest(IntegrationTestFactory factory) : base(factory)
        {
            _encryptionService = new EncryptionService();
        }

        [Fact]
        public async Task ShouldReturnUnauthorized()
        {
            //Arrange
            var books = FakeDataBooks.FakeListBooks(10);
            await _dbContext.Books.AddRangeAsync(books);
            await _dbContext.SaveChangesAsync();

            var bookToCheckout = books[3].Id;

            //Act
            var response = await _client.PostAsync($"/checkouts/{bookToCheckout}",null);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseErrorMessagesDTO>(content);

            result.Should().NotBeNull();
            result.Errors.Should().Contain("O token JWT não foi enviado ou é inválido.");
        }

        [Fact]
        public async Task ShouldCheckOutABook()
        {
            //Arrange
            var email = "valid@mail.com";
            var password = "validPassword";
            var books = FakeDataBooks.FakeListBooks(10);
            var user = FakeDataUsers.FakeUserWithSpecificValues(email: email, password: _encryptionService.Encrypt(password));
            await _dbContext.Books.AddRangeAsync(books);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var bookToCheckout = books[5].Id;
            var requestLogin = new RequestLoginDTO
            {
                Email = email,
                Password = password
            };

            var loginResponse = await _client.PostAsJsonAsync("/login", requestLogin);
            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonConvert.DeserializeObject<ResponseRegisteredUserDTO>(loginContent);
            if(loginResult is not null)
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {loginResult.AccessToken}");

            //Act
            var response = await _client.PostAsync($"/checkouts/{bookToCheckout}", null);

            //Arrange
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var checkout = _dbContext.Checkouts.FirstOrDefault(c => c.BookId == bookToCheckout);
            checkout.Should().NotBeNull();
        }
    }
}
