using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    // private static readonly string[] Summaries = new[]
    // {
    //     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    // };
    //
    // private readonly ILogger<WeatherForecastController> _logger;
    //
    // public WeatherForecastController(ILogger<WeatherForecastController> logger)
    // {
    //     _logger = logger;
    // }

    [HttpGet(Name = "Start")]
    public void Listen()
    {
        var factory = new ConnectionFactory
        {
            UserName = "rmuser",
            Password = "rmpassword",
            HostName = "localhost:5672"
        };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "test",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
        };
        channel.BasicConsume(queue: "test",
            autoAck: true,
            consumer: consumer);
        
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
        // // declare a server-named queue
        // var queueName = "test";
        // channel.QueueBind(queue: queueName,
        //     exchange: "logs",
        //     routingKey: string.Empty);
        //
        // Console.WriteLine(" [*] Waiting for logs.");
        //
        // var consumer = new EventingBasicConsumer(channel);
        // consumer.Received += (model, ea) =>
        // {
        //     byte[] body = ea.Body.ToArray();
        //     var message = Encoding.UTF8.GetString(body);
        //     Console.WriteLine($" [x] {message}");
        // };
        // channel.BasicConsume(queue: queueName,
        //     autoAck: true,
        //     consumer: consumer);
    }

    // [HttpGet(Name = "GetWeatherForecast")]
    // public IEnumerable<WeatherForecast> Get()
    // {
    //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //         {
    //             Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //             TemperatureC = Random.Shared.Next(-20, 55),
    //             Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //         })
    //         .ToArray();
    // }
}