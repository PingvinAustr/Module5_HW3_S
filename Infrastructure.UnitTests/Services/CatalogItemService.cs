using AutoMapper;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.Host.Tests.Services
{
    public class CatalogItemServiceTests
    {
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapperMock;
        private readonly Mock<ILogger<BaseDataService<ApplicationDbContext>>> _loggerMock;
        private readonly Mock<ICatalogItemRepository> _catalogItemRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ICatalogItemService _catalogItemService;

        public CatalogItemServiceTests()
        {
            _dbContextWrapperMock = new Mock<IDbContextWrapper<ApplicationDbContext>>();
            _loggerMock = new Mock<ILogger<BaseDataService<ApplicationDbContext>>>();
            _catalogItemRepositoryMock = new Mock<ICatalogItemRepository>();
            _mapperMock = new Mock<IMapper>();

            _catalogItemService = new CatalogItemService(
                _dbContextWrapperMock.Object,
                _loggerMock.Object,
                _catalogItemRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Add_ValidItem_ReturnsNewItemId()
        {
            // Arrange
            var name = "Test Product";
            var description = "A test product";
            var price = 10.99m;
            var availableStock = 100;
            var catalogBrandId = 1;
            var catalogTypeId = 2;
            var pictureFileName = "test.jpg";
            var newItemId = 1;

            _catalogItemRepositoryMock.Setup(x => x.Add(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<decimal>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>()))
                .ReturnsAsync(newItemId);

            // Act
            var result = await _catalogItemService.Add(name, description, price, availableStock, catalogBrandId, catalogTypeId, pictureFileName);

            // Assert
            Assert.Equal(newItemId, result);
        }

        [Fact]
        public async Task Delete_ItemExists_ReturnsTrue()
        {
            // Arrange
            var itemId = 1;

            _catalogItemRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(true);

            // Act
            var result = _catalogItemService.Delete(itemId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Put_ItemExists_ReturnsNewItemId()
        {
            // Arrange
            var itemId = 1;
            var item = new CatalogItemDto { Name = "Test Product" };

            _mapperMock.Setup(x => x.Map<CatalogItem>(It.IsAny<CatalogItemDto>())).Returns(new CatalogItem());
            _catalogItemRepositoryMock.Setup(x => x.Put(
                    It.IsAny<CatalogItem>(),
                    It.IsAny<int>()))
                .ReturnsAsync(itemId);

            // Act
            var result = await _catalogItemService.Put(item, itemId);

            // Assert
            Assert.Equal(itemId, result);
        }
    }
}
