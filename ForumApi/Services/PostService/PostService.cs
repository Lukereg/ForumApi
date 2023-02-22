using AutoMapper;
using ForumApi.Entities;
using ForumApi.Exceptions;
using ForumApi.Models.Posts;
using Microsoft.EntityFrameworkCore;

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
            post.Tags = await MatchTags(addPostDto);

            await _forumDbContext.Posts.AddAsync(post);
            await _forumDbContext.SaveChangesAsync();

            return post.Id;
        }

        public async Task<GetPostDto> GetPostById(int categoryId, int postId)
        {
            var post = await GetPostEntityById(categoryId, postId);
            var result = _mapper.Map<GetPostDto>(post);
            return result;
        }

        public async Task<IEnumerable<GetPostDto>> GetPosts(int categoryId)
        {
            var posts = await _forumDbContext.Posts.Where(p => p.CategoryId == categoryId)
                .AsNoTracking()
                .ToListAsync();
            var result = _mapper.Map<List<GetPostDto>>(posts);
            return result;
        }

        public async Task<IEnumerable<GetPostDto>> GetPostsByAuthor(int authorId)
        {
            var posts = await _forumDbContext.Posts.Where(p => p.AuthorId == authorId)
                .AsNoTracking()
                .ToListAsync();
            var result = _mapper.Map<List<GetPostDto>>(posts);
            return result;
        }

        public async Task DeletePost(int categoryId, int postId)
        {
            var post = await GetPostEntityById(categoryId, postId);
            _forumDbContext.Posts.Remove(post);
            await _forumDbContext.SaveChangesAsync();
        }

        public async Task<Post> GetPostEntityById(int categoryId, int postId)
        {
            var post = await _forumDbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null || post.CategoryId != categoryId)
                throw new NotFoundException("Post not found");

            return post;
        }

        private async Task<List<Tag>?> MatchTags(AddPostDto addPostDto)
        {
            if (addPostDto.TagsIds is null)
                return null;

            var tags = new List<Tag>();

            foreach (var id in addPostDto.TagsIds)
            {
                var tag = await _forumDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
                if (tag is not null)
                    tags.Add(tag);
            }

            return tags;
        }
    }
}
