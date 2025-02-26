using TechLibrary.Application.DTOs.Books.Request;
using TechLibrary.Application.DTOs.Books.Response;

namespace TechLibrary.Application.Interfaces.Books
{
    public interface IFilterBooksUseCase
    {
        /// <summary>
        /// Executes an asynchronous operation to retrieve a list of books based on the provided filter criteria.
        /// </summary>
        /// <param name="request">The filter criteria for retrieving the books, which includes the page number and an optional title filter.</param>
        /// <returns>A task that represents the asynchronous operation, containing a <see cref="ResponseBooksDTO"/> with the paginated list of books and their details.</returns>
        Task<ResponseBooksDTO> ExecuteAsync(RequestFilterBooksDTO request);
    }
}
