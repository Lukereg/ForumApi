using ForumApi.Models.Roles;

namespace ForumApi.Services.UserService
{
    public interface IUserRolesService
    {
        public Task AddRoleToUser(int userId, string roleName);
        public Task<IEnumerable<GetRoleDto>> GetUserRoles(int userId);
        public Task RemoveRoleFromUser(int userId, string roleName);
    }
}
