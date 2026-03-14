using MyFinanceTracker.Core.Enums;

namespace MyFinanceTracker.Application.Features.Categories.DTOs
{
    public record UpdateCategoryRequest
    {
        public string Name { get; init; } = null!;
        public TransactionType Type { get; init; }
        public bool IsActive { get; init; }
    }
}
