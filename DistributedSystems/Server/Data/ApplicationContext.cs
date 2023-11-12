using DistributedSystems.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DistributedSystems.Server.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Link> Links { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql("Host=postgres;Database=db;Username=user;Password=1234");
}