namespace API.Models.DTO.Cart.CartResponses
{
    public class CartHeaderResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public long Total { get; set; }
        public long Discount { get; set; }
        public string CouponCode { get; set; }
    }
}
