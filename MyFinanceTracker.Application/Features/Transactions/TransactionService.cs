using MyFinanceTracker.Application.Common.DTOs;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Application.Features.Transactions.DTOs;
using MyFinanceTracker.Application.Features.Transactions.Filters;
using MyFinanceTracker.Core.Entities;
using MyFinanceTracker.Core.Enums;

namespace MyFinanceTracker.Application.Features.Transactions
{
    internal sealed class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<TransactionResponse>> GetByIdAsync(string userId, Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _transactionRepository.GetByIdAsync(userId, id, cancellationToken);
            if (entity is null)
            {
                return Result<TransactionResponse>.Failure("Transaction not found.");
            }
            return Result<TransactionResponse>.Success(MapToResponse(entity));
        }

        public async Task<Result<IReadOnlyList<TransactionResponse>>> GetAllAsync(string userId, TransactionFilterRequest filter, CancellationToken cancellationToken = default)
        {
            var list = await _transactionRepository.GetAllAsync(userId, filter, cancellationToken);
            return Result<IReadOnlyList<TransactionResponse>>.Success(list.Select(MapToResponse).ToList());
        }

        public async Task<Result<TransactionResponse>> CreateAsync(string userId, CreateTransactionRequest request, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetByIdAsync(userId, request.AccountId, cancellationToken);
            if (account is null)
            {
                return Result<TransactionResponse>.Failure("Account not found.");
            }
            var category = await _categoryRepository.GetByIdAsync(userId, request.CategoryId, cancellationToken);
            if (category is null)
            {
                return Result<TransactionResponse>.Failure("Category not found.");
            }
            var entity = new Transaction
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AccountId = request.AccountId,
                CategoryId = request.CategoryId,
                Type = request.Type,
                Amount = request.Amount,
                TransactionDate = request.TransactionDate,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            ApplyTransactionToBalance(account, request.Type, request.Amount);
            await _accountRepository.UpdateAsync(account, cancellationToken);
            await _transactionRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<TransactionResponse>.Success(MapToResponse(entity));
        }

        public async Task<Result<TransactionResponse>> UpdateAsync(string userId, Guid id, UpdateTransactionRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await _transactionRepository.GetByIdWithAccountAndCategoryAsync(userId, id, cancellationToken);
            if (entity is null)
            {
                return Result<TransactionResponse>.Failure("Transaction not found.");
            }
            var account = await _accountRepository.GetByIdAsync(userId, request.AccountId, cancellationToken);
            if (account is null)
            {
                return Result<TransactionResponse>.Failure("Account not found.");
            }
            var category = await _categoryRepository.GetByIdAsync(userId, request.CategoryId, cancellationToken);
            if (category is null)
            {
                return Result<TransactionResponse>.Failure("Category not found.");
            }
            ApplyTransactionToBalance(entity.Account, entity.Type, -entity.Amount);
            entity.AccountId = request.AccountId;
            entity.CategoryId = request.CategoryId;
            entity.Category = category;
            entity.Account = account;
            entity.Type = request.Type;
            entity.Amount = request.Amount;
            entity.TransactionDate = request.TransactionDate;
            entity.Description = request.Description;
            entity.UpdatedAt = DateTime.UtcNow;
            ApplyTransactionToBalance(account, request.Type, request.Amount);
            await _accountRepository.UpdateAsync(entity.Account, cancellationToken);
            await _accountRepository.UpdateAsync(account, cancellationToken);
            await _transactionRepository.UpdateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<TransactionResponse>.Success(MapToResponse(entity));
        }

        public async Task<Result> DeleteAsync(string userId, Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _transactionRepository.GetByIdWithAccountAndCategoryAsync(userId, id, cancellationToken);
            if (entity is null)
            {
                return Result.Failure("Transaction not found.");
            }
            ApplyTransactionToBalance(entity.Account, entity.Type, -entity.Amount);
            await _accountRepository.UpdateAsync(entity.Account, cancellationToken);
            await _transactionRepository.DeleteAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        private static void ApplyTransactionToBalance(Account account, TransactionType transactionType, decimal amount)
        {
            account.Balance += transactionType == TransactionType.Income ? amount : -amount;
            account.UpdatedAt = DateTime.UtcNow;
        }

        private static TransactionResponse MapToResponse(Transaction t) => new()
        {
            Id = t.Id,
            AccountId = t.AccountId,
            CategoryId = t.CategoryId,
            Type = t.Type,
            Amount = t.Amount,
            TransactionDate = t.TransactionDate,
            Description = t.Description,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        };
    }
}
