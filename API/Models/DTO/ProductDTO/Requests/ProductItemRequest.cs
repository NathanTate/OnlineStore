namespace API.Models.DTO.ProductDTO.Requests
{
    public class ProductItemRequest
    {
        public long OriginalPrice { get; set; }
        public long SalePrice { get; set; }
        public string ProductCode { get; set; }
        public string Color { get; set; }
    }
}
