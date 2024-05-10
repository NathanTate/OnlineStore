using System.ComponentModel.DataAnnotations;

namespace API.Models.ProductModel
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string BrandName { get; set; }

        [Required]  
        public string BrandDescription { get; set; }
        public List<Product> Products { get; set; }
    }
}
