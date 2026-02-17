namespace MyFinanceTracker.Api.Common.DTOs
{
    /// <summary>
    /// Response DTO for error information
    /// </summary>
    public record ErrorResponse
    {
        /// <summary>
        /// The main error message
        /// </summary>
        public string? Message { get; init; }

        /// <summary>
        /// Additional details about the error
        /// </summary>
        public string?Details { get; init; }
    }
}
