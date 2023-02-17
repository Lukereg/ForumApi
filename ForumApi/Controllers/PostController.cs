﻿using ForumApi.Models.Categories;
using ForumApi.Models.Posts;
using ForumApi.Services.CategoryService;
using ForumApi.Services.PostService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumApi.Controllers
{
    [Route("v1/categories/{categoryId}/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<ActionResult> AddPost([FromRoute] int categoryId, [FromBody] AddPostDto addPostDto)
        {
            var id = await _postService.AddPost(categoryId, addPostDto);
            return Created($"/v1/categories/{categoryId}/posts/{id}", null);
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<ActionResult> GetPosts([FromRoute] int categoryId)
        {
            var posts = await _postService.GetPosts(categoryId);
            return Ok(posts);
        }

        [AllowAnonymous]
        [HttpGet("{postId}")]
        public async Task<ActionResult> GetPostById([FromRoute] int categoryId, [FromRoute] int postId)
        {
            var post = await _postService.GetPostById(categoryId, postId);
            return Ok(post);
        }

        [AllowAnonymous]
        [HttpDelete("{postId}")]
        public async Task<ActionResult> DeletePost([FromRoute] int categoryId, [FromRoute] int postId)
        {
            await _postService.DeletePost(categoryId, postId);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("~/v1/authors/{authorId}/posts")]
        public async Task<ActionResult> GetPostsByAuthor([FromRoute] int authorId)
        {
            var posts = await _postService.GetPostsByAuthor(authorId);
            return Ok(posts);
        }

        

    }
}
