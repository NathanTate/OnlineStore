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
        public int ProductItemId { get; set; }
        public ProductItem ProductItem { get; set; }
        public int Count { get; set; }
    }
}
