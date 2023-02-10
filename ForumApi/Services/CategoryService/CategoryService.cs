using AutoMapper;
using ForumApi.Entities;
using ForumApi.Exceptions;
using ForumApi.Models.Categories;
using Microsoft.EntityFrameworkCore;

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

        public async Task<GetCategoryDto> GetCategoryById(int id)
        {
            var category = await _forumDbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category is null)
                throw new NotFoundException("Category not found");

            var result = _mapper.Map<GetCategoryDto>(category);
            return result;
        }

        public async Task<IEnumerable<GetCategoryDto>> GetAllCategories()
        {
            var categories = await _forumDbContext.Categories.ToListAsync();
            var result = _mapper.Map<List<GetCategoryDto>>(categories);
            return result;
        }

        public async Task UpdateCategory(UpdateCategoryDto updateCategoryDto, int categoryId)
        {
            var category = await _forumDbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            
            if (category is null)
                throw new NotFoundException("Category not found");

            category.Name = updateCategoryDto.Name;

            await _forumDbContext.SaveChangesAsync();
        }
    }
}
