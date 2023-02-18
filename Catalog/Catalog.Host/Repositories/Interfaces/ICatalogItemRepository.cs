using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
namespace Catalog.Host.Repositories.Interfaces;

public interface ICatalogItemRepository
{
    Task<PaginatedItems<CatalogItem>> GetByPageAsync(int pageIndex, int pageSize);
    Task<PaginatedItems<CatalogItem>> GetById(int id);
    Task<PaginatedItems<CatalogItem>> GetByBrand(int brandId);
    Task<PaginatedItems<CatalogItem>> GetByType(int typdId);
    Task<PaginatedItems<CatalogBrand>> GetBrands();
    Task<PaginatedItems<CatalogType>> GetTypes();
    Task<int?> Put(CatalogItem item, int id);
    bool Delete(int id);
    Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
}