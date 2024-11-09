using API.Models.DTO.Cart.CartRequests;
using FluentValidation;

namespace API.FluentValidators.CartValidators
{
    public class CartDetailRequestValidator : AbstractValidator<CartDetailRequest>
    {
        public CartDetailRequestValidator()
        {
            RuleFor(c => c.ProductId).NotEmpty();
            RuleFor(c => c.Count).NotNull();
            RuleFor(c => c.ColorId).NotEmpty();
        }
    }
}
