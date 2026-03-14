using MyFinanceTracker.Core.Enums;

namespace MyFinanceTracker.Application.Features.Accounts.DTOs
{
    public record CreateAccountRequest
    {
        public string Name { get; init; } = null!;
        public AccountType AccountType { get; init; }
        public CurrencyCode Currency { get; init; }
        public decimal InitialBalance { get; init; }
    }
}
