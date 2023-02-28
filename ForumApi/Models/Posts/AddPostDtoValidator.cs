using FluentValidation;
using ForumApi.Entities;

namespace ForumApi.Models.Posts
{
    public class AddPostDtoValidator : AbstractValidator<AddPostDto>
    {
        public AddPostDtoValidator(ForumDbContext _forumDbContext)
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Message)
                .NotEmpty()
                .MaximumLength(1000000000);

            RuleFor(x => x.AuthorId)
                .Custom((value, context) =>
                {
                    var author = _forumDbContext.Users.Any(u => u.Id == value);

                    if (!author)
                        context.AddFailure("Author", "There is no user with this id");
                });

            RuleFor(x => x.TagsIds)
                .Custom((value, context) =>
                {
                    if (value is not null)
                        foreach (var id in value)
                        {
                            var tag = _forumDbContext.Tags.Any(t => t.Id == id);

                            if (!tag)
                                context.AddFailure("Tag", "Invalid tag id");
                        }
                });
        }
    }
}
