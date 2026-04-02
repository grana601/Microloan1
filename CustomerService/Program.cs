using CustomerService.Data;
using CustomerService.Interfaces;
using CustomerService.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microloan.Shared.Logging;
using Microloan.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Initialize Serilog
builder.Host.UseSerilog(SerilogExtensions.ConfigureSerilog);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework Core with PostgreSQL
builder.Services.AddDbContext<customerdbcontext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register DI
builder.Services.AddScoped<ICustomerService, CustomerAppService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global Logging & Exception Handling
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseSerilogRequestLogging();

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
