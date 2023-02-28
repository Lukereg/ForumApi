﻿using FluentValidation;
using ForumApi.Entities;

namespace ForumApi.Models.Comments
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