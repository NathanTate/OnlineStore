using API.Helpers.Pagination;

namespace API.Helpers.OrderParameters
{
    public class OrderParams : PaginationParams
    {
        public string SortBy { get; set; }
        public string SortColumn { get; set; }
        public string SearchTerm { get; set; }
        public string OrderStatus { get; set; }
    }
}
