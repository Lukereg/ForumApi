namespace ForumApi.Services.UserService
{
    public interface IUserRolesService
    {
        public Task AddRoleToUser(int userId, string roleName);
        public Task RemoveRoleFromUser(int userId, string roleName);
    }
}
