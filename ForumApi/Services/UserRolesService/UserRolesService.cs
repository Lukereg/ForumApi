using ForumApi.Entities;
using ForumApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.Services.UserService
{
    public class UserRolesService : IUserRolesService
    {
        private readonly IForumDbContext _forumDbContext;

        public UserRolesService(IForumDbContext forumDbContext)
        {
            _forumDbContext = forumDbContext;
        }

        public async Task AddRoleToUser(int idUser, string roleName)
        {
            var role = await _forumDbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
                throw new NotFoundException("Role does not exist");

            var user = await _forumDbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == idUser);
            if (user == null)
                throw new NotFoundException("User does not exist");

            if (user.Roles.Contains(role))
                throw new BadRequestException("This user already has this role");

            user.Roles.Add(role);

            await _forumDbContext.SaveChangesAsync();
        }
    }
}
