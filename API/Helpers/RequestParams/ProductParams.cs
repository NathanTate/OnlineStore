namespace API.Helpers.RequestParams
{
    public class ProductParams
    {
        public int categoryId { get; set; } = 1;
        public List<int?> subCategoryIds { get; set; } = new();
        public long PriceMin { get; set; } = default;
        public long PriceMax { get; set;} = int.MaxValue;
        public int BrandId { get; set; } = default;
        public List<string> Colors { get; set; } = new();
    }
}
