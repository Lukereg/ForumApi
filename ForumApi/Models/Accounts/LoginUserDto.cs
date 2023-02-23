namespace ForumApi.Models.Accounts
{
    public class LoginUserDto
    {
        public string LoginOrEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
