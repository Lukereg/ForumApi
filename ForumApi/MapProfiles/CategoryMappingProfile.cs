using AutoMapper;
using ForumApi.Entities;
using ForumApi.Models.Categories;

namespace ForumApi.MapProfiles
{
    public class CategoryMappingProfile: Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<AddCategoryDto, Category>();
        }
    }
}
