using ForumApi.Models.Tags;
using ForumApi.Services.TagService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumApi.Controllers
{
    [Route("v1/tags")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<ActionResult> AddTag([FromBody] AddTagDto addTagDto)
        {
            var id = await _tagService.AddTag(addTagDto);
            return Created($"/v1/tags/{id}", null);
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<ActionResult> GetAllTags()
        {
            var tags = await _tagService.GetAllTags();
            return Ok(tags);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTagById(int id)
        {
            var tag = await _tagService.GetTagById(id);
            return Ok(tag);
        }
    }
}
