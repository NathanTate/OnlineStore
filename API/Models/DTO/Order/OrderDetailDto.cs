using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Responses;

namespace API.Models.DTO.Order
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public ColorResponse Color { get; set; }
        public string PhotoUrl { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
