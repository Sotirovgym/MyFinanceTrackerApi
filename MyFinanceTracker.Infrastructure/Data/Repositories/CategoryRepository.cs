using Microsoft.EntityFrameworkCore;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Core.Entities;

namespace MyFinanceTracker.Infrastructure.Data.Repositories
{
    internal sealed class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Category?> GetByIdAsync(string userId, Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Category>> GetAllAsync(string userId, bool includeInactive, CancellationToken cancellationToken = default)
        {
            return await _db.Categories
                .AsNoTracking()
                .Where(c => c.UserId == userId && (includeInactive || c.IsActive))
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }

        public Task AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            _db.Categories.Add(category);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
        {
            _db.Categories.Update(category);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Category category, CancellationToken cancellationToken = default)
        {
            _db.Categories.Remove(category);
            return Task.CompletedTask;
        }
    }
}
