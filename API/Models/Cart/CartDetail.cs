using API.Models.ProductModel;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Cart
{
    public class CartDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CartHeaderId { get; set; }
        public CartHeader CartHeader { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
    }
}
