using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Interfaces;
using NotificationService.Services;

using NotificationService.Messaging;

using Serilog;
using Microloan.Shared.Logging;
using Microloan.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);
// Initialize Serilog
builder.Host.UseSerilog(SerilogExtensions.ConfigureSerilog);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<notificationdbcontext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<INotificationAppService, NotificationAppServiceImplementation>();
builder.Services.AddHostedService<RabbitMqConsumerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseSerilogRequestLogging();

app.UseAuthorization();
app.MapControllers();

app.Run();

