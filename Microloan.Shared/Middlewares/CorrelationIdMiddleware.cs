using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Microloan.Shared.Middlewares;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[CorrelationIdHeaderName].FirstOrDefault() 
                            ?? Guid.NewGuid().ToString();

        // Ensure it's in the response headers
        if (!context.Response.Headers.ContainsKey(CorrelationIdHeaderName))
        {
            context.Response.Headers.Append(CorrelationIdHeaderName, correlationId);
        }

        // Push CorrelationId into Serilog context so ALL logs in this request share it
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}
