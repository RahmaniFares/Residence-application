namespace residence.api.Models
{
    /// <summary>
    /// Standard error response model for API errors
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Timestamp when the error occurred
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// HTTP status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Error title/type
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detailed error message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Stack trace (only in development environment)
        /// </summary>
        public string? StackTrace { get; set; }

        /// <summary>
        /// Inner exception details (only in development environment)
        /// </summary>
        public ErrorResponse? InnerError { get; set; }

        /// <summary>
        /// Request path that caused the error
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// Error code for third-party integration
        /// </summary>
        public string? ErrorCode { get; set; }
    }
}
