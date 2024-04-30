using System.ComponentModel.DataAnnotations;

namespace API.Models.Product
{
    public class ProductSpecification
    {
        public int ProductId { get; set; }
        public Product Product{ get; set; }
        public int SpecificationId { get; set; }
        public Specification Specification { get; set; }

        [Required]
        [MaxLength(255)]
        public string Value { get; set; }

    }
}
