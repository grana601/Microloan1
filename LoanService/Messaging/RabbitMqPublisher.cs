using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace LoanService.Messaging;

public interface IRabbitMqPublisher
{
    Task PublishEventAsync<T>(T @event, string queueName) where T : class;
}

public class RabbitMqPublisher : IRabbitMqPublisher
{
    private readonly IConfiguration _configuration;
    private readonly string _hostname;
    private readonly string _username;
    private readonly string _password;

    public RabbitMqPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
        _hostname = _configuration["RabbitMQ:HostName"] ?? "localhost";
        _username = _configuration["RabbitMQ:UserName"] ?? "guest";
        _password = _configuration["RabbitMQ:Password"] ?? "guest";
    }

    public async Task PublishEventAsync<T>(T @event, string queueName) where T : class
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostname,
            UserName = _username,
            Password = _password
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var message = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(message);

        var properties = new BasicProperties
        {
            Persistent = true
        };

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: queueName,
            mandatory: false,
            basicProperties: properties,
            body: body);
    }
}

