using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Data.Entities;

using AutoMapper;
using Moq;
using Xunit;

namespace Catalog.Host.UnitTests.Services
{
    public class CatalogBrandServiceTests
    {
        private readonly Mock<ICatalogBrandRepository> _catalogBrandRepositoryMock;
        private readonly IMapper _mapper;
        private readonly ICatalogBrandService _catalogBrandService;

        public CatalogBrandServiceTests()
        {
            _catalogBrandRepositoryMock = new Mock<ICatalogBrandRepository>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new Catalog.Host.Mapping.MappingProfile())).CreateMapper();
            _catalogBrandService = new CatalogBrandService(null, null, _catalogBrandRepositoryMock.Object, _mapper);
        }

        [Fact]
        public void Add_ValidBrand_ReturnsNewBrandId()
        {
            // Arrange
            var brandName = "NewBrand";
            _catalogBrandRepositoryMock.Setup(x => x.Add(It.IsAny<string>())).ReturnsAsync(1);

            // Act
            var result = _catalogBrandService.Add(brandName).Result;

            // Assert
            Assert.Equal(1, result);
            _catalogBrandRepositoryMock.Verify(x => x.Add(brandName), Times.Once);
        }

        [Fact]
        public void Delete_ExistingBrand_ReturnsTrue()
        {
            // Arrange
            var brandId = 1;
            _catalogBrandRepositoryMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(true);

            // Act
            var result = _catalogBrandService.Delete(brandId);

            // Assert
            Assert.True(result);
            _catalogBrandRepositoryMock.Verify(x => x.Delete(brandId), Times.Once);
        }

        [Fact]
        public void Put_ValidBrand_ReturnsUpdatedBrandId()
        {
            // Arrange
            var brandToUpdateId = 1;
            var brandToUpdate = new CatalogBrandDto { Id = brandToUpdateId, Brand = "BrandToUpdate" };
            var updatedBrand = new CatalogBrand { Id = brandToUpdateId, Brand = "UpdatedBrand" };
            _catalogBrandRepositoryMock.Setup(x => x.Put(It.IsAny<CatalogBrand>(), It.IsAny<int>())).ReturnsAsync(brandToUpdateId);

            // Act
            var result = _catalogBrandService.Put(brandToUpdate, brandToUpdateId).Result;

            // Assert
            Assert.Equal(brandToUpdateId, result);
            _catalogBrandRepositoryMock.Verify(x => x.Put(updatedBrand, brandToUpdateId), Times.Once);
        }
    }
}
