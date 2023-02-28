using FluentValidation;
using ForumApi.Entities;

namespace ForumApi.Models.Tags
{
    public class UpdateTagDtoValidator : AbstractValidator<UpdateTagDto>
    {
        public UpdateTagDtoValidator(ForumDbContext _forumDbContext)
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
