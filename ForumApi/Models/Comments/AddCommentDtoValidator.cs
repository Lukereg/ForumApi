using FluentValidation;
using ForumApi.Entities;

namespace ForumApi.Models.Comments
{
    public class AddCommentDtoValidator : AbstractValidator<AddCommentDto>
    {
        public AddCommentDtoValidator(ForumDbContext _forumDbContext)
        {
            RuleFor(x => x.Message)
                .NotEmpty()
                .MaximumLength(10000000);

            RuleFor(x => x.AuthorId)
                .Custom((value, context) =>
                {
                    var author = _forumDbContext.Users.Any(u => u.Id == value);

                    if (!author)
                        context.AddFailure("Author", "There is no user with this id");
                });
                
        }
    }
}
