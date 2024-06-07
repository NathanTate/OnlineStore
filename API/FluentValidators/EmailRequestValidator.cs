using API.Models.DTO.UserDTO.Requests;
using FluentValidation;

namespace API.FluentValidators
{
    public class EmailRequestValidator : AbstractValidator<EmailRequest>
    {
        public EmailRequestValidator()
        {
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Email).EmailAddress().NotEmpty().MaximumLength(100);
        }
    }
}
