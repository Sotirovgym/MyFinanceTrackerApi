using MyFinanceTracker.Application.Common.DTOs;
using MyFinanceTracker.Application.Features.Categories.DTOs;

namespace MyFinanceTracker.Application.Common.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<CategoryResponse>> GetByIdAsync(string userId, Guid id, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<CategoryResponse>>> GetAllAsync(string userId, bool includeInactive = false, CancellationToken cancellationToken = default);
        Task<Result<CategoryResponse>> CreateAsync(string userId, CreateCategoryRequest request, CancellationToken cancellationToken = default);
        Task<Result<CategoryResponse>> UpdateAsync(string userId, Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(string userId, Guid id, CancellationToken cancellationToken = default);
    }
}
