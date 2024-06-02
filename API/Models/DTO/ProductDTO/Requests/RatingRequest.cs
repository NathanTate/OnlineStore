namespace API.Models.DTO.ProductDTO.Requests
{
    public class RatingRequest
    {
        public int RatingScore { get; set; }
        public string OrderStatus { get; set; }
        public string UserId { get; set; }
    }
}
