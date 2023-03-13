using AutoMapper;
using ForumApi.Entities;
using ForumApi.Models.Roles;

namespace ForumApi.MapProfiles
{
    public class RoleMappingProfile: Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, GetRoleDto>();
        }
    }
}
