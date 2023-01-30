using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Catalog.Host.Repositories;

public class CatalogBrandRepository : ICatalogBrandRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogBrandRepository> _logger;

    public CatalogBrandRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogBrandRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<int?> Add(string brand)
    {
        var item = await _dbContext.AddAsync(new CatalogBrand { Brand = brand });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public bool Delete(int id)
    {
        try
        {
            var result = _dbContext.CatalogBrands.ToList().RemoveAll(x => x.Id == id);
            _dbContext.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<int?> Put(CatalogBrand item, int id)
    {
        _dbContext.CatalogBrands.Where(x => x.Id == id).ToList().ForEach(x =>
        {
            x.Brand = item.Brand;
        });
        await _dbContext.SaveChangesAsync();
        return id;
    }
}