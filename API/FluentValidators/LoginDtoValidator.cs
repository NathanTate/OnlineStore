using API.Models.DTO.UserDTO.Requests;
using FluentValidation;

namespace API.FluentValidators
{
    public class LoginDtoValidator : AbstractValidator<LoginRequest>
    {
        public LoginDtoValidator()
        {
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Email).EmailAddress().NotEmpty().Length(11, 100);
            RuleFor(x => x.Password).NotEmpty().Length(6, 32);
        }
    }
}
