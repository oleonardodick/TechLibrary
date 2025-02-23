using TechLibrary.Domain.Entities;

namespace TechLibrary.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
