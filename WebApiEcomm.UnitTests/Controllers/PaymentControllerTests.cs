using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApiEcomm.API.Controllers;
using WebApiEcomm.Core.Entites.Basket;
using WebApiEcomm.Core.Services;

namespace WebApiEcomm.UnitTests.Controllers
{
    [TestFixture]
    public class PaymentControllerTests
    {
        private Mock<IPaymentService> _mockPaymentService;
        private PaymentController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockPaymentService = new Mock<IPaymentService>();
            _controller = new PaymentController(_mockPaymentService.Object);
        }

        [Test]
        public async Task Create_WhenBasketExists_ReturnsBasket()
        {
            // Arrange
            var basketId = "basket123";
            var deliveryId = 1;
            var basket = new CustomerBasket(basketId)
            {
                PaymentIntentId = "pi_123",
                ClientSecret = "secret_abc"
            };

            _mockPaymentService
                .Setup(s => s.CreateOrUpdatePaymentAsync(basketId, deliveryId))
                .ReturnsAsync(basket);

            // Act
            var result = await _controller.Create(basketId, deliveryId);

            // Assert
            Assert.That(result.Result, Is.Null); 
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.Id, Is.EqualTo(basketId));
            Assert.That(result.Value.PaymentIntentId, Is.EqualTo("pi_123"));
            Assert.That(result.Value.ClientSecret, Is.EqualTo("secret_abc"));
        }

        [Test]
        public async Task Create_WhenBasketDoesNotExist_ReturnsNull()
        {
            // Arrange
            var basketId = "basket123";
            var deliveryId = 1;

            _mockPaymentService
                .Setup(s => s.CreateOrUpdatePaymentAsync(basketId, deliveryId))
                .ReturnsAsync((CustomerBasket)null);

            // Act
            var result = await _controller.Create(basketId, deliveryId);

            // Assert
            Assert.That(result.Value, Is.Null);
        }

        [Test]
        public void Create_WhenServiceThrows_ExceptionBubblesUp()
        {
            // Arrange
            var basketId = "basket123";
            var deliveryId = 1;

            _mockPaymentService
                .Setup(s => s.CreateOrUpdatePaymentAsync(basketId, deliveryId))
                .ThrowsAsync(new Exception("Stripe error"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () =>
                await _controller.Create(basketId, deliveryId));
        }
    }
}
