using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Coupon
{
    public class Coupon
    {
        [Key]
        [MaxLength(50)]
        public string CouponCode { get; set; }

        [Precision(16, 2)]
        public decimal DiscountAmount { get; set; }

        [Precision(16, 2)]
        public decimal MinPrice { get; set; }
    }
}
