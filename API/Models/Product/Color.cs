using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.ProductModel
{
    [Table("Colors")]
    public class Color
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Value { get; set; }
        public IEnumerable<ProductColor> Colors { get; set; }
    }
}
