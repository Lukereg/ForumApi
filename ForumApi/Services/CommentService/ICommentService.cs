using ForumApi.Models.Comments;
using ForumApi.Models.Pagination;
using ForumApi.Models.Queries;

namespace ForumApi.Services.CommentService
{
    public interface ICommentService
    {
        public Task<int> AddComment(int categoryId, int postId, AddCommentDto addCommentDto);
        public Task DeleteComment(int categoryId, int postId, int commentId);
        public Task<PagedResultDto<GetCommentDto>> GetComments(int categoryId, int postId, PaginationQuery paginationQuery);
        public Task UpdateComment(int categoryId, int postId, int commentId, UpdateCommentDto updateCommentDto);
    }
}
