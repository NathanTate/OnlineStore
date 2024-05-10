using System.ComponentModel.DataAnnotations;

namespace API.Models.ProductModel
{
    public class ProductSpecification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Value { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
