using FluentValidation;
using ForumApi.Entities;

namespace ForumApi.Models.Categories
{
    public class AddCategoryDtoValidator : AbstractValidator<AddCategoryDto>
    {
        public AddCategoryDtoValidator(ForumDbContext _forumDbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200)
                .Custom((value, context) =>
                {
                    var category = _forumDbContext.Categories.Any(c => c.Name == value);

                    if (category)
                        context.AddFailure("Category", "This category already exists");
                });
        }
    }
}
