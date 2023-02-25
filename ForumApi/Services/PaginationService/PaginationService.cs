using ForumApi.Entities;
using ForumApi.Exceptions;
using ForumApi.Models.Queries;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.Services.PaginationService
{
    public class PaginationService<T>: IPaginationService<T>
    {
        public PaginationService() 
        {
        }

        public async Task<IEnumerable<T>> ItemsWithPagination(IQueryable<T> items, PaginationQuery paginationQuery)
        {
            if (paginationQuery.PageNumber <= 0 || paginationQuery.PageSize <= 0)
                throw new BadRequestException("Values needed for pagination cannot be non-positive");

            var itemsList = await items
                .Skip(paginationQuery.PageSize * (paginationQuery.PageNumber - 1))
                .Take(paginationQuery.PageSize)
                .ToListAsync();

            return itemsList;
        }
    }
}
