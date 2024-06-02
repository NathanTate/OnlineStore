using API.Models.DTO.UserDTO.Responses;

namespace API.Models.DTO.ProductDTO.Responses
{
    public class RatingResponse
    {
        public int Id { get; set; }
        public int RatingScore { get; set; }
        public string OrderStatus { get; set; }
        public MemberResponse member { get; set; }
    }
}
