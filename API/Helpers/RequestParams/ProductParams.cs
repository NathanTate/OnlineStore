using System.Text;
using API.Helpers.Pagination;

namespace API.Helpers.RequestParams
{
    public class ProductParams : PaginationParams
    {
        public int categoryId { get; set; } = 2;
        public List<int?> SubCategories { get; set; } = new();
        public decimal PriceMin { get; set; } = default;
        public decimal PriceMax { get; set; } = int.MaxValue;
        public int BrandId { get; set; } = default;
        public bool? InStock { get; set; } = null;
        public string SortBy { get; set; }
        public string SortColumn { get; set; }
        public string SearchTerm { get; set; }
        public List<string> Colors { get; set; } = new();

        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (var item in Colors)
            {
                sb.Append(item);
            }
            string colors = sb.ToString();
            sb.Clear();
            foreach (var item in SubCategories)
            {
                sb.Append(item);
            }
            string subCategories = sb.ToString();
            return $"{categoryId}-{subCategories}-{PriceMin}-{PriceMax}-{BrandId}-{InStock}-{SortBy}-{SortColumn}-{SearchTerm}-{colors}";
        }
    }
}

