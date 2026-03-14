using MyFinanceTracker.Application.Common.DTOs;
using MyFinanceTracker.Application.Features.Transactions.DTOs;
using MyFinanceTracker.Application.Features.Transactions.Filters;

namespace MyFinanceTracker.Application.Common.Interfaces
{
    public interface ITransactionService
    {
        Task<Result<TransactionResponse>> GetByIdAsync(string userId, Guid id, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<TransactionResponse>>> GetAllAsync(string userId, TransactionFilterRequest filter, CancellationToken cancellationToken = default);
        Task<Result<TransactionResponse>> CreateAsync(string userId, CreateTransactionRequest request, CancellationToken cancellationToken = default);
        Task<Result<TransactionResponse>> UpdateAsync(string userId, Guid id, UpdateTransactionRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(string userId, Guid id, CancellationToken cancellationToken = default);
    }
}
