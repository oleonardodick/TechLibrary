using TechLibrary.Application.DTOs.Login.Request;
using TechLibrary.Application.DTOs.Users.Response;
using TechLibrary.Application.Interfaces.Login;
using TechLibrary.Domain.Exceptions;
using TechLibrary.Domain.Interfaces.Repositories;
using TechLibrary.Domain.Interfaces.Services;

namespace TechLibrary.Application.UseCases.Login
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IEncryptionService _encryptionService;

        public DoLoginUseCase(IUserRepository userRepository, IJwtService jwtService, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _encryptionService = encryptionService;
        }
        public async Task<ResponseRegisteredUserDTO> ExecuteLoginAsync(RequestLoginDTO request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user is null)
                throw new InvalidLoginException();

            var passwordIsValid = _encryptionService.Decrypt(request.Password, user.Password);
            if (passwordIsValid == false) throw new InvalidLoginException();

            return new ResponseRegisteredUserDTO
            {
                Name = user.Name,
                AccessToken = _jwtService.GenerateToken(user.Id),
            };
        }
    }
}
