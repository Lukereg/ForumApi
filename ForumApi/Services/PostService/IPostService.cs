using ForumApi.Models.Posts;

namespace ForumApi.Services.PostService
{
    public interface IPostService
    {
        public Task<int> AddPost(int categoryId, AddPostDto addPostDto);
    }
}
