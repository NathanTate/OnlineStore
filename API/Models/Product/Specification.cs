using System.ComponentModel.DataAnnotations;

namespace API.Models.Product
{
    public class Specification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public List<ProductSpecification> ProductSpecifications { get; set; }
    }
}
