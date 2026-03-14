using MyFinanceTracker.Core.Enums;

namespace MyFinanceTracker.Application.Features.Transactions.DTOs
{
    public record CreateTransactionRequest
    {
        public Guid AccountId { get; init; }
        public Guid CategoryId { get; init; }
        public TransactionType Type { get; init; }
        public decimal Amount { get; init; }
        public DateTime TransactionDate { get; init; }
        public string? Description { get; init; }
    }
}
