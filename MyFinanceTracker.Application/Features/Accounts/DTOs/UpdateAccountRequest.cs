using MyFinanceTracker.Core.Enums;

namespace MyFinanceTracker.Application.Features.Accounts.DTOs
{
    public record UpdateAccountRequest
    {
        public string Name { get; init; } = null!;
        public AccountType AccountType { get; init; }
        public CurrencyCode Currency { get; init; }
        public bool IsActive { get; init; }
    }
}
