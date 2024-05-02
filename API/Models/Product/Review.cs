using System.ComponentModel.DataAnnotations;

namespace API.Models.Product
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

        [Required]
        public int RatingId { get; set; }
        public Rating Rating { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
