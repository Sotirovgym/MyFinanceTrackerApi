using MyFinanceTracker.Application.Features.Transactions.Filters;
using MyFinanceTracker.Core.Entities;

namespace MyFinanceTracker.Application.Common.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(string userId, Guid id, CancellationToken cancellationToken = default);
        Task<Transaction?> GetByIdWithAccountAndCategoryAsync(string userId, Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Transaction>> GetAllAsync(string userId, TransactionFilterRequest filter, CancellationToken cancellationToken = default);
        Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
        Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default);
        Task DeleteAsync(Transaction transaction, CancellationToken cancellationToken = default);
    }
}
