using System.ComponentModel.DataAnnotations;

namespace API.Models.ProductModel
{
    public class ProductSubCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string SubCategoryName { get; set; }

        [Required]
        public string SubCategoryDescription { get; set; }

        [Required]
        [MaxLength(100)]
        public string Group { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; }
        public List<Product> Products { get; set; } = new();
    }
}
