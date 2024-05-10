using API.Models.DTO.ProductDTO;

namespace API.Models.DTO.Cart.CartResponses
{
    public class CartDetailResponse
    {
        public int Id { get; set; }
        public int CartHeaderId { get; set; }
        public ProductItemDto ProductItem { get; set; }
        public int Count { get; set; }
    }
}
