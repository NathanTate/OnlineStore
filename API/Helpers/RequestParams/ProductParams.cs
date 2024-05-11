using Microsoft.EntityFrameworkCore;

namespace API.Helpers.RequestParams
{
    public class ProductParams
    {
        public int categoryId { get; set; } = 1;
        public List<int?> subCategories { get; set; } = new();

        [Precision(16, 2)]
        public decimal PriceMin { get; set; } = default;

        [Precision(16, 2)]
        public decimal PriceMax { get; set; } = int.MaxValue;
        public int BrandId { get; set; } = default;
        public List<string> Colors { get; set; } = new();
    }
}
