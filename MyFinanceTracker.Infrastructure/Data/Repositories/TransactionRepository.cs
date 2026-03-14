using Microsoft.EntityFrameworkCore;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Application.Features.Transactions.Filters;
using MyFinanceTracker.Core.Entities;
using MyFinanceTracker.Infrastructure.Data.Filters;

namespace MyFinanceTracker.Infrastructure.Data.Repositories
{
    internal sealed class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly TransactionQueryFilter _queryFilter;

        public TransactionRepository(ApplicationDbContext db, TransactionQueryFilter queryFilter)
        {
            _db = db;
            _queryFilter = queryFilter;
        }

        public async Task<Transaction?> GetByIdAsync(string userId, Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.UserId == userId && t.Id == id, cancellationToken);
        }

        public async Task<Transaction?> GetByIdWithAccountAndCategoryAsync(string userId, Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Transactions
                .Include(t => t.Account)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.UserId == userId && t.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Transaction>> GetAllAsync(string userId, TransactionFilterRequest filter, CancellationToken cancellationToken = default)
        {
            var query = _queryFilter.Apply(_db.Transactions.AsNoTracking(), userId, filter);

            return await query
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync(cancellationToken);
        }

        public Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            _db.Transactions.Add(transaction);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            _db.Transactions.Update(transaction);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            _db.Transactions.Remove(transaction);
            return Task.CompletedTask;
        }
    }
}
