using System.Text;
using DistributedSystems.Server.Data;
using DistributedSystems.Server.Data.Entities;
using DistributedSystems.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace DistributedSystems.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class LinksController : ControllerBase
{
    private readonly ApplicationContext context;
    private readonly IRabbitMqService mqService;

    public LinksController(ApplicationContext context, IRabbitMqService mqService)
    {
        this.mqService = mqService;
        this.context = context;
    }

    // POST /links — сохраняет ссылку в БД и возвращает ее id
    [HttpPost("add")]
    public async Task<ActionResult<Link>> CreateLinks([FromBody] string url)
    {
        var link = new Link
        {
            Id = Guid.NewGuid(),
            Url = url,
            Status = "NOT refresh!"
        };
        context.Links.Add(link);
        await context.SaveChangesAsync();
        return CreatedAtAction("GetLink", new { id = link.Id }, link);
    }

    // GET /links/ — отдает ссылку из БД по id
    [HttpGet("current")]
    public async Task<ActionResult<Link>> GetLink([FromQuery] string id)
    {
        var guidId = Guid.Parse(id);
        var link = await context.Links.FindAsync(guidId);
        return link == null ? NotFound() : link;
    }

    // PUT /links/ для обновления статуса ссылки
    [HttpPut("refresh-status")]
    public async Task<ActionResult<Link>> PutStatusLink([FromQuery] Guid id)
    {
        var link = await context.Links.FindAsync(id);
        if (link == null)
            return NotFound();

        link.Status = "refresh!";
        await context.SaveChangesAsync();


        var integrationEventData = JsonConvert.SerializeObject(new
        {
            id = link.Id,
            url = link.Url,
            newStatus = link.Status
        });
        mqService.SendMessage(integrationEventData);

        return CreatedAtAction("GetLink", new { id = link.Id }, link);
    }
}