using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using TechLibrary.Application.DTOs.Error.Response;
using TechLibrary.Application.DTOs.Users.Request;
using TechLibrary.Application.DTOs.Users.Response;
using TechLibrary.Domain.Entities;
using TechLibrary.Tests.Utils.Users;

namespace TechLibrary.Tests.IntegrationTests.Users
{
    public class UsersControllerTest : IntegrationTestBase
    {
        public UsersControllerTest(IntegrationTestFactory factory) : base(factory) { }

        [Fact]
        public async Task ShouldReturnBadRequestWhenHasValidationError()
        {
            //Arrange
            var user = FakeDataUsers.FakeUserWithSpecificValues(email: "invalidEmail");

            var request = new RequestRegisterUserDTO
            {
                Email = user.Email,
                Name = user.Name,
                Password = user.Password
            };

            //Act
            var response = await _client.PostAsJsonAsync("/users", request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseErrorMessagesDTO>(content);

            result.Should().NotBeNull();
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ShouldCreateTheUserAndReturnCreated()
        {
            //Arrange
            var user = FakeDataUsers.FakeListUsers(1);

            var request = new RequestRegisterUserDTO
            {
                Email = user[0].Email,
                Name = user[0].Name,
                Password = user[0].Password
            };

            //Act
            var response = await _client.PostAsJsonAsync("/users", request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseRegisteredUserDTO>(content);

            result.Should().NotBeNull();
            result.Name.Should().Be(user[0].Name);
            result.AccessToken.Should().NotBeNullOrEmpty();

            var createdUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user[0].Email);
            createdUser.Should().NotBeNull();
        }
    }
}
