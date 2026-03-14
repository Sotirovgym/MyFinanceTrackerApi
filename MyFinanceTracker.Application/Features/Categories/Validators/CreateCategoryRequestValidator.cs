using FluentValidation;
using MyFinanceTracker.Application.Features.Categories.DTOs;

namespace MyFinanceTracker.Application.Features.Categories.Validators
{
    internal sealed class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Type).IsInEnum();
        }
    }
}
