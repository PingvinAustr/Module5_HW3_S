using Catalog.Host.Models.Dtos;
namespace Catalog.Host.Services.Interfaces;

public interface ICatalogItemService
{
    Task<int?> Add(string name, string description, decimal price, int availableStock, int catalogBrandId, int catalogTypeId, string pictureFileName);
    bool Delete(int id);
    Task<int?> Put(CatalogItemDto item, int itemToUpdate);
}