using FluentValidation;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace API.Middlewares;

public class ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            KeyNotFoundException => HttpStatusCode.NotFound,
            InvalidOperationException => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = (int)statusCode;

        var trace = new StackTrace(exception, true);
        var frame = trace.GetFrames()?.FirstOrDefault(f => f.GetFileLineNumber() != 0);

        var fileName = frame?.GetFileName();
        var lineNumber = frame?.GetFileLineNumber();

        _logger.LogError(exception,
            """
            Exception caught
            Type: {ExceptionType}
            Message: {Message}
            File: {File}
            Line: {Line}
            Path: {Path}
            Method: {Method}
            """,
            exception.GetType().Name,
            exception.Message,
            fileName,
            lineNumber,
            context.Request.Path,
            context.Request.Method
        );

        object response = exception switch
        {
            ValidationException ve => new
            {
                message = "Validation failed",
                errors = ve.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray())
            },

            _ => new
            {
                message = "An unexpected error occurred. Please try again later."
            }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
