using TechLibrary.Application.DTOs.Login.Request;
using TechLibrary.Application.DTOs.Users.Response;

namespace TechLibrary.Application.Interfaces.Login
{
    public interface IDoLoginUseCase
    {
        Task<ResponseRegisteredUserDTO> ExecuteLoginAsync(RequestLoginDTO request);
    }
}
