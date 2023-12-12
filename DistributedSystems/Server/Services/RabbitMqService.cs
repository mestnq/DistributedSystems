using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace DistributedSystems.Server.Services;

public class RabbitMqService : IRabbitMqService
{
    public void SendMessage(object obj)
    {
        var message = JsonSerializer.Serialize(obj);
        SendMessage(message);
    }

    public void SendMessage(string message)
    {
        var factory = new ConnectionFactory
        {
            // HostName = "localhost",
            UserName = "rmuser",
            Password = "rmpassword"
        };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "test",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                routingKey: "test",
                basicProperties: null,
                body: body);
        }

        // var factory = new ConnectionFactory 
        // {
        //     // HostName = "localhost",
        //     UserName = "rmuser",
        //     Password = "rmpassword"
        // };
        // using var connection = factory.CreateConnection();
        // using var channel = connection.CreateModel();
        //
        // channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
        //
        // var body = Encoding.UTF8.GetBytes(message);
        // channel.BasicPublish(exchange: "logs",
        //     routingKey: string.Empty,
        //     basicProperties: null,
        //     body: body);
        // Console.WriteLine($" [x] Sent {message}");
    }
}