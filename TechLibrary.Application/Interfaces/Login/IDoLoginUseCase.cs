using TechLibrary.Application.DTOs.Login.Request;
using TechLibrary.Application.DTOs.Users.Response;
using TechLibrary.Domain.Exceptions;

namespace TechLibrary.Application.Interfaces.Login
{
    public interface IDoLoginUseCase
    {
        /// <summary>
        /// Executes an asynchronous operation to login the user in the system, based on the provided filter criteria.
        /// </summary>
        /// <param name="request">The filter criteria to execute the login. This <see cref="RequestLoginDTO"/> should have the email and password of the user.</param>
        /// <returns>A task that represents the asynchronous operation containing a <see cref="ResponseRegisteredUserDTO"/> witch returns the name of the user and a token. When the e-mail or password is invalid, throws an error.</returns>
        /// <exception cref="InvalidLoginException"/>
        Task<ResponseRegisteredUserDTO> ExecuteLoginAsync(RequestLoginDTO request);
    }
}
