using API.Helpers.Pagination;

namespace API.Helpers.OrderParameters
{
    public class ReviewParams : PaginationParams
    {
      public int Id { get; set; } = 1;
      public string SortBy { get; set; }
      public string SortColumn { get; set; }
      public int Stars { get; set; } = 0;
    }
}
