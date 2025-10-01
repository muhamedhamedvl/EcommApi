using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using WebApiEcomm.API.Controllers;
using WebApiEcomm.Core.Entites.Product;
using WebApiEcomm.Core.Interfaces;

using WebApiEcomm.Core.Interfaces.IUnitOfWork;

namespace WebApiEcomm.UnitTests.API.Controllers
{
    [TestFixture]
    public class bugControllerTests
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<ICategoryRepository> _catRepoMock;
        private Mock<IMapper> _mapperMock;
        private bugController _controller;

        [SetUp]
        public void SetUp()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _catRepoMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();

            _uowMock.Setup(u => u.CategoryRepository).Returns(_catRepoMock.Object);
            _controller = new bugController(_uowMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetNotFound_WhenCategoryIsNull_ReturnsNotFound()
        {
            // Arrange
            _catRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                        .ReturnsAsync((Category)null);

            // Act
            var result = await _controller.GetNotFound();

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetNotFound_WhenCategoryExists_ReturnsOkWithCategory()
        {
            // Arrange
            var category = new Category { Id = 100, Name = "Electronics" };
            _catRepoMock.Setup(r => r.GetByIdAsync(100)).ReturnsAsync(category);

            // Act
            var result = await _controller.GetNotFound();

            // Assert
            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.Value, Is.SameAs(category));
        }

        [Test]
        public void GetServerError_WhenCategoryIsNull_ThrowsNullReferenceException()
        {
            // Arrange
            _catRepoMock.Setup(r => r.GetByIdAsync(100))
                        .ReturnsAsync((Category)null);

            // Act & Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await _controller.GetServerError());
        }

        [Test]
        public async Task GetServerError_WhenCategoryExists_SetsNameEmptyAndReturnsOk()
        {
            // Arrange
            var category = new Category { Id = 100, Name = "Before" };
            _catRepoMock.Setup(r => r.GetByIdAsync(100)).ReturnsAsync(category);

            // Act
            var result = await _controller.GetServerError();

            // Assert
            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.Value, Is.SameAs(category));
            Assert.That(category.Name, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task GetBadRequest_NoParam_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.GetBadRequest();

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task GetBadRequest_WithParam_ReturnsOk()
        {
            // Act
            var result = await _controller.GetBadRequest(5);

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
        }
    }
}
