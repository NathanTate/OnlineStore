using System.ComponentModel.DataAnnotations;

namespace API.Models.ProductModel
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Pros { get; set; }

        [Required]
        [MaxLength(500)]    
        public string Cons { get; set; }

        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        [Required]
        [Range(10, 50)]
        public int RatingScore { get; set; }

        [MaxLength(255)]
        public string OrderStatus { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
