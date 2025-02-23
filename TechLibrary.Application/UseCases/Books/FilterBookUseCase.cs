using TechLibrary.Application.DTOs.Books.Request;
using TechLibrary.Application.DTOs.Books.Response;
using TechLibrary.Application.DTOs.Pagination;
using TechLibrary.Application.Interfaces.Books;
using TechLibrary.Domain.Interfaces.Repositories;

namespace TechLibrary.Application.UseCases.Books
{
    public class FilterBookUseCase : IFilterBooksUseCase
    {
        private readonly IBookRepository _bookRepository;
        private const int PAGE_SIZE = 10;

        public FilterBookUseCase(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<ResponseBooksDTO> ExecuteAsync(RequestFilterBooksDTO request)
        {
            var (books, totalCount) = await _bookRepository.FilterBooksAsync(request.Title, request.PageNumber, PAGE_SIZE);

            return new ResponseBooksDTO
            {
                Pagination = new PaginationDTO
                {
                    PageNumber = request.PageNumber,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)PAGE_SIZE)
                },
                Books = books.Select(book => new ResponseBookDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                }).ToList()
            };   
        }
    }
}
