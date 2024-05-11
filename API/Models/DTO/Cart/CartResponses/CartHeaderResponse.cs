namespace API.Models.DTO.Cart.CartResponses
{
    public class CartHeaderResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public string CouponCode { get; set; }
    }
}
