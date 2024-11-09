namespace API.Models.DTO.ProductDTO.Requests;

public class PhotoUpdateRequest
{
    public int ItemId { get; set; }
    public IFormFileCollection PhotoCollection { get; set; }
    public List<int> IdsToRemove { get; set; }
}