using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Login.DoLogin;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status401Unauthorized)]
    public class LoginController : ControllerBase
    {
        private readonly DoLoginUseCase _doLoginUseCase;

        public LoginController(DoLoginUseCase doLoginUseCase)
        {
            _doLoginUseCase = doLoginUseCase;
        }

        [HttpPost]
        public IActionResult DoLogin(RequestLoginJson request)
        {
            var response = _doLoginUseCase.Execute(request);
            return Ok(response);
        }
    }
}
