using FluentAssertions;
using TechLibrary.Infrastructure.Services.Security.Cryptography;

namespace TechLibrary.Tests.UnitTests.Services.Security.Cryptography
{
    public class EncryptionServiceTest
    {
        private EncryptionService _encryptionService;

        public EncryptionServiceTest()
        {
            _encryptionService = new EncryptionService();
        }
        [Fact]
        public void ShouldEncryptTheText()
        {
            //Arrange
            var originalText = "passwordToEncrypt";

            //Act
            var encryptedText = _encryptionService.Encrypt(originalText);

            //Assert
            encryptedText.Should().NotBeNullOrEmpty();
            encryptedText.Should().NotBe(originalText);
        }

        [Fact]
        public void ShouldDecryptTheText()
        {
            //Arrange
            var originalText = "passwordToEncrypt";
            var encryptedText = _encryptionService.Encrypt(originalText);

            //Act
            var sameText = _encryptionService.Decrypt(originalText, encryptedText);

            //Assert
            sameText.Should().BeTrue();
        }
    }
}
