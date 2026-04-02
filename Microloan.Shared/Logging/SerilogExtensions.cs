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
                .WriteTo.Console(new JsonFormatter()) 
                .WriteTo.File(new JsonFormatter(), $"logs/{appName}-.log", 
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7);

            if (!string.IsNullOrWhiteSpace(seqUri))
            {
                configuration.WriteTo.Seq(seqUri);
            }

            if (!string.IsNullOrWhiteSpace(elasticUri))
            {
                try
                {
                    configuration.WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(elasticUri))
                    {
                        AutoRegisterTemplate = true,
                        IndexFormat = $"{appName.ToLower()}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                        NumberOfReplicas = 1,
                        NumberOfShards = 2
                    });
                }
                catch
                {
                    // Ignore URI format exception if config is bad
                }
            }
        };
}
