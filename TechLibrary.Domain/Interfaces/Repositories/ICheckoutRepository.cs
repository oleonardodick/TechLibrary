namespace TechLibrary.Domain.Interfaces.Repositories
{
    public interface ICheckoutRepository
    {
        Task<int> GetAmountBooksNotReturnedAsync(Guid bookId);
        Task CreateBookCheckoutAsync(Guid bookId, Guid userId);
    }
}
