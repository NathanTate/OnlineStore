using API.Models.DTO.UserDTO.Responses;

namespace API.Models.DTO.ProductDTO.Responses
{
    public class ReviewResponse
    {
        public int Id { get; set; }
        public string Pros { get; set; }
        public string Cons { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RatingScore { get; set; }
        public string OrderStatus { get; set; }
        public MemberResponse member { get; set; }
        public int ProductId { get; set; }
    }
}
