using API.Models.Order;
using API.Models.ProductModel;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Coupon
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [Required]
        public int OrderHeaderId { get; set; }
        public OrderHeader OrderHeader { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
