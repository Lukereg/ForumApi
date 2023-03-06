using FluentValidation;
using ForumApi.Entities;
using ForumApi.Models.Comments;

namespace ForumApi.Validators
{
    public class AddCommentDtoValidator : AbstractValidator<AddCommentDto>
    {
        public AddCommentDtoValidator(ForumDbContext _forumDbContext)
        {
            RuleFor(x => x.Message)
                .NotEmpty()
                .MaximumLength(10000000);

        }
    }
}
