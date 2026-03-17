using MyFinanceTracker.Core.Enums;

namespace MyFinanceTracker.Application.Features.Transactions.DTOs
{
    public record TransactionResponse
    {
        public Guid Id { get; init; }
        public Guid AccountId { get; init; }
        public Guid CategoryId { get; init; }
        public TransactionType Type { get; init; }
        public decimal Amount { get; init; }
        public DateTime TransactionDate { get; init; }
        public string? Description { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }
}
