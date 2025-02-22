using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Infrastructure.DataAccess;

namespace TechLibrary.Api.UseCases.Books.Filter
{
    public class FilterBookUseCase
    {
        private readonly TechLibraryDbContext _dbContext;
        private const int PAGE_SIZE = 10;

        public FilterBookUseCase(TechLibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ResponseBooksJson Filter(RequestFilterBooksJson request)
        {
            var query = _dbContext.Books.AsQueryable();
            if (string.IsNullOrWhiteSpace(request.Title) == false) 
            {
                query = query.Where(b => b.Title.Trim().ToLower().Contains(request.Title.Trim().ToLower()));
            };

            var books = query
                .OrderBy(b => b.Title).ThenBy(b => b.Author)
                .Skip((request.PageNumber - 1) * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToList();

            var totalBooks = 0;
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                totalBooks = _dbContext.Books.Count();
            } else
            {
                totalBooks = _dbContext.Books.Count(b => b.Title.Trim().ToLower().Contains(request.Title.Trim().ToLower()));
            }

                return new ResponseBooksJson
                {
                    Pagination = new ResponsePaginationJson
                    {
                        PageNumber = request.PageNumber,
                        TotalPages = (int)Math.Ceiling(totalBooks / (double)PAGE_SIZE),
                        TotalCount = totalBooks
                    },
                    Books = books.Select(b => new ResponseBookJson
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = b.Author,
                    }).ToList()
                };
        }
    } 
}
