using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO.ProductDTO.Requests
{
    public class ProductRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public IFormFile ProductImage { get; set; }
        public int BrandId { get; set; }
        public List<ProductItemRequest> ProductItemsDto { get; set; }

        public List<SpecificationDto> SpecificationsDto { get; set; }
    }
}
