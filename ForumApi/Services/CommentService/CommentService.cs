using AutoMapper;
using ForumApi.Entities;
using ForumApi.Exceptions;
using ForumApi.Models.Comments;
using ForumApi.Services.PostService;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public CommentService(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<int> AddComment(int categoryId, int postId, AddCommentDto addCommentDto)
        {
            var post = await _forumDbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null || post.CategoryId != categoryId)
                throw new NotFoundException("Post not found");

            var comment = _mapper.Map<Comment>(addCommentDto);
            comment.PostId = post.Id;

            await _forumDbContext.Comments.AddAsync(comment);
            await _forumDbContext.SaveChangesAsync();

            return comment.Id;
        }
    }
}
