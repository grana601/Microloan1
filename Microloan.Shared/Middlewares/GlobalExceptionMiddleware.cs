using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microloan.Shared.Models;
using System.Net;

namespace Microloan.Shared.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            await _next(context);
            
            watch.Stop();
            if (watch.ElapsedMilliseconds > 1000) 
            {
                _logger.LogWarning("Long running request: {Path} took {ElapsedMilliseconds} ms", 
                    context.Request.Path, watch.ElapsedMilliseconds);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled an exception occurred while processing {Path} [{Method}]",
                context.Request.Path, context.Request.Method);
                
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var traceId = context.Response.Headers["X-Correlation-Id"].FirstOrDefault() ?? Guid.NewGuid().ToString();

        return context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = "An unexpected error occurred. Please try again later.",
            TraceId = traceId
        }.ToString());
    }
}
