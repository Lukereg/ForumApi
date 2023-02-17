using AutoMapper;
using ForumApi.Entities;
using ForumApi.Models.Tags;

namespace ForumApi.MapProfiles
{
    public class TagMappingProfile: Profile
    {
        public TagMappingProfile()
        {
            CreateMap<AddTagDto, Tag>();
            CreateMap<Tag, GetTagDto>();
        }
    }
}
