namespace API.Helpers.Pagination
{
    public class PaginationParams
    {
        private readonly int _maxPageSize = 20;

        private int _pageSize = 20;
        public int Page { get; set; } = 1;
        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
        }


    }
}
