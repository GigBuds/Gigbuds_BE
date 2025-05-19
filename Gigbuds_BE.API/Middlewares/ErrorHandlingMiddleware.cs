using Gigbuds_BE.Domain.Exceptions;

namespace Gigbuds_BE.API.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        // =======================================
        // === Fields & Props
        // =======================================

        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        // =======================================
        // === Constructor
        // =======================================

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        // =======================================
        // === Methods
        // =======================================

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Request has been pass down into the next middleware
                await next.Invoke(context);

                // In here, if error occurs, then short-circuiting the middleware
                // and catch the exception
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, message: ex.Message);
                await WriteToResponse(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (CreateFailedException ex)
            {
                _logger.LogError(ex, message: ex.Message);
                await WriteToResponse(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (DuplicateUserException ex)
            {
                _logger.LogError(ex, message: ex.Message);
                await WriteToResponse(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, message: ex.Message);
                await WriteToResponse(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Write essential data to response object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task WriteToResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                StatusCode = statusCode,
                Message = message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
