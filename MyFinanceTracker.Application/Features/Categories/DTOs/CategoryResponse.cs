using MyFinanceTracker.Core.Enums;

namespace MyFinanceTracker.Application.Features.Categories.DTOs
{
    public record CategoryResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public TransactionType Type { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
    }
}
