using DistributedSystems.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace DistributedSystems.Server.Data.Repositories;

public class Repository<T> : IRepository<T> where T : BaseModel
{
    private readonly ApplicationContext DbContext;

    protected Repository(ApplicationContext dbContext) => DbContext = dbContext;

    public async Task<T?> Add(T entity)
    {
        await DbContext.AddAsync(entity);
        await DbContext.SaveChangesAsync();
        return await GetById(entity.Id);
    }

    public async Task<T?> GetById(long id)
    {
        return await DbContext.Set<T>().SingleOrDefaultAsync(entity => entity.Id == id /*&& !entity.IsDeleted*/);
    }

    public IQueryable<T> GetAll()
    {
        return DbContext.Set<T>().AsNoTracking()/*.Where(entity => !entity.IsDeleted)*/;
    }
}