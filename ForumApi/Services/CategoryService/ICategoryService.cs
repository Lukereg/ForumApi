using ForumApi.Models.Categories;

namespace ForumApi.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<int> AddCategory(AddCategoryDto addCategoryDto);
        public Task<GetCategoryDto> GetCategoryById(int id);
    }
}
