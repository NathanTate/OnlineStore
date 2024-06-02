using API.Models.DTO.ProductDTO.Requests;
using FluentValidation;

namespace API.FluentValidators.ProductValidators
{
    public class RatingDtoValidator : AbstractValidator<RatingRequest>
    {
        public RatingDtoValidator()
        {
            RuleFor(r => r.RatingScore).NotEmpty();
            RuleFor(r => r.UserId).NotEmpty();
        }
    }
}
