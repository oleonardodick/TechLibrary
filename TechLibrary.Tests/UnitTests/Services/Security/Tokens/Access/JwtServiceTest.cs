using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TechLibrary.Domain.Interfaces.Services;
using TechLibrary.Infrastructure.Services.Security.Tokens.Access;

namespace TechLibrary.Tests.UnitTests.Services.Security.Tokens.Access
{
    public class JwtServiceTest
    {
        
        private JwtService _jwtService = default!;

        [Fact]
        public void ShouldGenerateJwtToken()
        {
            //Arrange
            Mock<IJwtKeyProvider> jwtKeyProvider = new Mock<IJwtKeyProvider>();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("uagqCB942DxV3FOsdvtN5oy4z09jxUtC"));
            jwtKeyProvider
                .Setup(x => x.GetSigningKey())
                .Returns(key);

            _jwtService = new JwtService(jwtKeyProvider.Object);
            var userId = Guid.NewGuid();

            //Act
            var token = _jwtService.GenerateToken(userId);

            //Assert
            token.Should().NotBeNullOrEmpty();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            jwtToken.Should().NotBeNull();
            jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == userId.ToString());
        }
    }
}
