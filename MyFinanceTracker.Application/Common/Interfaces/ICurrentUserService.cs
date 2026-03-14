namespace MyFinanceTracker.Application.Common.Interfaces
{
    /// <summary>
    /// Provides the current authenticated user's identity. Implemented in the API layer using the request's claims.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// The current user's ID (from the NameIdentifier claim), or <see cref="string.Empty"/> when not authenticated.
        /// </summary>
        string UserId { get; }
    }
}
