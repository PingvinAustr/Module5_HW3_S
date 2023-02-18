using Catalog.Host.Data;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Data.Entities;

using AutoMapper;

namespace Catalog.Host.Services;

public class CatalogTypeService : BaseDataService<ApplicationDbContext>, ICatalogTypeService
{
    private readonly ICatalogTypeRepository _catalogTypeRepository;
    private readonly IMapper _mapper;

    public CatalogTypeService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogTypeRepository catalogTypeRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogTypeRepository = catalogTypeRepository;
        _mapper = mapper;
    }

    public Task<int?> Add(string brand)
    {
        return ExecuteSafeAsync(() => _catalogTypeRepository.Add(brand));
    }

    public bool Delete(int itemId)
    {
        return _catalogTypeRepository.Delete(itemId);
    }

    public Task<int?> Put(CatalogTypeDto item, int itemToUpdate)
    {
        var catalogItem = _mapper.Map<CatalogType>(item);
        return ExecuteSafeAsync(() => _catalogTypeRepository.Put(catalogItem, itemToUpdate));
    }
}