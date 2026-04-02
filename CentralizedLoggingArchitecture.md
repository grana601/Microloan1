# Centralized Logging & Observability Architecture

This document provides a production-ready blueprint for centralized logging, error handling, and distributed tracing across your 5 Microservices + API Gateway using Serilog, Seq (and ELK/Grafana in the future).

## 📁 1. Recommended Folder Structure (The "Shared Library" Pattern)

Instead of duplicating middleware and Serilog configuration 6 times, we will create a shared NuGet package or Class Library called `Microloan.Shared` that all microservices will reference.

```text
Microloan1/
 ├── Microloan.Shared/
 │    ├── Logging/
 │    │    ├── SerilogExtensions.cs
 │    ├── Middlewares/
 │    │    ├── GlobalExceptionMiddleware.cs
 │    │    ├── CorrelationIdMiddleware.cs
 │    ├── Models/
 │    │    ├── ErrorDetails.cs
 ├── LoanApiGateway/
 ├── IdentityService/
 ├── CustomerService/
 ├── LoanService/
 ├── NotificationService/
 └── PaymentService/
```

## 📦 2. Required Packages
Every service (and the Shared project) needs these packages:
```bash
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Sinks.Seq
dotnet add package Serilog.Sinks.Elasticsearch # For ELK integration later
dotnet add package Serilog.Enrichers.Environment
dotnet add package Serilog.Enrichers.Process
dotnet add package Serilog.Enrichers.Thread
```

## 🧩 3. Reusable Middleware Code (in `Microloan.Shared`)

### A. The Error Model
```csharp
namespace Microloan.Shared.Models;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;

    public override string ToString() => System.Text.Json.JsonSerializer.Serialize(this);
}
```

### B. Correlation ID Middleware
Extracts an incoming `X-Correlation-Id` or generates a new one, logs it, and passes it forward.
```csharp
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
```

### C. Global Exception Handling Middleware
Catches all unhandled exceptions, safely masks sensitive internals, and logs the full stack trace with Serilog.
```csharp
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
            // Optional Bonus: Performance Logging (Time taken per request)
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
            Message = "An unexpected error occurred. Please try again later.", // Mask explicit exception details
            TraceId = traceId
        }.ToString());
    }
}
```

### D. Serilog Configuration Extension
```csharp
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Microloan.Shared.Logging;

public static class SerilogExtensions
{
    public static Action<HostBuilderContext, LoggerConfiguration> ConfigureSerilog =
        (context, configuration) =>
        {
            var appName = context.Configuration.GetValue<string>("ApplicationName") ?? "UnknownService";
            var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
            var seqUri = context.Configuration.GetValue<string>("SeqConfiguration:Uri");

            configuration
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", appName)
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .WriteTo.Console(new JsonFormatter()) // Structured JSON for kubernetes/docker logs
                .WriteTo.File(new JsonFormatter(), $"logs/{appName}-.log", 
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7);

            // Optional Seq Sink (Best for local dev/testing)
            if (!string.IsNullOrWhiteSpace(seqUri))
            {
                configuration.WriteTo.Seq(seqUri);
            }

            // Optional ElasticSearch Sink (Best for production ELK/Grafana)
            if (!string.IsNullOrWhiteSpace(elasticUri))
            {
                configuration.WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"{appName.ToLower()}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                    NumberOfReplicas = 1,
                    NumberOfShards = 2
                });
            }
        };
}
```

## ⚙️ 4. Integration into Microservices

You will apply this identically to **all 5 services + API Gateway**. 

### `appsettings.json`
Add this to every service to name it uniquely and point to Seq/Elastic.
```json
{
  "ApplicationName": "LoanService",
  "SeqConfiguration": {
    "Uri": "http://localhost:5341"
  },
  "ElasticConfiguration": {
    "Uri": "http://localhost:9200"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### `Program.cs`
Update your `Program.cs` to inject Serilog and use the shared middlewares.
```csharp
using Serilog;
using Microloan.Shared.Logging;
using Microloan.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// 1. Initialize Serilog via our Extension
builder.Host.UseSerilog(SerilogExtensions.ConfigureSerilog);

// ... Add your services ...
builder.Services.AddControllers();

var app = builder.Build();

// 2. Add Middlewares in CORRECT order
app.UseMiddleware<CorrelationIdMiddleware>();  // Must be first to generate correlation ID!
app.UseMiddleware<GlobalExceptionMiddleware>(); // Must be second to catch errors!

// 3. Optional: Native ASP.NET Core Request Logging using Serilog
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
});

// ... routing, auth, endpoints
app.MapControllers();

app.Run();
```

## 📡 5. API Gateway Specific Concerns (Ocelot)

Since you are using Ocelot, the API Gateway itself needs to pass the `X-Correlation-Id` header to downstream services.
Ocelot inherently forwards all custom headers, so as long as `CorrelationIdMiddleware` runs **before** `app.UseOcelot()`, the Header will flow from Postman -> Api Gateway -> Identity Service.

```csharp
// LoanApiGateway/Program.cs
app.UseMiddleware<CorrelationIdMiddleware>(); // Sets Header
app.UseMiddleware<GlobalExceptionMiddleware>(); 
app.UseSerilogRequestLogging(); 

// Wait for it...
await app.UseOcelot(); // Ocelot will capture X-Correlation-Id and forward it automatically!
```

## 🚀 6. Future Integration (ELK & Grafana Loki)

### Elasticsearch / Kibana
The setup provided above already writes to Elasticsearch via the `Serilog.Sinks.Elasticsearch` package.
Once you deploy Elasticsearch on Kubernetes or Docker, just update `"ElasticConfiguration": {"Uri": "http://your-elastic:9200"}` in your `appsettings.json`. Logs will auto-populate indices called `loanservice-logs-development-2026-04`.

### Grafana Loki
If you prefer Grafana over Kibana, simply swap the Elastic package for `Serilog.Sinks.Grafana.Loki`. Loki loves structured JSON output from stdout, so you can also just deploy Loki using Promtail to scrape the console outputs generated by `.WriteTo.Console(new JsonFormatter())`. 

## ✨ 7. Example JSON Log Output
```json
{
  "Timestamp": "2026-04-02T22:55:10.123456+05:30",
  "Level": "Information",
  "MessageTemplate": "User {UserId} created loan {LoanId}",
  "Properties": {
    "UserId": "f2ccdb8d-9bd3-4ad8-857f-d897d197afce",
    "LoanId": "10552",
    "CorrelationId": "9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d", // Tracked across all services!
    "Application": "LoanService",
    "Environment": "Development",
    "MachineName": "DESKTOP-DEV1",
    "ProcessId": 14502,
    "ThreadId": 12
  }
}
```

## 💡 Masking Sensitive Data (Bonus)
If you need to log requests/responses but want to avoid burning passwords or PII to disks, use the Destructurama.Attributed package.
1. `dotnet add package Destructurama.Attributed`
2. Add `.Destructure.UsingAttributes()` to the Serilog Logger config.
3. Place `[NotLogged]` attribute on your DTO password properties:
```csharp
public class LoginRequest
{
    public string Username { get; set; }
    
    [NotLogged] // Will appear as "*****" in logs
    public string Password { get; set; }
}
```
