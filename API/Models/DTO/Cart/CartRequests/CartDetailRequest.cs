namespace API.Models.DTO.Cart.CartRequests
{
    public class CartDetailRequest
    {
        public int CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; } = 0;
        public bool CountUpdate { get; set; } = false;
    }
}
