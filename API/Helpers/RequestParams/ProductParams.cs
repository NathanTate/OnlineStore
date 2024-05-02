namespace API.Helpers.RequestParams
{
    public class ProductParams
    {
        public int categoryId { get; set; } = 1;
        public int? subCategoryId { get; set; } = default;
        public long PriceStart { get; set; } = default;
        public long PriceEnd { get; set;} = int.MaxValue;
        public int BrandId { get; set; } = default;
        public string Color { get; set; } = default;
    }
}
