using API.Models.DTO.Feedback;
using FluentValidation;

namespace API.FluentValidators 
{
    public class FeedbackDtoValidator : AbstractValidator<FeedbackDto>
    {
        public FeedbackDtoValidator() 
        {
           ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

          RuleFor(x => x.email).NotEmpty().EmailAddress().MaximumLength(100);
          RuleFor(x => x.name).NotEmpty().MaximumLength(100);
          RuleFor(x => x.phone).NotEmpty().MaximumLength(15);
          RuleFor(x => x.message).NotEmpty().MaximumLength(500);
        }
    }
}