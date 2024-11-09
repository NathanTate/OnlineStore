using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Responses;

namespace API.Models.DTO.Cart.CartResponses
{
    public class CartDetailResponse
    {
        public int Id { get; set; }
        public int CartHeaderId { get; set; }
        public ColorResponse Color { get; set; }
        public ProductResponse Product { get; set; }
        public int Count { get; set; }
    }
}
