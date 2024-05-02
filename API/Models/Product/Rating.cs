using System.ComponentModel.DataAnnotations;

namespace API.Models.Product
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(10, 50)]
        public int RatingScore { get; set; }

        [MaxLength(255)]
        public string OrderStatus { get; set; }

        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
