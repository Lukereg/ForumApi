using ForumApi.Models.Categories;
using ForumApi.Services.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumApi.Controllers
{
    [Route("v1/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<ActionResult> AddCategory([FromBody] AddCategoryDto addCategoryDto)
        {
            var id = await _categoryService.AddCategory(addCategoryDto);
            return Created($"/v1/categories/{id}", null);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategoryById([FromRoute] int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return Ok(category);
        }

        [AllowAnonymous]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<GetCategoryDto>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory([FromBody] UpdateCategoryDto updateCategoryDto, [FromRoute] int id)
        {
            await _categoryService.UpdateCategory(updateCategoryDto, id);
            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] int id)
        {
            await _categoryService.DeleteCategory(id);
            return NoContent();
        }
    }
}
