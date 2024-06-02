using API.Models.DTO.Cart.CartRequests;
using FluentValidation;

namespace API.FluentValidators.CartValidators
{
    public class CartHeaderRequestValidator : AbstractValidator<CartHeaderRequest>
    {
        public CartHeaderRequestValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.Total).NotEmpty();
            RuleFor(c => c.CouponCode).NotEmpty().MaximumLength(50);
        }
    }
}
