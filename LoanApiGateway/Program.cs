using LoanApiGateway.Extensions;
using LoanApiGateway.Middleware;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add Ocelot JSON config
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCorsPolicy();

// Configure Swagger for API Gateway API itself
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Ocelot
builder.Services.AddOcelot(builder.Configuration);

// Add SwaggerForOcelot integration
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();

// Error Handling Middleware
app.UseGlobalExceptionHandler();

// Use CORS
app.UseCors("CorsPolicy");

// Enable Swagger UI and aggregation
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Configure Ocelot to run
await app.UseOcelot();

app.Run();
