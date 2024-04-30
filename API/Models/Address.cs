using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Country { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string State { get; set; }

        [Required]
        [MaxLength(200)]
        public string Street { get; set; }

        [Required]
        [MaxLength(100)]
        public string HouseNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string ZipCode { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
    }
}
