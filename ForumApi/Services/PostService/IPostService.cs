using ForumApi.Entities;
using ForumApi.Models.Pagination;
using ForumApi.Models.Posts;
using ForumApi.Models.Queries;

namespace ForumApi.Services.PostService
{
    public interface IPostService
    {
        public Task<int> AddPost(int categoryId, AddPostDto addPostDto);
        public Task<GetPostDto> GetPostById(int categoryId, int postId);
        public Task<PagedResultDto<GetPostDto>> GetPosts(int categoryId, PaginationQuery paginationQuery);
        public Task<PagedResultDto<GetPostDto>> GetPostsByAuthor(int authorId, PaginationQuery paginationQuery);
        public Task<Post> GetPostEntityById (int categoryId, int postId);
        public Task DeletePost(int categoryId, int postId);
        public Task <PagedResultDto<GetPostDto>> GetPostsByTag(int tagId, PaginationQuery paginationQuery);
    }
}
