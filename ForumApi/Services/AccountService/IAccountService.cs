using ForumApi.Models.Accounts;

namespace ForumApi.Services.AccountService
{
    public interface IAccountService
    {
        public Task<string> LoginUser(LoginUserDto loginUserDto);
        public Task RegisterUser(RegisterUserDto registerUserDto);
    }
}
