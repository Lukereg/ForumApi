using FluentValidation;
using ForumApi.Entities;
using ForumApi.Models.Tags;

namespace ForumApi.Validators
{
    public class AddTagDtoValidator : AbstractValidator<AddTagDto>
    {
        public AddTagDtoValidator(ForumDbContext _forumDbContext)
        {
            RuleFor(x => x.Value)
                .NotEmpty()
                .MaximumLength(40)
                .Custom((value, context) =>
                {
                    var tag = _forumDbContext.Tags.Any(t => t.Value == value);

                    if (tag)
                        context.AddFailure("Tag", "This tag already exists");
                });
        }
    }
}
