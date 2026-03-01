namespace MyFinanceTracker.Application.Features.Authentication.DTOs
{
    /// <summary>
    /// Response containing the JWT access token and expiration.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// The JWT bearer token for authentication.
        /// </summary>
        public string AccessToken { get; init; } = null!;

        /// <summary>
        /// The UTC date and time when the token expires.
        /// </summary>
        public DateTime ExpiresAt { get; init; }
    }
}
