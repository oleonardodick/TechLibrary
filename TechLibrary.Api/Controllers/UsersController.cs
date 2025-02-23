using Microsoft.AspNetCore.Mvc;
using TechLibrary.Application.DTOs.Error.Response;
using TechLibrary.Application.DTOs.Users.Request;
using TechLibrary.Application.DTOs.Users.Response;
using TechLibrary.Application.Interfaces.Users;

namespace TechLibrary.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRegisterUserUseCase _registerUserUseCase;

        public UsersController(IRegisterUserUseCase registerUserUseCase)
        {
            _registerUserUseCase = registerUserUseCase;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorMessagesDTO), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RequestRegisterUserDTO request)
        {
            var response = await _registerUserUseCase.RegisterUser(request);
            return Created(string.Empty, response);
        }
    }
}
