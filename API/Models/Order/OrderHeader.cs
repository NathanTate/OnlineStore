using API.Models.Coupon;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Order
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Precision(16, 2)]
        public decimal OrderTotal { get; set; }

        [MaxLength(50)]
        public string? CouponCode { get; set; }

        [Precision(16, 2)]
        public decimal Discount { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(15)]
        public string Phone { get; set; }

        [Required]
        public int AddressId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }
        public string StripeSessionId { get; set; }
        public string PaymentIntentId { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
