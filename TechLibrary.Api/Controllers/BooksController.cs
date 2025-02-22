using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Books.Filter;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly FilterBookUseCase _filterBookUseCase;

        public BooksController(FilterBookUseCase filterBookUseCase)
        {
            _filterBookUseCase = filterBookUseCase;
        }
        [HttpGet("Filter")]
        [ProducesResponseType(typeof(ResponseBooksJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Filter(int pageNumber, string? title)
        {
            var result = _filterBookUseCase.Filter(new RequestFilterBooksJson
            {
                PageNumber = pageNumber,
                Title = title
            });

            return Ok(result);

        }
    }
}
