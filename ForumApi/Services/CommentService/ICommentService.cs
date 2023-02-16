using ForumApi.Models.Comments;

namespace ForumApi.Services.CommentService
{
    public interface ICommentService
    {
        public Task<int> AddComment(int categoryId, int postId, AddCommentDto addCommentDto);
    }
}
