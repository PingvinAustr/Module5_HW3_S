using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Catalog.Host.Repositories;

public class CatalogTypeRepository : ICatalogTypeRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogTypeRepository> _logger;

    public CatalogTypeRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogTypeRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> Add(string type)
    {
        var item = await _dbContext.AddAsync(new CatalogType { Type = type });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public bool Delete(int id)
    {
        try
        {
            var result = _dbContext.CatalogTypes.ToList().RemoveAll(x => x.Id == id);
            _dbContext.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<int?> Put(CatalogType item, int id)
    {
        _dbContext.CatalogTypes.Where(x => x.Id == id).ToList().ForEach(x =>
        {
            x.Type = item.Type;
        });
        await _dbContext.SaveChangesAsync();
        return id;
    }
}