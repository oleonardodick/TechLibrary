using Microsoft.AspNetCore.Mvc;
using TechLibrary.Application.DTOs.Error.Response;
using TechLibrary.Application.DTOs.Login.Request;
using TechLibrary.Application.DTOs.Users.Response;
using TechLibrary.Application.Interfaces.Login;
namespace TechLibrary.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IDoLoginUseCase _doLoginUseCase;

        public LoginController(IDoLoginUseCase doLoginUseCase)
        {
            _doLoginUseCase = doLoginUseCase;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorMessagesDTO), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DoLogin(RequestLoginDTO request)
        {
            var response = await _doLoginUseCase.ExecuteLoginAsync(request);
            return Ok(response);
        }
    }
}
