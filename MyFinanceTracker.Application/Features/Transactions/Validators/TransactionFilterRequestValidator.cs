using FluentValidation;
using MyFinanceTracker.Application.Features.Transactions.Filters;

namespace MyFinanceTracker.Application.Features.Transactions.Validators
{
    internal sealed class TransactionFilterRequestValidator : AbstractValidator<TransactionFilterRequest>
    {
        public TransactionFilterRequestValidator()
        {
            RuleFor(x => x)
                .Must(x => !x.From.HasValue || !x.To.HasValue || x.From.Value <= x.To.Value)
                .WithMessage("'From' date must be before or equal to 'To' date.");
        }
    }
}
