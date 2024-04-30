using System.ComponentModel.DataAnnotations;

namespace API.Models.Product
{
    public class ProductSubCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string SubCategoryName { get; set; }

        [Required]
        [MaxLength(500)]
        public string Image { get; set; }

        [Required]
        public string SubCategoryDescription { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; }
        public List<Product> Products { get; set; } = new();
    }
}
