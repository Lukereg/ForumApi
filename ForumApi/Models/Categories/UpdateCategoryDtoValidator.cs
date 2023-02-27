using FluentValidation;
using ForumApi.Entities;

namespace ForumApi.Models.Categories
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator(ForumDbContext _forumDbContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Custom((value, context) =>
                {
                    var category = _forumDbContext.Categories.Any(c => c.Name == value);

                    if (category)
                        context.AddFailure("Category", "This category already exists");
                });
        }
    }
}
