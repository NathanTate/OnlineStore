using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO.ProductDTO
{
    public class BrandDto
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string BrandDescription { get; set; }
    }
}
