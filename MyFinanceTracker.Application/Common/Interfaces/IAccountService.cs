using MyFinanceTracker.Application.Common.DTOs;
using MyFinanceTracker.Application.Features.Accounts.DTOs;

namespace MyFinanceTracker.Application.Common.Interfaces
{
    public interface IAccountService
    {
        Task<Result<AccountResponse>> GetByIdAsync(string userId, Guid id, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<AccountResponse>>> GetAllAsync(string userId, bool includeInactive = false, CancellationToken cancellationToken = default);
        Task<Result<AccountResponse>> CreateAsync(string userId, CreateAccountRequest request, CancellationToken cancellationToken = default);
        Task<Result<AccountResponse>> UpdateAsync(string userId, Guid id, UpdateAccountRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(string userId, Guid id, CancellationToken cancellationToken = default);
    }
}
