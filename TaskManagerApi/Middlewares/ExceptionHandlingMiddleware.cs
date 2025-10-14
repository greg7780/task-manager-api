using System.Net;
using System.Text.Json;
using TaskManagerApi.DTOs;
using TaskManagerApi.Exceptions;

namespace TaskManagerApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpResponseException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                var errorResponse = new ErrorResponse
                {
                    StatusCode = ex.StatusCode,
                    Message = ex.Message,
                    TraceId = context.TraceIdentifier
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var errorResponse = new ErrorResponse
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An unexpected error occurred.",
                    TraceId = context.TraceIdentifier
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
