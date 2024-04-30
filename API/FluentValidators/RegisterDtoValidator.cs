using API.Models.DTO.UserDTO.Requests;
using FluentValidation;

namespace API.FluentValidators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterDtoValidator() 
        {
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Email).EmailAddress().NotEmpty().Length(11, 100);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().Length(6, 32);
        }
    }
}
