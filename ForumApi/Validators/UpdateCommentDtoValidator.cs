using FluentValidation;
using ForumApi.Entities;
using ForumApi.Models.Comments;

namespace ForumApi.Validators
{
    public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
    {
        public UpdateCommentDtoValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty()
                .MaximumLength(10000000);
        }
    }
}
