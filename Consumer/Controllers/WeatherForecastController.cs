using System.Text;
using Consumer.Services;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private IHttpService _httpService;

    public WeatherForecastController(IHttpService httpService)
    {
        _httpService = httpService;
    }

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

        channel.QueueDeclare(
            queue: "links",
            durable: true,
            exclusive: false);
        
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();

            var message = Encoding.UTF8.GetString(body);
            
            Console.WriteLine(message);

            Task.Run(async () => await _httpService.UpdateHttpStatusLink(message.Replace("\"", "")));
        };

        channel.BasicConsume(queue: "links", autoAck: true, consumer: consumer);
    }
}