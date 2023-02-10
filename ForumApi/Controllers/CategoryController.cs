﻿using ForumApi.Models.Categories;
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
    }
}
