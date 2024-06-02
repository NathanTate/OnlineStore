namespace API.Models.DTO.ProductDTO.Responses
{
    public class CategoriesResponse
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public IEnumerable<SubcategoryGroupResponse> SubcategoryGroups { get; set; }
    }
}
