using ForumApi.Models.Categories;

namespace ForumApi.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<int> AddCategory(AddCategoryDto addCategoryDto);
    }
}
