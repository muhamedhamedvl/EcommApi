using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Product;
using WebApiEcomm.Core.Interfaces;
using WebApiEcomm.Core.Services;
using WebApiEcomm.Core.Sharing;
using WebApiEcomm.InfraStructure.Data;
using WebApiEcomm.InfraStructure.Repositories;

namespace WebApiEcomm.UnitTests
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private AppDbContext _dbContext;
        private ProductRepository _repo;
        private Mock<IMapper> _mapperMock;
        private Mock<IImageManagementService> _imageServiceMock;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(System.Guid.NewGuid().ToString())
                .Options;
            _dbContext = new AppDbContext(options);

            _mapperMock = new Mock<IMapper>();
            _imageServiceMock = new Mock<IImageManagementService>();

            _repo = new ProductRepository(_dbContext, _mapperMock.Object, _imageServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task AddAsync_ShouldAddProduct()
        {
            var addDto = new AddProductDto
            {
                Name = "Product1",
                Description = "Desc",
                NewPrice = 100m,
                OldPrice = 50m,
                CategoryId = 1,
                Photo = new FormFileCollection() 
            };

            var productEntity = new Product
            {
                Name = addDto.Name,
                Description = addDto.Description,
                NewPrice = addDto.NewPrice,
                OldPrice = addDto.OldPrice,
                CategoryId = addDto.CategoryId
            };

            _mapperMock.Setup(m => m.Map<Product>(addDto)).Returns(productEntity);
            _imageServiceMock.Setup(s => s.AddImageAsync(addDto.Photo, addDto.Name)).ReturnsAsync(new List<string>());

            var result = await _repo.AddAsync(addDto);

            Assert.That(result, Is.True);
            Assert.That(_dbContext.Products.Count(), Is.EqualTo(1));
            Assert.That(_dbContext.Products.First().Name, Is.EqualTo("Product1"));
        }
    }
}
