using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class CatalogServiceTests
{
    private readonly IMapper _mapper;

    public CatalogServiceTests()
    {
        // setup AutoMapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<Catalog.Host.Mapping.MappingProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetCatalogItemsAsync_ReturnsPaginatedItemsResponse()
    {
        // arrange
        var mockRepository = new Mock<ICatalogItemRepository>();
        mockRepository.Setup(repo => repo.GetByPageAsync(0, 10)).ReturnsAsync(new PaginatedItemsDto<CatalogItemDto>
        {
            Data = new List<CatalogItemDto>
            {
                new CatalogItemDto { Id = 1, Name = "Item 1", Price = 10 },
                new CatalogItemDto { Id = 2, Name = "Item 2", Price = 20 }
            },
            TotalCount = 2
        });
        var service = new CatalogService(
            dbContextWrapper: null,
            logger: Mock.Of<ILogger<BaseDataService<ApplicationDbContext>>>(),
            catalogItemRepository: mockRepository.Object,
            mapper: _mapper
        );

        // act
        var result = await service.GetCatalogItemsAsync(10, 0);

        // assert
        Assert.IsType<PaginatedItemsResponse<CatalogItemDto>>(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(0, result.PageIndex);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(2, result.Data.Count);
        Assert.Equal("Item 1", result.Data[0].Name);
        Assert.Equal("Item 2", result.Data[1].Name);
    }

    [Fact]
    public async Task GetById_ReturnsPaginatedItemsResponse()
    {
        // arrange
        var mockRepository = new Mock<ICatalogItemRepository>();
        mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(new PaginatedItemsDto<CatalogItemDto>
        {
            Data = new List<CatalogItemDto>
            {
                new CatalogItemDto { Id = 1, Name = "Item 1", Price = 10 }
            },
            TotalCount = 1
        });
        var service = new CatalogService(
            dbContextWrapper: null,
            logger: Mock.Of<ILogger<BaseDataService<ApplicationDbContext>>>(),
            catalogItemRepository: mockRepository.Object,
            mapper: _mapper
        );

        // act
        var result = await service.GetById(1);

        // assert
        Assert.IsType<PaginatedItemsResponse<CatalogItemDto>>(result);
        Assert.Equal(1, result.Count);
        Assert.Equal(0, result.PageIndex);
        Assert.Equal(0, result.PageSize);
        Assert.Single(result.Data);
        Assert.Equal("Item 1", result.Data[0].Name);
    }
