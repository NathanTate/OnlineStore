using API.Models.DTO.UserDTO.Requests;
using FluentValidation;

namespace API.FluentValidators
{
    public class VerifyEmailRequestValidator : AbstractValidator<VerifyEmailRequest>
    {
        public VerifyEmailRequestValidator()
        {
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Email).EmailAddress().NotEmpty().MaximumLength(100);
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
