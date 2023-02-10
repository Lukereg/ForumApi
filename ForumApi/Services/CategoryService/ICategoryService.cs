using ForumApi.Models.Categories;

namespace ForumApi.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<int> AddCategory(AddCategoryDto addCategoryDto);
        public Task<GetCategoryDto> GetCategoryById(int id);
        public Task<IEnumerable<GetCategoryDto>> GetAllCategories();
        public Task UpdateCategory(UpdateCategoryDto updateCategoryDto, int categoryId);
        public Task DeleteCategory(int id);
    }
}
