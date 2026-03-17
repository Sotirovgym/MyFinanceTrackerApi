namespace MyFinanceTracker.Application.Common.Interfaces
{
    /// <summary>
    /// Ensures that multiple repository operations in a single use case are committed in one transaction.
    /// </summary>
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
