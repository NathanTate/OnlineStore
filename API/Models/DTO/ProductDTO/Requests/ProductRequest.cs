namespace API.Models.DTO.ProductDTO.Requests
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal SalePrice { get; set; } = 0m;
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public List<string> Colors { get; set; }
        public List<ProductSpecificationDto> ProductSpecifications { get; set; }
    }
}
