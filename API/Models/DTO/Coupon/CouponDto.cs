namespace API.Models.DTO.Coupon
{
    public record CouponDto(string CouponCode, long DiscountAmount, long MinPrice);
}
