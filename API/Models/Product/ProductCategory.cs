using System.ComponentModel.DataAnnotations;

namespace API.Models.Product
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        [Required]
        public string CategoryDescription { get; set; }

        public List<ProductSubCategory> SubCategories { get; set; } = new();
    }
}
