namespace API.Models.DTO.Cart.CartRequests
{
    public class CartHeaderRequest
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public string CouponCode { get; set; }
    }
}
