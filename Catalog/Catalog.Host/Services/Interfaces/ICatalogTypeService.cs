using Catalog.Host.Models.Dtos;
namespace Catalog.Host.Services.Interfaces;

public interface ICatalogTypeService
{
    Task<int?> Add(string brand);
    bool Delete(int id);
    Task<int?> Put(CatalogTypeDto item, int itemToUpdate);
}