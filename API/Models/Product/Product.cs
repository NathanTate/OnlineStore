using System.ComponentModel.DataAnnotations;

namespace API.Models.ProductModel
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(10, 50)]
        public int ProductRating { get; set; } = 0;

        public int TotalReviews { get; set; } = 0;

        [Required]
        public int SubCategoryId { get; set; }
        public ProductSubCategory SubCategory { get; set; }

        public List<ProductItem> ProductItems { get; set; } = new();
        public List<ProductImage> ProductImages { get; set; } = new();
        public List<ProductSpecification> ProductSpecifications { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();

        [Required]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

    }
}
