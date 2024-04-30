namespace API.Models.DTO.ProductDTO
{
    public class ProductSubCategoryDto
    {
        public int Id { get; set; }
        public string SubCategoryName { get; set; }
        public string Image { get; set; }
        public string SubCategoryDescription { get; set; }
        public int CategoryId { get; set; }
    }
}
