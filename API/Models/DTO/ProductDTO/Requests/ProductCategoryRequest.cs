namespace API.Models.DTO.ProductDTO.Requests
{
    public class ProductCategoryRequest
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
    }
}
