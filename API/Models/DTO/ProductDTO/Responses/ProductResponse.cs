namespace API.Models.DTO.ProductDTO.Responses
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }
        public string Description { get; set; }
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public double ProductRating { get; set; } = 0;
        public int TotalReviews { get; set; } = 0;
        public string MainImageUrl { get; set; }
        public BrandDto Brand { get; set; }
        public List<ColorResponse> Colors { get; set; }
        public List<ProductImageDto> ProductImages { get; set; } = new();
        public List<ProductSpecificationDto> ProductSpecifications { get; set; } = new();
        public List<ProductReviewResponse> Reviews { get; set; } = new();
    }
}
