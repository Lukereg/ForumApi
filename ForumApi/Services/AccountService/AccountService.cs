using ForumApi.Entities;
using ForumApi.Exceptions;
using ForumApi.Models.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ForumApi.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly ForumDbContext _forumDbContext;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(ForumDbContext forumDbContext, IPasswordHasher<User> passwordHasher)
        {
            _forumDbContext = forumDbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task RegisterUser(RegisterUserDto registerUserDto)
        {
            var newUser = new User()
            {
                Email = registerUserDto.Email,
                Login = registerUserDto.Login,
                Name = registerUserDto.Name,
                Surname = registerUserDto.Surname
            };

            newUser.Password = _passwordHasher.HashPassword(newUser, registerUserDto.Password);

            var userRole = await _forumDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "User");

            if (userRole is null)
                throw new NotFoundException("User role does not exist in the database");

            newUser.Roles.Add(userRole);

            await _forumDbContext.Users.AddAsync(newUser);
            await _forumDbContext.SaveChangesAsync();
        }
    }
}
