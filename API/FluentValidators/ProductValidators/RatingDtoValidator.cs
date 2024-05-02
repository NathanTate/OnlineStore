using API.Models.DTO.ProductDTO;
using FluentValidation;

namespace API.FluentValidators.ProductValidators
{
    public class RatingDtoValidator : AbstractValidator<RatingDto>
    {
        public RatingDtoValidator()
        {
            RuleFor(r => r.RatingScore).NotEmpty();
            RuleFor(r => r.UserId).NotEmpty();
        }
    }
}
