using FluentValidation;
using MyFinanceTracker.Application.Features.Categories.DTOs;

namespace MyFinanceTracker.Application.Features.Categories.Validators
{
    internal sealed class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Type).IsInEnum();
        }
    }
}
