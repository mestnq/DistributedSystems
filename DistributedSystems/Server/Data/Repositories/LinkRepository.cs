using DistributedSystems.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace DistributedSystems.Server.Data.Repositories;

public class LinkRepository : Repository<LinkModel>
{
    protected LinkRepository(ApplicationContext dbContext) : base(dbContext)
    {
    }
    
    

    // public async ValueTask<LinkModel?> GetLink(long id) => await GetById(id);
    //
    // public async ValueTask<IReadOnlyCollection<LinkModel>> GetLinks()
    // {
    //     return await GetAll()
    //         //.Include()
    //         .ToListAsync();
    // }
    //
    // public async ValueTask<LinkModel?> AddLink(LinkModel link) => await Add(link);
}