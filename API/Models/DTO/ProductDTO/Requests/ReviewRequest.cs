namespace API.Models.DTO.ProductDTO.Requests
{
    public class ReviewRequest
    {
        public string Pros { get; set; }
        public string Cons { get; set; }
        public string Comment { get; set; }
        public RatingRequest Rating { get; set; }
        public int ProductId { get; set; }
    }
}
