using MyFinanceTracker.Core.Enums;

namespace MyFinanceTracker.Application.Features.Accounts.DTOs
{
    public record AccountResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public AccountType AccountType { get; init; }
        public CurrencyCode Currency { get; init; }
        public decimal Balance { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }
}
