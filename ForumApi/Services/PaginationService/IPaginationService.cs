using ForumApi.Entities;
using ForumApi.Models.Queries;

namespace ForumApi.Services.PaginationService
{
    public interface IPaginationService<T>
    {
        public Task<IEnumerable<T>> ItemsWithPagination(IQueryable<T> items, PaginationQuery paginationQuery);
    }
}
