using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TechLibrary.Application.DTOs.Books.Request;
using TechLibrary.Application.DTOs.Books.Response;
using TechLibrary.Application.Interfaces.Books;

namespace TechLibrary.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IFilterBooksUseCase _filterBookUseCase;
        public BooksController(IFilterBooksUseCase filterBooksUseCase)
        {
            _filterBookUseCase = filterBooksUseCase;    
        }

        [HttpGet("Filter")]
        [ProducesResponseType(typeof(ResponseBooksDTO), StatusCodes.Status200OK, MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Filter(int pageNumber, string? title)
        {
            var result = await _filterBookUseCase.ExecuteAsync(new RequestFilterBooksDTO
            {
                PageNumber = pageNumber,
                Title = title
            });

            return Ok(result);

        }
    }
}
