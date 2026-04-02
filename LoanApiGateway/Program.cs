using LoanApiGateway.Extensions;
using LoanApiGateway.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

using Serilog;
using Microloan.Shared.Logging;
using Microloan.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);
// Initialize Serilog
builder.Host.UseSerilog(SerilogExtensions.ConfigureSerilog);

// Add Ocelot JSON config
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCorsPolicy();

// 1. JWT Authentication Setup
var jwtConfig = builder.Configuration.GetSection("jwt");
var key = Encoding.UTF8.GetBytes(jwtConfig["key"] ?? throw new InvalidOperationException("JWT key is missing"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("Bearer", options =>
{
    options.RequireHttpsMetadata = false; // Note: false for dev, true for prod
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtConfig["issuer"],
        ValidateAudience = true,
        ValidAudience = jwtConfig["audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Configure Swagger for API Gateway API itself
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Ocelot
builder.Services.AddOcelot(builder.Configuration);

// Add SwaggerForOcelot integration
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();

// Error Handling Middleware
//app.UseGlobalExceptionHandler();

// Use CORS
app.UseCors("CorsPolicy");

// Enable Swagger UI and aggregation
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseSerilogRequestLogging();

app.UseAuthorization();

// Automatically assign ClientId for RateLimiting based on Token or IP
app.UseMiddleware<ClientIdMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Configure Ocelot to run
await app.UseOcelot();

app.Run();

