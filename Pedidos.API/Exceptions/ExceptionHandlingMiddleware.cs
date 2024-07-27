using System.Net;
using Pedidos.Infrastructure.Exceptions;

namespace Pedidos.API.Exceptions
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
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Not found error occurred.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                var errorResponse = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, "Bad request error occurred.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var errorResponse = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var errorResponse = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An unexpected error occurred. Please try again later."
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}