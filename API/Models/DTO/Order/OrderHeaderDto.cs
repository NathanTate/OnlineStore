namespace API.Models.DTO.Order
{
    public class OrderHeaderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal OrderTotal { get; set; }
        public string CouponCode { get; set; }
        public decimal Discount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public int AddressId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public string StripeSessionId { get; set; }
        public string PaymentIntentId { get; set; }
        public IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }
}
