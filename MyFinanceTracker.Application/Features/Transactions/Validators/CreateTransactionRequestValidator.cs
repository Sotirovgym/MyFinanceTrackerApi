using FluentValidation;
using MyFinanceTracker.Application.Features.Transactions.DTOs;

namespace MyFinanceTracker.Application.Features.Transactions.Validators
{
    internal sealed class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
    {
        public CreateTransactionRequestValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.Type).IsInEnum();
            RuleFor(x => x.Amount).NotEqual(0).WithMessage("Amount cannot be zero.");
            RuleFor(x => x.Description).MaximumLength(500);
        }
    }
}
