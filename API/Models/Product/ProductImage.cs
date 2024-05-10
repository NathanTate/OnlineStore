using System.ComponentModel.DataAnnotations;

namespace API.Models.ProductModel
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public bool IsMain { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
