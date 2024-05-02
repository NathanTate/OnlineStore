using API.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace API.Models.DTO.ProductDTO
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Pros { get; set; }
        public string Cons { get; set; }
        public string Comment { get; set; }
        public Rating Rating { get; set; }
        public int ProductId { get; set; }
    }
}
