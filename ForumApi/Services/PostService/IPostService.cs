using ForumApi.Entities;
using ForumApi.Models.Posts;

namespace ForumApi.Services.PostService
{
    public interface IPostService
    {
        public Task<int> AddPost(int categoryId, AddPostDto addPostDto);
        public Task<GetPostDto> GetPostById(int categoryId, int postId);
        public Task<IEnumerable<GetPostDto>> GetPosts(int categoryId);
        public Task<IEnumerable<GetPostDto>> GetPostsByAuthor(int authorId);
        public Task<Post> GetPostEntityById (int categoryId, int postId);
        public Task DeletePost(int categoryId, int postId);
    }
}
