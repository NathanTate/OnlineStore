using System.ComponentModel.DataAnnotations;

namespace API.Models.Coupon
{
    public class Coupon
    {
        [Key]
        [MaxLength(50)]
        public string CouponCode { get; set; }
        public long DiscountAmount { get; set; }
        public long MinPrice { get; set; }
    }
}
