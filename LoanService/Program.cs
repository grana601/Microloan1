using LoanService.Data;
using LoanService.Interfaces;
using LoanService.Services;
using LoanService.Services.External;
using LoanService.Messaging;
using Microsoft.EntityFrameworkCore;

using Serilog;
using Microloan.Shared.Logging;
using Microloan.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);
// Initialize Serilog
builder.Host.UseSerilog(SerilogExtensions.ConfigureSerilog);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<loandbcontext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// External Service Clients
builder.Services.AddHttpClient<IIdentityServiceClient, IdentityServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:IdentityService"] ?? "https://localhost:5001");
});

builder.Services.AddHttpClient<ICustomerServiceClient, CustomerServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CustomerService"] ?? "https://localhost:5003");
});

// RabbitMQ
builder.Services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();

builder.Services.AddScoped<ILoanService, LoanAppService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseSerilogRequestLogging();

app.UseAuthorization();
app.MapControllers();

app.Run();

