using MyFinanceTracker.Core.Entities;

namespace MyFinanceTracker.Application.Common.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(string userId, Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Account>> GetAllAsync(string userId, bool includeInactive, CancellationToken cancellationToken = default);
        Task AddAsync(Account account, CancellationToken cancellationToken = default);
        Task UpdateAsync(Account account, CancellationToken cancellationToken = default);
        Task DeleteAsync(Account account, CancellationToken cancellationToken = default);
    }
}
