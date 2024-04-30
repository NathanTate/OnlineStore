using System.ComponentModel.DataAnnotations;

namespace API.Models.Product
{
    public class ProductItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public long OriginalPrice { get; set; }

        public long SalePrice { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProductCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string Color { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
