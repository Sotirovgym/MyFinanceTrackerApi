namespace MyFinanceTracker.Api.Common.Options
{
    /// <summary>
    /// Configuration options for CORS (allowed origins for the SPA).
    /// </summary>
    public class CorsSettings
    {
        public const string SectionName = "Cors";

        /// <summary>
        /// Origins allowed to call the API (e.g. SPA URL). Defaults to localhost:3000 if empty.
        /// </summary>
        public string[] AllowedOrigins { get; set; } = new[] { "http://localhost:3000" };
    }
}
