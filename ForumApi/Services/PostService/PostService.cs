using AutoMapper;
using ForumApi.Entities;
using ForumApi.Exceptions;
using ForumApi.Models.Pagination;
using ForumApi.Models.Posts;
using ForumApi.Models.Queries;
using ForumApi.Services.PaginationService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ForumApi.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IForumDbContext _forumDbContext;
        private readonly IMapper _mapper;
        private readonly IPaginationService<Post> _paginationService;

        public PostService(IForumDbContext forumDbContext, IMapper mapper, IPaginationService<Post> paginationService)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
            _paginationService = paginationService; 
        }

        public async Task<int> AddPost(int categoryId, AddPostDto addPostDto)
        {
            var category = await _forumDbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category is null)
                throw new NotFoundException("Category not found");

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

        public async Task<PagedResultDto<GetPostDto>> GetPosts(int categoryId, PaginationQuery paginationQuery)
        {
            var category = await _forumDbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category is null)
                throw new NotFoundException("Category not found");

            var posts = _forumDbContext.Posts
                .Where(post => post.CategoryId == categoryId)
                .AsNoTracking();

            return await SortAndPaginate(posts, paginationQuery);
        }

        public async Task<PagedResultDto<GetPostDto>> GetPostsByAuthor(int authorId, PaginationQuery paginationQuery)
        {
            var posts = _forumDbContext.Posts
                .Where(p => p.AuthorId == authorId)
                .AsNoTracking();

            return await SortAndPaginate(posts, paginationQuery);
        }

        public async Task DeletePost(int categoryId, int postId)
        {
            var post = await GetPostEntityById(categoryId, postId);
            _forumDbContext.Posts.Remove(post);
            await _forumDbContext.SaveChangesAsync();
        }

        public async Task<Post> GetPostEntityById(int categoryId, int postId)
        {
            var category = await _forumDbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category is null)
                throw new NotFoundException("Category not found");

            var post = await _forumDbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null || post.CategoryId != categoryId)
                throw new NotFoundException("Post not found");

            return post;
        }

        public async Task<PagedResultDto<GetPostDto>> GetPostsByTag(int tagId, PaginationQuery paginationQuery)
        {
            var tag = await _forumDbContext.Tags
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == tagId);

            if (tag is null)
                throw new NotFoundException("Tag not found");

            var posts = _forumDbContext.Posts.Where(p => p.Tags.Contains(tag))
                .AsNoTracking();

            return await SortAndPaginate(posts, paginationQuery);
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

        private async Task<PagedResultDto<GetPostDto>> SortAndPaginate(IQueryable<Post> posts, PaginationQuery paginationQuery)
        {
            posts = Sort(posts, paginationQuery.SortBy, paginationQuery.SortDirection);

            var totalCount = posts.Count();
            var postsList = await _paginationService.ItemsWithPagination(posts, paginationQuery);
            var result = _mapper.Map<List<GetPostDto>>(postsList);

            return new PagedResultDto<GetPostDto>(result, totalCount, paginationQuery.PageSize, paginationQuery.PageNumber);
        }

        private IQueryable<Post> Sort(IQueryable<Post> posts, string? sortBy, SortDirection? sortDirection)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                posts.OrderByDescending(p => p.CreatedDate);
                return posts;
            }
                
            var columnsSelector = new Dictionary<string, Expression<Func<Post, object>>>
                {
                    { nameof(Post.CreatedDate), p => p.CreatedDate },
                    { nameof(Post.Title), p => p.Title },
                };

            if (!columnsSelector.ContainsKey(sortBy))
                throw new BadRequestException("Invalid sorting rule");

            var selectedColumn = columnsSelector[sortBy];

            posts = sortDirection == SortDirection.ASC
                ? posts.OrderBy(selectedColumn)
                : posts.OrderByDescending(selectedColumn);

            return posts;
        }
        
    }
}
