using FluentValidation;
using FluentValidation.Results;
using TechLibrary.Application.DTOs.Users.Request;
using TechLibrary.Application.DTOs.Users.Response;
using TechLibrary.Application.Interfaces.Users;
using TechLibrary.Domain.Entities;
using TechLibrary.Domain.Interfaces.Repositories;
using TechLibrary.Domain.Interfaces.Services;
using TechLibrary.Exception;

namespace TechLibrary.Application.UseCases.Users
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IJwtService _jwtService;
        private readonly IValidator<RequestRegisterUserDTO> _validator;

        public RegisterUserUseCase(IUserRepository userRepository, IEncryptionService encryptionService, IJwtService jwtService, IValidator<RequestRegisterUserDTO> validator)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _jwtService = jwtService;
            _validator = validator;
        }

        public async Task<ResponseRegisteredUserDTO> RegisterUser(RequestRegisterUserDTO request)
        {
            await ValidateUser(request);

            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
                Password = _encryptionService.Encrypt(request.Password)
            };

            await _userRepository.CreateUserAsync(user);

            return new ResponseRegisteredUserDTO
            {
                Name = request.Name,
                AccessToken = _jwtService.GenerateToken(user.Id)
            };
        }

        private async Task ValidateUser(RequestRegisterUserDTO request)
        {
            var result = await _validator.ValidateAsync(request);

            var existsUserWithEmail = await _userRepository.GetUserByEmailAsync(request.Email) != null;
            if (existsUserWithEmail) result.Errors.Add(new ValidationFailure("Email", "E-mail já cadastrado."));

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
