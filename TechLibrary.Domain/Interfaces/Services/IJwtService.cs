namespace TechLibrary.Domain.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId);
    }
}
