using Microsoft.EntityFrameworkCore;
using residence.api.Models;
using System.Net;
using System.Text.Json;

namespace residence.api.Middleware
{
    /// <summary>
    /// Global exception handling middleware that catches all unhandled exceptions
    /// and returns structured error responses with appropriate HTTP status codes
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred. Exception details: {ExceptionMessage}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path
            };

            // Determine status code and set response based on exception type
            switch (exception)
            {
                case DbUpdateException dbEx:
                    HandleDbUpdateException(context, dbEx, response);
                    break;

                case InvalidOperationException invalidEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Title = "Invalid Operation";
                    response.Message = invalidEx.Message;
                    response.ErrorCode = "INVALID_OPERATION";
                    break;

                case ArgumentException argEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Title = "Argument Error";
                    response.Message = argEx.Message;
                    response.ErrorCode = "ARGUMENT_ERROR";
                    break;

                case UnauthorizedAccessException unauthorizedEx:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Title = "Unauthorized";
                    response.Message = "You do not have permission to access this resource.";
                    response.ErrorCode = "UNAUTHORIZED";
                    break;

                case KeyNotFoundException keyNotFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Title = "Resource Not Found";
                    response.Message = keyNotFoundEx.Message;
                    response.ErrorCode = "NOT_FOUND";
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Title = "Internal Server Error";
                    response.Message = "An unexpected error occurred while processing your request.";
                    response.ErrorCode = "INTERNAL_SERVER_ERROR";
                    break;
            }

            // Include stack trace and inner exception details only in development
            if (_environment.IsDevelopment())
            {
                response.StackTrace = exception.StackTrace;
                if (exception.InnerException != null)
                {
                    response.InnerError = new ErrorResponse
                    {
                        Title = exception.InnerException.GetType().Name,
                        Message = exception.InnerException.Message,
                        StackTrace = exception.InnerException.StackTrace,
                        ErrorCode = "INNER_EXCEPTION"
                    };
                }
            }

            context.Response.StatusCode = response.StatusCode;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return context.Response.WriteAsJsonAsync(response, options);
        }

        private void HandleDbUpdateException(HttpContext context, DbUpdateException ex, ErrorResponse response)
        {
            // Log the full exception with inner details
            _logger.LogError(ex, "Database update exception occurred. Inner exception: {InnerException}", 
                ex.InnerException?.Message);

            // Check for specific constraint violations
            var innerException = ex.InnerException?.Message ?? string.Empty;

            if (innerException.Contains("IX_Users_Email", StringComparison.OrdinalIgnoreCase) ||
                innerException.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                response.StatusCode = (int)HttpStatusCode.Conflict;
                response.Title = "Duplicate Entry";
                response.Message = "This email is already registered in the system.";
                response.ErrorCode = "DUPLICATE_EMAIL";
                return;
            }

            // Check for other unique constraint violations
            if (innerException.Contains("UNIQUE KEY", StringComparison.OrdinalIgnoreCase) ||
                innerException.Contains("unique constraint", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                response.StatusCode = (int)HttpStatusCode.Conflict;
                response.Title = "Duplicate Entry";
                response.Message = "This record already exists in the system.";
                response.ErrorCode = "DUPLICATE_ENTRY";
                return;
            }

            // Check for foreign key violations
            if (innerException.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) ||
                innerException.Contains("constraint", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Title = "Invalid Reference";
                response.Message = "The referenced record does not exist or the operation violates referential integrity.";
                response.ErrorCode = "INVALID_REFERENCE";
                return;
            }

            // Generic database error
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.Title = "Database Error";
            response.Message = "An error occurred while accessing the database. Please try again later.";
            response.ErrorCode = "DATABASE_ERROR";
        }
    }
}
