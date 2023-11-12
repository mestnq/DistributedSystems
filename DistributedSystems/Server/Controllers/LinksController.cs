using DistributedSystems.Server.Data;
using DistributedSystems.Server.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DistributedSystems.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class LinksController : ControllerBase
{
    private readonly ApplicationContext context;

    public LinksController(ApplicationContext context)
    {
        this.context = context;
    }

    // POST /links — сохраняет ссылку в БД и возвращает ее id
    [HttpPost("add")]
    public async Task<ActionResult<Link>> CreateLinks([FromBody] string url)
    {
        var link = new Link
        {
            Id = Guid.NewGuid(),
            Url = url
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
}