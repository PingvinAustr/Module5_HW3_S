using Catalog.Host.Models.Dtos;
namespace Catalog.Host.Services.Interfaces;

public interface ICatalogBrandService
{
    Task<int?> Add(string brand);
    bool Delete(int id);
    Task<int?> Put(CatalogBrandDto item, int itemToUpdate);
}