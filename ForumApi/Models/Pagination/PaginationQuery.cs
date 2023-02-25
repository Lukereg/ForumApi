using ForumApi.Models.Pagination;

namespace ForumApi.Models.Queries
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SortBy { get; set; }
        public SortDirection? SortDirection { get; set; }
    }
}
