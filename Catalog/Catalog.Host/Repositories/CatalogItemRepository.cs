using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Catalog.Host.Repositories;

public class CatalogItemRepository : ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CatalogItemRepository> _logger;

    public CatalogItemRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogItemRepository> logger)
    {
        _dbContext = dbContextWrapper.DbContext;
        _logger = logger;
    }

    public async Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize)
    {
        var totalItems = await _dbContext.CatalogItems
            .LongCountAsync();

        var itemsOnPage = await _dbContext.CatalogItems
            .Include(i => i.CatalogBrand)
            .Include(i => i.CatalogType)
            .OrderBy(c => c.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>() { TotalCount = totalItems, Data = itemsOnPage };
    }

    public async Task<PaginatedItems<CatalogItem>> GetById(int id)
    {
        var item = await _dbContext.CatalogItems.Where(x => x.Id == id).ToListAsync();
        return new PaginatedItems<CatalogItem>() { TotalCount = 1, Data = item };
    }

    public async Task<PaginatedItems<CatalogItem>> GetByBrand(int brand)
    {
        var items = await _dbContext.CatalogItems.Where(x => x.CatalogBrand.Id == brand).ToListAsync();
        return new PaginatedItems<CatalogItem>() { TotalCount = items.Count, Data = items };
    }

    public async Task<PaginatedItems<CatalogItem>> GetByType(int typeId)
    {
        var items = await _dbContext.CatalogItems.Where(x => x.CatalogType.Id == typeId).ToListAsync();
        return new PaginatedItems<CatalogItem>() { Data = items, TotalCount = items.Count };
    }

    public async Task<PaginatedItems<CatalogBrand>> GetBrands()
    {
        var items = await _dbContext.CatalogBrands.ToListAsync();
        return new PaginatedItems<CatalogBrand>() { Data = items, TotalCount = items.Count };
    }

    public async Task<PaginatedItems<CatalogType>> GetTypes()
    {
        var items = await _dbContext.CatalogTypes.ToListAsync();
        return new PaginatedItems<CatalogType>() { Data = items, TotalCount = items.Count };
    }

    public async Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName)
    {
        var item = await _dbContext.AddAsync(new CatalogItem
        {
            CatalogBrandId = catalogBrandId,
            CatalogTypeId = catalogTypeId,
            Description = description,
            Name = name,
            PictureFileName = pictureFileName,
            Price = price
        });

        await _dbContext.SaveChangesAsync();

        return item.Entity.Id;
    }

    public bool Delete(int id)
    {
        try
        {
            var result = _dbContext.CatalogItems.ToList().RemoveAll(x => x.Id == id);
            _dbContext.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<int?> Put(CatalogItem item, int id)
    {
        _dbContext.CatalogItems.Where(x => x.Id == id).ToList().ForEach(x =>
        {
            x.Name = item.Name;
            x.Description = item.Description;
            x.Price = item.Price;
            x.AvailableStock = item.AvailableStock;
            x.CatalogType = item.CatalogType;
            x.PictureFileName = item.PictureFileName;
            x.CatalogBrand = item.CatalogBrand;
        });
        await _dbContext.SaveChangesAsync();
        return id;
    }
}