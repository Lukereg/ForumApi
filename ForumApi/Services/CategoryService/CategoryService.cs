using AutoMapper;
using ForumApi.Entities;
using ForumApi.Models.Categories;

namespace ForumApi.Services.CategoryService
{
    public class CategoryService: ICategoryService
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IMapper _mapper;
        
        public CategoryService(ForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task<int> AddCategory(AddCategoryDto addCategoryDto)
        {
            var category = _mapper.Map<Category>(addCategoryDto);
            _forumDbContext.Categories.Add(category);
            await _forumDbContext.SaveChangesAsync();

            return category.Id;
        }

    }
}
