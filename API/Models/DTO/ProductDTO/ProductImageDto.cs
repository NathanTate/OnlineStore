using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO.ProductDTO
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }

    }
}
