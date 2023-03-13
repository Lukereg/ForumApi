using AutoMapper;
using ForumApi.Entities;
using ForumApi.Exceptions;
using ForumApi.Models.Roles;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.Services.UserService
{
    public class UserRolesService : IUserRolesService
    {
        private readonly IForumDbContext _forumDbContext;
        private readonly IMapper _mapper;

        public UserRolesService(IForumDbContext forumDbContext, IMapper mapper)
        {
            _forumDbContext = forumDbContext;
            _mapper = mapper;
        }

        public async Task AddRoleToUser(int userId, string roleName)
        {
            var role = await FindRole(roleName);

            var user = await FindUser(userId);

            if (user.Roles.Contains(role))
                throw new BadRequestException("This user already has this role");

            user.Roles.Add(role);

            await _forumDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<GetRoleDto>> GetUserRoles(int userId)
        {
            var user = await FindUser(userId);

            var roles = new List<Role>();

            foreach (var role in user.Roles)
                roles.Add(role);

            var result = _mapper.Map<List<GetRoleDto>>(roles);
            return result;
        }

        public async Task RemoveRoleFromUser(int userId, string roleName)
        {
            var role = await FindRole(roleName);

            var user = await FindUser(userId);

            if (!user.Roles.Contains(role))
                throw new BadRequestException("This user does not have this role");

            user.Roles.Remove(role);

            await _forumDbContext.SaveChangesAsync();
        }

        private async Task<Role> FindRole(string roleName)
        {
            var role = await _forumDbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
                throw new NotFoundException("Role does not exist");

            return role;
        }

        private async Task<User> FindUser(int userId)
        {
            var user = await _forumDbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new NotFoundException("User does not exist");

            return user;
        }
    }
}
