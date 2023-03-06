using FluentValidation;
using ForumApi.Entities;
using ForumApi.Models.Accounts;
using Microsoft.AspNetCore.Identity;

namespace ForumApi.Validators
{
    public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserDtoValidator(ForumDbContext _forumDbContext, IPasswordHasher<User> passwordHasher)
        {
            RuleFor(x => x.LoginOrEmail)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .Custom((value, context) =>
                {
                    var user = _forumDbContext.Users.FirstOrDefault(u => u.Email == value.LoginOrEmail);
                    user ??= _forumDbContext.Users.FirstOrDefault(u => u.Login == value.LoginOrEmail);

                    if (user != null)
                    {
                        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, value.Password);
                        if (passwordVerificationResult == PasswordVerificationResult.Failed)
                            context.AddFailure("LoginOrEmailOrPassword", "Incorrect data");
                    }
                    else
                    {
                        context.AddFailure("LoginOrEmailOrPassword", "Incorrect data");
                    }
                });
        }
    }
}
