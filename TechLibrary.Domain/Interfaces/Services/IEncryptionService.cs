namespace TechLibrary.Domain.Interfaces.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string value);
        bool Decrypt(string value, string hashedValue);
    }
}
