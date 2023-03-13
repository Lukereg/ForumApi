namespace ForumApi.Services.UserService
{
    public interface IUserRolesService
    {
        public Task AddRoleToUser(int idUser, string roleName);
    }
}
