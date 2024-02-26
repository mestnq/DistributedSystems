using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet(Name = "Start")]
    public void Listen() //TODO: РЕФАКТОРИНГ!!!
    {
        var factory = new ConnectionFactory()
        {
            HostName = "rabbitmq",
            UserName = "rmuser",
            Password = "rmpassword",
            VirtualHost = "/"
        };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        var queueName = "links";
        
        var s = channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false);
        
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] {message}");
        };
        channel.BasicConsume(queue: queueName,
            autoAck: true,
            consumer: consumer);
    }
}