using ForumApi.Models.Accounts;

namespace ForumApi.Services.AccountService
{
    public interface IAccountService
    {
        public Task RegisterUser(RegisterUserDto registerUserDto);
    }
}
