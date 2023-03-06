using FluentValidation;
using ForumApi.Entities;
using ForumApi.Models.Accounts;

namespace ForumApi.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(ForumDbContext _forumDbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(80)
                .Custom((value, context) =>
                {
                    var email = _forumDbContext.Users.Any(u => u.Email == value);

                    if (email)
                        context.AddFailure("Email", "That email is taken");
                });

            RuleFor(x => x.Login)
                .NotEmpty()
                .MaximumLength(80)
                .Custom((value, context) =>
                {
                    if (value.Contains("@"))
                        context.AddFailure("Login", "Login cannot contain @");

                    var login = _forumDbContext.Users.Any(u => u.Login == value);

                    if (login)
                        context.AddFailure("Login", "User with given login already exists");
                });

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(e => e.Password)
                .WithMessage("The passwords must be the same");

            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches("^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$")
                .MaximumLength(60);

            RuleFor(x => x.Surname)
                .NotEmpty()
                .Matches("^[a-zA-ZàáâäãåąčćęèéêëėįìíîïłńòóôöõøùúûüųūÿýżźñçčšžÀÁÂÄÃÅĄĆČĖĘÈÉÊËÌÍÎÏĮŁŃÒÓÔÖÕØÙÚÛÜŲŪŸÝŻŹÑßÇŒÆČŠŽ∂ð ,.'-]+$")
                .MaximumLength(60);
        }
    }
}
