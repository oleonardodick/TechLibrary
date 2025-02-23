using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Application.Interfaces.Checkout;

namespace TechLibrary.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly IBookCheckoutUseCase _bookCheckoutUseCase;

        public CheckoutsController(IBookCheckoutUseCase bookCheckoutUseCase)
        {
            _bookCheckoutUseCase = bookCheckoutUseCase;
        }

        [HttpPost]
        [Route("{bookId}")]
        public async Task<IActionResult> BookCheckout(Guid bookId)
        {
            await _bookCheckoutUseCase.CheckoutBook(bookId);

            return NoContent();
        }
    }
}
