using MyFinanceTracker.Core.Enums;

namespace MyFinanceTracker.Core.Entities
{
    public sealed class Transaction
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Account Account { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
