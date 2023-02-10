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
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<ActionResult> AddCategory([FromBody] AddCategoryDto addCategoryDto)
        {
            await categoryService.AddCategory(addCategoryDto);
            return Ok();
        }
    }
}
