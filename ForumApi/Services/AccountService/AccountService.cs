using ForumApi.Entities;
using ForumApi.Exceptions;
using ForumApi.Models.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ForumApi.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IForumDbContext _forumDbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(IForumDbContext forumDbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _forumDbContext = forumDbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public async Task<string> LoginUser(LoginUserDto loginUserDto)
        {
            var user = await _forumDbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == loginUserDto.LoginOrEmail);
            user ??= await _forumDbContext.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Login == loginUserDto.LoginOrEmail);

            if (user is null)
                throw new BadRequestException("Invalid email, login or password");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginUserDto.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Invalid email, login or password");

            return GenerateTokenJwt(user);
        }

        private string GenerateTokenJwt(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                new Claim("Roles", String.Join(",", user.Roles.Select(r => r.Name))),
            };

            foreach (Role r in user.Roles)
                claims.Add(new Claim(ClaimTypes.Role, r.Name));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, 
                _authenticationSettings.JwtIssuer,
                claims, 
                expires: expires, 
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
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
