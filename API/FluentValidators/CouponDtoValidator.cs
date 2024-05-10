using API.Models.DTO.Coupon;
using FluentValidation;

namespace API.FluentValidators
{
    public class CouponDtoValidator: AbstractValidator<CouponDto>
    {
        public CouponDtoValidator()
        {
            RuleFor(c => c.CouponCode).NotEmpty().MaximumLength(50);
            RuleFor(c => c.DiscountAmount).NotEmpty();
            RuleFor(c => c.MinPrice).NotEmpty();
        }
    }
}
