using AutoMapper;
using ForumApi.Entities;
using ForumApi.Models.Comments;

namespace ForumApi.MapProfiles
{
    public class CommentMappingProfile: Profile
    {
        public CommentMappingProfile()
        {
            CreateMap<AddCommentDto, Comment>();
            CreateMap<Comment, GetCommentDto>();
        }
    }
}
