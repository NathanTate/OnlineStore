namespace API.Models.DTO.Cart.CartRequests
{
    public class CartDetailRequest
    {
        public int CartHeaderId { get; set; }
        public int ProductItemId { get; set; }
        public int Count { get; set; } = 0;
    }
}
