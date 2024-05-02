namespace API.Models.DTO.ProductDTO
{
    public class ProductItemDto
    {
        public long OriginalPrice { get; set; }
        public long SalePrice { get; set; }
        public string ProductCode { get; set; }
        public string Color { get; set; }
    }
}
