using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Models.ProductModel
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; }

        [Precision(16, 2)]
        public decimal OriginalPrice { get; set; }

        [Precision(16, 2)]
        public decimal SalePrice { get; set; }

        public int Quantity { get; set; }

        public List<ProductColor> Colors { get; set; }

        public string Description { get; set; }

        [Range(10, 50)]
        public double ProductRating { get; set; } = 0;

        public int TotalReviews { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public bool IsPlaceholder { get; set; } = false;

        public int SubCategoryId { get; set; }
        public ProductSubCategory SubCategory { get; set; }

        [MaxLength(250)]
        public string MainImageUrl { get; set; }

        public List<ProductImage> ProductImages { get; set; } = new();
        public List<ProductSpecification> ProductSpecifications { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

    }
}
