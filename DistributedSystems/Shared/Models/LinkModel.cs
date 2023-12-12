namespace DistributedSystems.Shared.Models;

public record LinkModel() : BaseModel
{
    public required string Name { get; set; }
}