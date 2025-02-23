using Microsoft.EntityFrameworkCore;
using TechLibrary.Domain.Entities;
using TechLibrary.Domain.Interfaces.Repositories;

namespace TechLibrary.Infrastructure.DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly TechLibraryDbContext _dbContext;
        public BookRepository(TechLibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(IEnumerable<Book>, int TotalCount)> FilterBooksAsync(string? title, int pageNumber, int pageSize)
        {
            var query = _dbContext.Books.AsQueryable();
            if (!string.IsNullOrEmpty(title))
            {
                string normalizedTitle = title.Trim().ToLower();
                query = query.Where(b => b.Title.Trim().ToLower().Contains(normalizedTitle));
            }

            var totalCount = await query.CountAsync();

            var books = await query
                .OrderBy(b => b.Title)
                .ThenBy(b => b.Author)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (books, totalCount);

        }

        public async Task<Book?> GetBookAsync(Guid bookId)
        {
            return await _dbContext.Books.FindAsync(bookId);
        }
    }
}
