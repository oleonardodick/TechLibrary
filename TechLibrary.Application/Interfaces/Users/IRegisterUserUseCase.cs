using TechLibrary.Application.DTOs.Users.Request;
using TechLibrary.Application.DTOs.Users.Response;

namespace TechLibrary.Application.Interfaces.Users
{
    public interface IRegisterUserUseCase
    {
        /// <summary>
        /// Executes an asynchronous operation to create the user in the system, based on the provided filter criteria.
        /// </summary>
        /// <param name="request">The user data to create according to <see cref="RequestRegisterUserDTO"/>.</param>
        /// <returns>A task that represents the asynchronous operation containing a <see cref="ResponseRegisteredUserDTO"/> witch returns the name of the user and a token.</returns>
        Task<ResponseRegisteredUserDTO> RegisterUser(RequestRegisterUserDTO request);
    }
}
