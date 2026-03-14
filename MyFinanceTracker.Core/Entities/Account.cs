using MyFinanceTracker.Core.Enums;

namespace MyFinanceTracker.Core.Entities
{
    public sealed class Account
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public AccountType AccountType { get; set; }
        public CurrencyCode Currency { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
