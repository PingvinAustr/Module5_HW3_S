using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogItemDto>> GetCatalogItemsAsync(int pageSize, int pageIndex);
    Task<PaginatedItemsResponse<CatalogItemDto>> GetById(int id);
    Task<PaginatedItemsResponse<CatalogItemDto>> GetByBrand(int brandId);
    Task<PaginatedItemsResponse<CatalogItemDto>> GetByType(int typeId);
    Task<PaginatedItemsResponse<CatalogBrandDto>> GetBrands();
    Task<PaginatedItemsResponse<CatalogTypeDto>> GetTypes();
}