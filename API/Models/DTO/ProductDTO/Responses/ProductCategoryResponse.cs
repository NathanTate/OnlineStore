namespace API.Models.DTO.ProductDTO.Requests
{
    public class ProductCategoryResponse
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public List<ProductSubCategoryDto> SubCategoriesDto { get; set; }
    }
}
