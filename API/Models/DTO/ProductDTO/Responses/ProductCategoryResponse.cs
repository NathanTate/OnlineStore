using API.Models.DTO.ProductDTO.Responses;

namespace API.Models.DTO.ProductDTO.Requests
{
    public class ProductCategoryResponse
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public IEnumerable<ProductSubCategoryDto> SubCategoriesDto { get; set; }
    }
}
