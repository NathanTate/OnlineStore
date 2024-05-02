using API.Models.DTO.ProductDTO;
using FluentValidation;

namespace API.FluentValidators.ProductValidators
{
    public class ReviewDtoValidator : AbstractValidator<ReviewDto>
    {
        public ReviewDtoValidator()
        {
            RuleFor(r => r.Pros).NotEmpty();
            RuleFor(r => r.Cons).NotEmpty();
            RuleFor(r => r.Rating).NotEmpty();
            RuleFor(r => r.ProductId).NotEmpty();
        }
    }
}
