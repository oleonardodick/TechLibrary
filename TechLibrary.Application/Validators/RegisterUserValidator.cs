using FluentValidation;
using TechLibrary.Application.DTOs.Users.Request;

namespace TechLibrary.Application.Validators
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserDTO>
    {
        public RegisterUserValidator()
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage("O nome é obrigatório.");
            RuleFor(request => request.Email).EmailAddress().WithMessage("O e-mail não é válido.");
            RuleFor(request => request.Password).NotEmpty().WithMessage("A senha é obrigatória.");
            When(request => string.IsNullOrEmpty(request.Password) == false, () =>
            {
                RuleFor(request => request.Password.Length)
                    .GreaterThanOrEqualTo(6)
                    .WithMessage("A senha deve possuir no mínimo 6 caracteres.");
            });
        }
    }
}
