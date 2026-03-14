using FluentValidation;
using MyFinanceTracker.Application.Features.Accounts.DTOs;

namespace MyFinanceTracker.Application.Features.Accounts.Validators
{
    internal sealed class CreateAccountRequestValidator : AbstractValidator<CreateAccountRequest>
    {
        public CreateAccountRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.AccountType).IsInEnum();
            RuleFor(x => x.Currency).IsInEnum();
        }
    }
}
