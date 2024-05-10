using System.ComponentModel.DataAnnotations;

namespace API.Models.Cart
{
    public class CartHeader
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public long Total { get; set; }
        public long Discount { get; set; }

        [MaxLength(50)]
        public string CouponCode { get; set; }
    }
}
