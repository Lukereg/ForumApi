using AutoMapper;
using ForumApi.Entities;
using ForumApi.Models.Posts;

namespace ForumApi.MapProfiles
{
    public class PostMappingProfile: Profile
    {
        public PostMappingProfile()
        {
            CreateMap<AddPostDto, Post>();
        }
    }
}
