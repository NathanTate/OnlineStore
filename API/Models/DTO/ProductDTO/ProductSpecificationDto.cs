using API.Models.ProductModel;
using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO.ProductDTO
{
    public class ProductSpecificationDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
