namespace API.Models.DTO.ProductDTO.Requests
{
    public class ReviewRequest
    {
        public string Pros { get; set; }
        public string Cons { get; set; }
        public string Comment { get; set; }
        public int RatingScore { get; set; }        
        public string OrderStatus { get; set; }
        public int ProductId { get; set; }
    }
}
