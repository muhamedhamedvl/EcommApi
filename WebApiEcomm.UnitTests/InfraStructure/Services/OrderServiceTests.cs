using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Basket;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Order;
using WebApiEcomm.Core.Entites.Product;
using WebApiEcomm.Core.Interfaces.IUnitOfWork;
using WebApiEcomm.Core.Services;
using WebApiEcomm.InfraStructure.Data;
using WebApiEcomm.InfraStructure.Repositores.Service;

namespace WebApiEcomm.UnitTests.InfraStructure.Services
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IPaymentService> _paymentServiceMock;
        private AppDbContext _dbContext;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new AppDbContext(options);

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _paymentServiceMock = new Mock<IPaymentService>();

            _order_service_ctor_setup();
        }
        private void _order_service_ctor_setup()
        {
            _orderService = new OrderService(
                _unitOfWorkMock.Object,
                _dbContext,
                _mapperMock.Object,
                _payment_service_or_default()
            );
        }

        private IPaymentService _payment_service_or_default()
        {
            return _paymentServiceMock.Object;
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                _dbContext.Database.EnsureDeleted();
            }
            catch {}
            _dbContext.Dispose();
        }

        [Test]
        public async Task CreateOrdersAsync_ShouldCreateOrderAndDeleteBasket()
        {
            // Arrange
            var basketId = "basket123";
            var buyerEmail = "test@example.com";
            var orderDto = new OrderDto
            {
                DelliveryMethodId = 1,
                basketId = basketId,
                shipaddressdto = new ShippingAddressDto("John", "Doe", "City", "12345", "Street", "State"),
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto(1, "Product1", "img1.jpg", 10.0m, 2)
                }
            };

            var basket = new CustomerBasket(basketId)
            {
                basketItems = new List<BasketItems>
                {
                    new BasketItems { Id = 1, Image = "img1.jpg", Price = 10, Quantity = 2 }
                },
                PaymentIntentId = "intent123"
            };

            var product = new Product { Id = 1, Name = "Product1", NewPrice = 10.0m };
            var deliveryMethod = new DeliveryMethod("Standard", "2 days", "desc", 5.0m);

            var shipAddress = new ShippingAddress("John", "Doe", "City", "12345", "Street", "State");


            _unitOfWorkMock.Setup(u => u.CustomerBasketRepository.GetCustomerBasketAsync(basketId))
                .ReturnsAsync(basket);
            _unitOfWorkMock.Setup(u => u.ProductRepository.GetByIdAsync(1))
                .ReturnsAsync(product);


            _dbContext.DeliveryMethods.Add(deliveryMethod);
            _dbContext.SaveChanges();

            _mapperMock.Setup(m => m.Map<ShippingAddress>(orderDto.shipaddressdto))
                .Returns(shipAddress);

            _unitOfWorkMock.Setup(u => u.CustomerBasketRepository.DeleteCustomerBasketAsync(basketId))
                .ReturnsAsync(true);

            // Act
            var result = await _orderService.CreateOrdersAsync(orderDto, buyerEmail);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.BuyerEmail, Is.EqualTo(buyerEmail));
            Assert.That(result.SubTotal, Is.EqualTo(20.0m)); // 10 * 2
            Assert.That(result.deliveryMethod.Id, Is.EqualTo(deliveryMethod.Id));
            Assert.That(result.PaymentIntentId, Is.EqualTo("intent123"));


            _unitOfWorkMock.Verify(u => u.CustomerBasketRepository.GetCustomerBasketAsync(basketId), Times.Once);
            _unitOfWorkMock.Verify(u => u.ProductRepository.GetByIdAsync(It.IsAny<int>()), Times.AtLeastOnce);
            _unitOfWorkMock.Verify(u => u.CustomerBasketRepository.DeleteCustomerBasketAsync(basketId), Times.Once);
        }
   

        [Test]
        public async Task GetAllOrdersForUserAsync_ShouldReturnOrdersForUser()
        {
            // Arrange
            var buyerEmail = "user@example.com";
            var order = new Order(buyerEmail, 100.0m, new ShippingAddress("A", "B", "C", "D", "E", "F"), null, new List<OrderItem>(), "intent1");
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            var orderDtoList = new List<OrderToReturnDTO>
            {
                new OrderToReturnDTO { Id = order.Id, BuyerEmail = buyerEmail, SubTotal = 100.0m }
            };

            _mapperMock.Setup(m => m.Map<IReadOnlyList<OrderToReturnDTO>>(It.IsAny<List<Order>>()))
                .Returns(orderDtoList);

            // Act
            var result = await _orderService.GetAllOrdersForUserAsync(buyerEmail);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].BuyerEmail, Is.EqualTo(buyerEmail));

            // Verify mapping called
            _mapperMock.Verify(m => m.Map<IReadOnlyList<OrderToReturnDTO>>(It.IsAny<List<Order>>()), Times.Once);
        }

        [Test]
        public async Task GetOrderByIdAsync_ShouldReturnOrderForUser()
        {
            // Arrange
            var buyerEmail = "user2@example.com";
            var order = new Order(buyerEmail, 50.0m, new ShippingAddress("A", "B", "C", "D", "E", "F"), null, new List<OrderItem>(), "intent2");
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            var orderToReturn = new OrderToReturnDTO { Id = order.Id, BuyerEmail = buyerEmail, SubTotal = 50.0m };
            _mapperMock.Setup(m => m.Map<OrderToReturnDTO>(It.IsAny<Order>()))
                .Returns(orderToReturn);

            // Act
            var result = await _orderService.GetOrderByIdAsync(order.Id, buyerEmail);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(order.Id));
            Assert.That(result.BuyerEmail, Is.EqualTo(buyerEmail));

            _mapperMock.Verify(m => m.Map<OrderToReturnDTO>(It.IsAny<Order>()), Times.Once);
        }

        [Test]
        public async Task GetDeliveryMethodAsync_ShouldReturnDeliveryMethods()
        {
            // Arrange
            var deliveryMethod = new DeliveryMethod("Express", "1 day", "fast", 15.0m);
            _dbContext.DeliveryMethods.Add(deliveryMethod);
            _dbContext.SaveChanges();

            // Act
            var result = await _orderService.GetDeliveryMethodAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("Express"));
        }
    }
}
