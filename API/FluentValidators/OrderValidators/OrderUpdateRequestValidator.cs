using API.Models.DTO.Order.Requests;
using FluentValidation;
using static API.Utility.SD;

namespace API.FluentValidators.OrderValidators
{
    public class OrderUpdateRequestValidator : AbstractValidator<OrderUpdateRequest>
    {
        public OrderUpdateRequestValidator()
        {
            RuleFor(o => o.orderHeaderId).NotEmpty();
            RuleFor(o => o.OrderStatus).NotEmpty().IsInEnum();

        }
    }
}
