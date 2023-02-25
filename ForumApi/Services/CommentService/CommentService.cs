using AutoMapper;
using ForumApi.Entities;
using ForumApi.Exceptions;
using ForumApi.Models.Comments;
using ForumApi.Models.Pagination;
using ForumApi.Models.Posts;
using ForumApi.Models.Queries;
using ForumApi.Services.PaginationService;
using ForumApi.Services.PostService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq.Expressions;

namespace ForumApi.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;
        private readonly IPostService _postService;
        private readonly IPaginationService<Comment> _paginationService;

        public CommentService(ForumDbContext forumDbContext, IMapper mapper, IPostService postService, IPaginationService<Comment> paginationService)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
            _postService = postService; 
            _paginationService = paginationService;
        }

        public async Task<int> AddComment(int categoryId, int postId, AddCommentDto addCommentDto)
        {
            var post = await _postService.GetPostEntityById(categoryId, postId);

            var comment = _mapper.Map<Comment>(addCommentDto);
            comment.PostId = post.Id;

            await _forumDbContext.Comments.AddAsync(comment);
            await _forumDbContext.SaveChangesAsync();

            return comment.Id;
        }

        public async Task DeleteComment(int categoryId, int postId, int commentId)
        {
            var post = await _postService.GetPostEntityById(categoryId, postId);
            var comment = await _forumDbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment is null || comment.PostId != post.Id)
                throw new NotFoundException("Comment not found");
            
            _forumDbContext.Comments.Remove(comment);
            await _forumDbContext.SaveChangesAsync();
        }

        public async Task<PagedResultDto<GetCommentDto>> GetComments(int categoryId, int postId, PaginationQuery paginationQuery)
        {
            var post = await _postService.GetPostEntityById(categoryId, postId);

            var comments = _forumDbContext.Comments
                .Where(c => c.PostId == post.Id)
                .AsNoTracking();

            comments = Sort(comments, paginationQuery.SortBy, paginationQuery.SortDirection);

            var totalCount = comments.Count();
            var commentsList = await _paginationService.ItemsWithPagination(comments, paginationQuery);
            var result = _mapper.Map<List<GetCommentDto>>(commentsList);

            return new PagedResultDto<GetCommentDto>(result, totalCount, paginationQuery.PageSize, paginationQuery.PageNumber);
        }

        public async Task UpdateComment(int categoryId, int postId, int commentId, UpdateCommentDto updateCommentDto)
        {
            var post = await _postService.GetPostEntityById(categoryId, postId);
            var comment = await _forumDbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (comment is null || comment.PostId != post.Id)
                throw new NotFoundException("Comment not found");

            comment.Message = updateCommentDto.Message;
            comment.UpdatedDate = DateTime.Now;

            await _forumDbContext.SaveChangesAsync();
        }

        private IQueryable<Comment> Sort(IQueryable<Comment> comments, string? sortBy, SortDirection? sortDirection)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                comments.OrderByDescending(c => c.CreatedDate);
                return comments;
            }

            var columnsSelector = new Dictionary<string, Expression<Func<Comment, object>>>
                {
                    { nameof(Comment.CreatedDate), c => c.CreatedDate },
                };

            if (!columnsSelector.ContainsKey(sortBy))
                throw new BadRequestException("Invalid sorting rule");

            var selectedColumn = columnsSelector[sortBy];

            comments = sortDirection == SortDirection.ASC
                ? comments.OrderBy(selectedColumn)
                : comments.OrderByDescending(selectedColumn);

            return comments;
        }
    }
}
