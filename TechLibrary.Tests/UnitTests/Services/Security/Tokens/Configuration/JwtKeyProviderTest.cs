using Azure.Core;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Text;
using TechLibrary.Application.UseCases.Login;
using TechLibrary.Domain.Exceptions;
using TechLibrary.Infrastructure.Services.Security.Tokens.Configuration;

namespace TechLibrary.Tests.UnitTests.Services.Security.Tokens.Configuration
{
    public class JwtKeyProviderTest
    {
        private JwtKeyProvider _jwtKeyProvider = default!;

        [Fact]
        public void ShouldFindTheSecretKey()
        {
            var key = "uagqCB942DxV3FOsdvtN5oy4z09jxUtC";
            //Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"JwtSettings:SecretKey", key},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _jwtKeyProvider = new JwtKeyProvider(configuration);

            //Act
            SymmetricSecurityKey keyReturned = _jwtKeyProvider.GetSigningKey();

            //Assert
            var keyConverted = Encoding.UTF8.GetString(keyReturned.Key);
            keyConverted.Should().NotBeNull();
            keyConverted.Should().Be(key);
        }

        [Fact]
        public void ShouldThrowAnExceptionWhenNoJwtSettingsFinded()
        {
            //Arrange
            var inMemorySettings = new Dictionary<string, string?> {};

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _jwtKeyProvider = new JwtKeyProvider(configuration);

            //Act & Assert
            FluentActions
                .Invoking(() => _jwtKeyProvider.GetSigningKey())
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("A configuração 'JwtSettings' não foi encontrada no appsettings.json.");
        }

        [Fact]
        public void ShouldThrowAnExceptionWhenTheJwtSettingsHasNoKeyInformed()
        {
            //Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"JwtSettings:SecretKey", ""},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _jwtKeyProvider = new JwtKeyProvider(configuration);

            //Act & Assert
            FluentActions
                .Invoking(() => _jwtKeyProvider.GetSigningKey())
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("A chave secreta do JWT não pode ser nula ou vazia.");
        }
    }
}
