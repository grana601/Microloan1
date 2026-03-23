using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using NotificationService.Messaging.Events;

namespace NotificationService.Messaging;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly ILogger<RabbitMqConsumerService> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;
    private string _queueName;

    public RabbitMqConsumerService(ILogger<RabbitMqConsumerService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _queueName = _configuration["RabbitMQ:QueueName"] ?? "LoanCreatedEvent";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
            UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        try
        {
            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.QueueDeclareAsync(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                try
                {
                    var @event = JsonSerializer.Deserialize<LoanCreatedEvent>(message);
                    if (@event != null)
                    {
                        _logger.LogInformation("Processing LoanCreatedEvent for LoanId: {LoanId}", @event.loanid);
                        
                        // Implement notification logic (email, SMS, etc.)
                        _logger.LogInformation("Successfully sent notification for Loan: {LoanId}", @event.loanid);
                    }
                    
                    await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                    // Depending on logic, nack or requeue
                    await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: _queueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            // Wait indefinitely until cancellation
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start RabbitMQ consumer.");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null)
        {
             await _channel.CloseAsync(cancellationToken);
             await _channel.DisposeAsync();
        }
        if (_connection != null)
        {
             await _connection.CloseAsync(cancellationToken);
             await _connection.DisposeAsync();
        }

        await base.StopAsync(cancellationToken);
    }
}
