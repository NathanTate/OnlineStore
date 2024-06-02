using API.Models.DTO.Order.Requests;
using FluentValidation;

namespace API.FluentValidators.OrderValidators
{
    public class OrderCheckoutRequestValidator : AbstractValidator<OrderCheckoutRequest>
    {
        public OrderCheckoutRequestValidator()
        {
            RuleFor(o => o.CartResponse).NotEmpty();
            RuleFor(o => o.FirstName).NotNull().Length(2, 50);
            RuleFor(o => o.LastName).NotNull().Length(2, 50);
            RuleFor(o => o.Email).NotNull().Length(2, 100);
            RuleFor(o => o.FirstName).NotNull().Length(2, 15);
            RuleFor(o => o.Address).NotEmpty();
        }
    }
}
