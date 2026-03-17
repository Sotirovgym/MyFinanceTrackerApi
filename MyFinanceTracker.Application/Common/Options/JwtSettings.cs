namespace MyFinanceTracker.Application.Common.Options
{
    /// <summary>
    /// Configuration options for JWT token generation and validation.
    /// </summary>
    public class JwtSettings
    {
        public const string SectionName = "Jwt";

        /// <summary>
        /// The secret key used to sign tokens. Must be at least 32 characters for HS256.
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// The issuer of the token.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// The intended audience of the token.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Token expiration time in minutes.
        /// </summary>
        public int ExpirationMinutes { get; set; } = 60;
    }
}
