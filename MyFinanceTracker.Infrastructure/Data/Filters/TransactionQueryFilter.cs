using MyFinanceTracker.Application.Features.Transactions.Filters;
using MyFinanceTracker.Core.Entities;

namespace MyFinanceTracker.Infrastructure.Data.Filters
{
    /// <summary>
    /// Applies transaction filter criteria to an <see cref="IQueryable{Transaction}"/>.
    /// Keeps query-building logic out of the repository.
    /// </summary>
    internal sealed class TransactionQueryFilter
    {
        /// <summary>
        /// Applies user scope and optional filter criteria (account, category, date range) to the query.
        /// Caller is responsible for ordering and execution (e.g. OrderByDescending, ToListAsync).
        /// </summary>
        public IQueryable<Transaction> Apply(IQueryable<Transaction> query, string userId, TransactionFilterRequest filter)
        {
            ArgumentNullException.ThrowIfNull(filter);

            query = query.Where(t => t.UserId == userId);

            if (filter.AccountId.HasValue)
            {
                query = query.Where(t => t.AccountId == filter.AccountId.Value);
            }
            if (filter.CategoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == filter.CategoryId.Value);
            }
            if (filter.From.HasValue)
            {
                query = query.Where(t => t.TransactionDate >= filter.From.Value);
            }
            if (filter.To.HasValue)
            {
                query = query.Where(t => t.TransactionDate <= filter.To.Value);
            }

            return query;
        }
    }
}
