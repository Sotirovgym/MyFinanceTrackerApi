using Microsoft.EntityFrameworkCore;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Core.Entities;

namespace MyFinanceTracker.Infrastructure.Data.Repositories
{
    internal sealed class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;

        public AccountRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Account?> GetByIdAsync(string userId, Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Account>> GetAllAsync(string userId, bool includeInactive, CancellationToken cancellationToken = default)
        {
            return await _db.Accounts
                .AsNoTracking()
                .Where(a => a.UserId == userId && (includeInactive || a.IsActive))
                .OrderBy(a => a.Name)
                .ToListAsync(cancellationToken);
        }

        public Task AddAsync(Account account, CancellationToken cancellationToken = default)
        {
            _db.Accounts.Add(account);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Account account, CancellationToken cancellationToken = default)
        {
            _db.Accounts.Update(account);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Account account, CancellationToken cancellationToken = default)
        {
            _db.Accounts.Remove(account);
            return Task.CompletedTask;
        }
    }
}
