using AutoMapper;
using ForumApi.Authorization;
using ForumApi.Entities;
using ForumApi.Exceptions;
using ForumApi.Models.Comments;
using ForumApi.Models.Pagination;
using ForumApi.Models.Posts;
using ForumApi.Models.Queries;
using ForumApi.Services.PaginationService;
using ForumApi.Services.PostService;
using ForumApi.Services.UserContextService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq.Expressions;

namespace ForumApi.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly IForumDbContext _forumDbContext;
        private readonly IMapper _mapper;
        private readonly IPostService _postService;
        private readonly IPaginationService<Comment> _paginationService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public CommentService(IForumDbContext forumDbContext, IMapper mapper, IPostService postService, 
            IPaginationService<Comment> paginationService, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
            _postService = postService; 
            _paginationService = paginationService;
            _authorizationService = authorizationService;
            _userContextService = userContextService;   
        }

        public async Task<int> AddComment(int categoryId, int postId, AddCommentDto addCommentDto)
        {
            var post = await _postService.GetPostEntityById(categoryId, postId);

            var comment = _mapper.Map<Comment>(addCommentDto);
            comment.PostId = post.Id;
            comment.AuthorId = _userContextService.GetUserId();

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

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.GetUser(), comment,
                new ResourceOperationRequirement(ResourceOperation.Delete));

            if (!authorizationResult.Succeeded)
                throw new ForbiddenException();

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

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.GetUser(), comment,
                new ResourceOperationRequirement(ResourceOperation.Update));

            if (!authorizationResult.Succeeded)
                throw new ForbiddenException();

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
