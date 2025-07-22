using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApi.Models.Dtos;

namespace WebApi.Utils
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request failed : {Message}", ex.Message);

                await HandleExceptionAsync(context, ex);
            }
            finally
            {
                _logger.LogInformation("Request {Path} completed with status {StatusCode}",
                    context.Request.Path, context.Response.StatusCode);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";

            if (exception is ServerException serverException)
            {
                context.Response.StatusCode = (int)serverException.StatusCode;
            }

            var jsonResp = JsonSerializer.Serialize(
                new ProblemDetails
                {
                    Title = "An error occurred while processing your request",
                    Detail = exception.Message,
                    Instance = context.Request.Path
                }
            );

            await context.Response.WriteAsync(jsonResp);
        }
    }
}
