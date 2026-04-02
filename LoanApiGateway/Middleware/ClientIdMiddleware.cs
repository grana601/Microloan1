using System.Security.Claims;

namespace LoanApiGateway.Middleware
{
    public class ClientIdMiddleware
    {
        private readonly RequestDelegate _next;

        public ClientIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1. First, check if the user is authenticated and has a NameIdentifier (or id)
            var clientId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // 2. Fallback to IP address if not authenticated
            if (string.IsNullOrEmpty(clientId))
            {
                clientId = context.Connection.RemoteIpAddress?.ToString();
            }

            // 3. Final fallback
            if (string.IsNullOrEmpty(clientId))
            {
                clientId = "unknown-client";
            }

            // Append the ClientId header so Ocelot's Rate Limiter can use it
            if (!context.Request.Headers.ContainsKey("ClientId"))
            {
                context.Request.Headers.Append("ClientId", clientId);
            }

            await _next(context);
        }
    }
}
