using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class CatalogTypeServiceTests
{
    private readonly IMapper _mapper;

    public CatalogTypeServiceTests()
    {
        // setup AutoMapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<Catalog.Host.Mapping.MappingProfile>();
        });
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Add_ReturnsNewItemId()
    {
        // arrange
        var mockRepository = new Mock<ICatalogTypeRepository>();
        mockRepository.Setup(repo => repo.Add(It.IsAny<string>())).ReturnsAsync(1);
        var service = new CatalogTypeService(
            dbContextWrapper: null,
            logger: Mock.Of<ILogger<BaseDataService<ApplicationDbContext>>>(),
            catalogTypeRepository: mockRepository.Object,
            mapper: _mapper
        );

        // act
        var result = await service.Add("TestBrand");

        // assert
        Assert.NotNull(result);
        Assert.Equal(1, result);
    }

    [Fact]
    public void Delete_ReturnsTrue()
    {
        // arrange
        var mockRepository = new Mock<ICatalogTypeRepository>();
        mockRepository.Setup(repo => repo.Delete(1)).Returns(true);
        var service = new CatalogTypeService(
            dbContextWrapper: null,
            logger: Mock.Of<ILogger<BaseDataService<ApplicationDbContext>>>(),
            catalogTypeRepository: mockRepository.Object,
            mapper: _mapper
        );

        // act
        var result = service.Delete(1);

        // assert
        Assert.True(result);
    }

    [Fact]
    public async Task Put_ReturnsUpdatedItemId()
    {
        // arrange
        var mockRepository = new Mock<ICatalogTypeRepository>();
        mockRepository.Setup(repo => repo.Put(It.IsAny<CatalogType>(), It.IsAny<int>())).ReturnsAsync(1);
        var service = new CatalogTypeService(
            dbContextWrapper: null,
            logger: Mock.Of<ILogger<BaseDataService<ApplicationDbContext>>>(),
            catalogTypeRepository: mockRepository.Object,
            mapper: _mapper
        );

        // act
        var result = await service.Put(new CatalogTypeDto { Type = "TestBrand" }, 1);

        // assert
        Assert.NotNull(result);
        Assert.Equal(1, result);
    }
}
