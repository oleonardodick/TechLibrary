using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechLibrary.Application.DTOs.Error.Response;
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorMessagesDTO), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> BookCheckout(Guid bookId)
        {
            await _bookCheckoutUseCase.CheckoutBook(bookId);

            return NoContent();
        }
    }
}
