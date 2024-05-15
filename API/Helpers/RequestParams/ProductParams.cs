using API.Helpers.Pagination;
using Microsoft.EntityFrameworkCore;

namespace API.Helpers.RequestParams
{
    public class ProductParams : PaginationParams
    {
        public int categoryId { get; set; } = 1;
        public List<int?> subCategories { get; set; } = new();
        public decimal PriceMin { get; set; } = default;
        public decimal PriceMax { get; set; } = int.MaxValue;
        public int BrandId { get; set; } = default;
        public string SortBy { get; set; }
        public string SortColumn { get; set; }
        public List<string> Colors { get; set; } = new();
    }
}
