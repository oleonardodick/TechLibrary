using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Checkouts;

namespace TechLibrary.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly BookCheckoutUseCase _bookCheckoutUseCase;

        public CheckoutsController(BookCheckoutUseCase bookCheckoutUseCase)
        {
            _bookCheckoutUseCase = bookCheckoutUseCase;
        }

        [HttpPost]
        [Route("{bookId}")]
        public IActionResult BookCheckout(Guid bookId)
        {
            _bookCheckoutUseCase.CheckoutBook(bookId);

            return NoContent();
        }
    }
}
