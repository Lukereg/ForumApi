﻿using AutoMapper;
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
        private readonly IPostService _postService;

        public CommentService(ForumDbContext forumDbContext, IMapper mapper, IPostService postService)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
            _postService = postService; 
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

        public async Task<IEnumerable<GetCommentDto>> GetComments(int categoryId, int postId)
        {
            var post = await _postService.GetPostEntityById(categoryId, postId);
            var comments = await _forumDbContext.Comments.Where(c => c.PostId == post.Id).ToListAsync();
            var result = _mapper.Map<List<GetCommentDto>>(comments);

            return result;
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
    }
}
