using MyFinanceTracker.Core.Entities;

namespace MyFinanceTracker.Application.Common.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(string userId, Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Category>> GetAllAsync(string userId, bool includeInactive, CancellationToken cancellationToken = default);
        Task AddAsync(Category category, CancellationToken cancellationToken = default);
        Task UpdateAsync(Category category, CancellationToken cancellationToken = default);
        Task DeleteAsync(Category category, CancellationToken cancellationToken = default);
    }
}
