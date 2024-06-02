using API.Models.DTO.ProductDTO.Requests;
using FluentValidation;

namespace API.FluentValidators.ProductValidators
{
    public class ReviewDtoValidator : AbstractValidator<ReviewRequest>
    {
        public ReviewDtoValidator()
        {
            RuleFor(r => r.Pros).NotEmpty().MaximumLength(500);
            RuleFor(r => r.Cons).NotEmpty().MaximumLength(500);
            RuleFor(r => r.Rating).NotEmpty();
            RuleFor(r => r.ProductId).NotEmpty();
        }
    }
}
