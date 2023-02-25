namespace ForumApi.Models.Pagination
{
    public class PagedResultDto<T>
    {
        public IEnumerable<T>? Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }

        public PagedResultDto(IEnumerable<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ItemsFrom = (pageSize * (pageNumber - 1)) + 1;
            ItemsTo = ItemsFrom + pageSize - 1 <= totalCount
                ? ItemsFrom + pageSize - 1
                : totalCount;
            TotalItemsCount = totalCount;
        }
    }
}
