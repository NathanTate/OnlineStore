using API.Models.DTO.UserDTO.Requests;
using FluentValidation;

namespace API.FluentValidators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterDtoValidator() 
        {
            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Email).EmailAddress().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Password).NotEmpty().Length(6, 32);
        }
    }
}
