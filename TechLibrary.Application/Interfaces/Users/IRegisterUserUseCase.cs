using TechLibrary.Application.DTOs.Users.Request;
using TechLibrary.Application.DTOs.Users.Response;

namespace TechLibrary.Application.Interfaces.Users
{
    public interface IRegisterUserUseCase
    {
        Task<ResponseRegisteredUserDTO> RegisterUser(RequestRegisterUserDTO request);
    }
}
