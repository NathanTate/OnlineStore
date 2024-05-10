namespace API.Models.DTO.Cart.CartResponses
{
    public class CartResponse
    {
        public CartHeaderResponse CartHeader { get; set; }
        public IEnumerable<CartDetailResponse> CartDetails { get; set; }
    }
}
