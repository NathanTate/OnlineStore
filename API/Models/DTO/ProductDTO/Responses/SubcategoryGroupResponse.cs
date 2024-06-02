namespace API.Models.DTO.ProductDTO.Responses
{
    public class SubcategoryGroupResponse
    {
        public string GroupName { get; set; }
        public IEnumerable<ProductSubCategoryDto> Subcategories { get; set; }
    }
}
