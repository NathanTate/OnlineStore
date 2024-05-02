using API.Models.DTO.ProductDTO.Responses;

namespace API.Models.DTO.ProductDTO
{
    public class ProductSubCategoryDto
    {
        public int Id { get; set; }
        public string SubCategoryName { get; set; }
        public string Image { get; set; }
        public string SubCategoryDescription { get; set; }
        public int CategoryId { get; set; }
        public List<ProductResponse> Products { get; set; }
    }
}
