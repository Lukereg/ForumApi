using ForumApi.Models.Categories;
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
    }
}
