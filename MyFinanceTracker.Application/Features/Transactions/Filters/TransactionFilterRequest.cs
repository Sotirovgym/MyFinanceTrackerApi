namespace MyFinanceTracker.Application.Features.Transactions.Filters
{
    /// <summary>
    /// Query filter for listing transactions (e.g. GET /api/Transactions with query params accountId, categoryId, from, to).
    /// All properties are optional; omitted values mean "no filter" for that criterion.
    /// </summary>
    public record TransactionFilterRequest
    {
        public Guid? AccountId { get; init; }
        public Guid? CategoryId { get; init; }
        public DateTime? From { get; init; }
        public DateTime? To { get; init; }
    }
}
