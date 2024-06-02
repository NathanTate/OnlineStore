namespace API.Models.DTO.ProductDTO.Responses
{
    public class ReviewResponse
    {
        public int Id { get; set; }
        public string Pros { get; set; }
        public string Cons { get; set; }
        public string Comment { get; set; }
        public RatingResponse Rating { get; set; }
        public int ProductId { get; set; }
    }
}
