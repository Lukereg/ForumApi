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
                
        }
    }
}
