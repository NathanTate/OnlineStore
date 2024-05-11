namespace API.Models.DTO.Coupon
{
    public record CouponDto(string CouponCode, decimal DiscountAmount, decimal MinPrice);
}
