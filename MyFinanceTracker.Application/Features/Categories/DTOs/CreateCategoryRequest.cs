using MyFinanceTracker.Core.Enums;

namespace MyFinanceTracker.Application.Features.Categories.DTOs
{
    public record CreateCategoryRequest
    {
        public string Name { get; init; } = null!;
        public TransactionType Type { get; init; }
    }
}
