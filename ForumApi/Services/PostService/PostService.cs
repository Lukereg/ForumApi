using AutoMapper;
using ForumApi.Entities;
using ForumApi.Models.Posts;

namespace ForumApi.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public PostService(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<int> AddPost(int categoryId, AddPostDto addPostDto)
        {
            var post = _mapper.Map<Post>(addPostDto);
            post.CategoryId = categoryId;

            await _forumDbContext.Posts.AddAsync(post);
            await _forumDbContext.SaveChangesAsync();

            return post.Id;
        }
    }
}
