﻿using ForumApi.Entities;
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

        public async Task AddRoleToUser(int userId, string roleName)
        {
            var role = await _forumDbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
                throw new NotFoundException("Role does not exist");

            var user = await _forumDbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new NotFoundException("User does not exist");

            if (user.Roles.Contains(role))
                throw new BadRequestException("This user already has this role");

            user.Roles.Add(role);

            await _forumDbContext.SaveChangesAsync();
        }

        public async Task RemoveRoleFromUser(int userId, string roleName)
        {
            var role = await _forumDbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
                throw new NotFoundException("Role does not exist");

            var user = await _forumDbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new NotFoundException("User does not exist");

            if (!user.Roles.Contains(role))
                throw new BadRequestException("This user does not have this role");

            user.Roles.Remove(role);

            await _forumDbContext.SaveChangesAsync();
        }
    }
}
