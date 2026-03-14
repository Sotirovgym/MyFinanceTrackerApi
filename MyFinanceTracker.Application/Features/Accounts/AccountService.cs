using MyFinanceTracker.Application.Common.DTOs;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Application.Features.Accounts.DTOs;
using MyFinanceTracker.Core.Entities;

namespace MyFinanceTracker.Application.Features.Accounts
{
    internal sealed class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AccountResponse>> GetByIdAsync(string userId, Guid id, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetByIdAsync(userId, id, cancellationToken);
            if (account is null)
            {
                return Result<AccountResponse>.Failure("Account not found.");
            }
            return Result<AccountResponse>.Success(MapToResponse(account));
        }

        public async Task<Result<IReadOnlyList<AccountResponse>>> GetAllAsync(string userId, bool includeInactive = false, CancellationToken cancellationToken = default)
        {
            var accounts = await _accountRepository.GetAllAsync(userId, includeInactive, cancellationToken);
            return Result<IReadOnlyList<AccountResponse>>.Success(accounts.Select(MapToResponse).ToList());
        }

        public async Task<Result<AccountResponse>> CreateAsync(string userId, CreateAccountRequest request, CancellationToken cancellationToken = default)
        {
            var account = new Account
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = request.Name,
                AccountType = request.AccountType,
                Currency = request.Currency,
                Balance = request.InitialBalance,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _accountRepository.AddAsync(account, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<AccountResponse>.Success(MapToResponse(account));
        }

        public async Task<Result<AccountResponse>> UpdateAsync(string userId, Guid id, UpdateAccountRequest request, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetByIdAsync(userId, id, cancellationToken);
            if (account is null)
            {
                return Result<AccountResponse>.Failure("Account not found.");
            }
            account.Name = request.Name;
            account.AccountType = request.AccountType;
            account.Currency = request.Currency;
            account.IsActive = request.IsActive;
            account.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.UpdateAsync(account, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<AccountResponse>.Success(MapToResponse(account));
        }

        public async Task<Result> DeleteAsync(string userId, Guid id, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetByIdAsync(userId, id, cancellationToken);
            if (account is null)
            {
                return Result.Failure("Account not found.");
            }
            await _accountRepository.DeleteAsync(account, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        private static AccountResponse MapToResponse(Account a) => new()
        {
            Id = a.Id,
            Name = a.Name,
            AccountType = a.AccountType,
            Currency = a.Currency,
            Balance = a.Balance,
            IsActive = a.IsActive,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        };
    }
}
