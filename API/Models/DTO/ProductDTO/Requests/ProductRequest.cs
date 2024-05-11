
namespace API.Models.DTO.ProductDTO.Requests
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SalePrice { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public IFormFile ProductImage { get; set; }
        public bool IsMainImage { get; set; } = false;
        public int BrandId { get; set; }
        public List<ProductSpecificationDto> ProductSpecificationsDto { get; set; }
    }
}
