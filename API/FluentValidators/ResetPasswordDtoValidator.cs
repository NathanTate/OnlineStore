using API.Models.DTO.UserDTO.Requests;
using FluentValidation;

namespace API.FluentValidators
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordDtoValidator()
        {
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Password).NotEmpty().Length(6, 32);
            RuleFor(x => x.ConfirmPassword).NotEmpty().Length(6, 32).Equal(x => x.Password);
        }
    }
}
