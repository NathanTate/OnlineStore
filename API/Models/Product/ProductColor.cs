using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.ProductModel
{
    [Table("ProductColors")]
    public class ProductColor
    {
        public int ColorId { get; set; }
        public Color Color { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
