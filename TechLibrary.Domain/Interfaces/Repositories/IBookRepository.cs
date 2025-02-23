using TechLibrary.Domain.Entities;

namespace TechLibrary.Domain.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<(IEnumerable<Book>, int TotalCount)> FilterBooksAsync(string? title, int pageNumber, int pageSize);
        Task<Book?> GetBookAsync(Guid bookId);
    }
}
