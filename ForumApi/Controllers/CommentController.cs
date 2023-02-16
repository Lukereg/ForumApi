﻿using ForumApi.Models.Comments;
using ForumApi.Services.CommentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumApi.Controllers
{
    [Route("v1/categories/{categoryId}/posts/{postId}/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<ActionResult> AddComment([FromRoute] int categoryId, [FromRoute] int postId, [FromBody] AddCommentDto addCommentDto)
        {
            var id = await _commentService.AddComment(categoryId, postId, addCommentDto);
            return Created($"/v1/categories/{categoryId}/posts/{postId}/comments/{id}", null);
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<ActionResult> GetComments([FromRoute] int categoryId, [FromRoute] int postId)
        {
            var comments = await _commentService.GetComments(categoryId, postId);
            return Ok(comments);
        }

        [AllowAnonymous]
        [HttpPut("{commentId}")]
        public async Task<ActionResult> UpdateComment([FromRoute] int categoryId, [FromRoute] int postId, int commentId, [FromBody] UpdateCommentDto updateCommentDto)
        {
            await _commentService.UpdateComment(categoryId, postId, commentId, updateCommentDto);
            return Ok();
        }
    }
}