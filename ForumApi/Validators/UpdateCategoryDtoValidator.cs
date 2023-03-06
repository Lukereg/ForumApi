using FluentValidation;
using ForumApi.Entities;
using ForumApi.Models.Categories;

namespace ForumApi.Validators
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator(ForumDbContext _forumDbContext)
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
