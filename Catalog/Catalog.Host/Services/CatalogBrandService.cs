using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Data.Entities;

using AutoMapper;

namespace Catalog.Host.Services;

public class CatalogBrandService : BaseDataService<ApplicationDbContext>, ICatalogBrandService
{
    private readonly ICatalogBrandRepository _catalogBrandRepository;
    private readonly IMapper _mapper;

    public CatalogBrandService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogBrandRepository catalogBrandRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogBrandRepository = catalogBrandRepository;
        _mapper = mapper;
    }

    public Task<int?> Add(string brand)
    {
        return ExecuteSafeAsync(() => _catalogBrandRepository.Add(brand));
    }

    public bool Delete(int itemId)
    {
        return _catalogBrandRepository.Delete(itemId);
    }

    public Task<int?> Put(CatalogBrandDto item, int itemToUpdate)
    {
        var catalogItem = _mapper.Map<CatalogBrand>(item);
        return ExecuteSafeAsync(() => _catalogBrandRepository.Put(catalogItem, itemToUpdate));
    }
}