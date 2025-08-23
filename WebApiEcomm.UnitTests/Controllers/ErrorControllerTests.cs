using NUnit.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiEcomm.API.Controllers;

namespace WebApiEcomm.UnitTests.Controllers
{
    [TestFixture]
    public class ErrorControllerTests
    {
        private ErrorController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new ErrorController();
        }

        [Test]
        public void HandleError_ShouldReturnNotFound_WhenStatusIs404()
        {
            // Act
            var result = _controller.HandleError(StatusCodes.Status404NotFound) as NotFoundObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(result.Value, Is.EqualTo(new { Message = "Resource not found." })
                                    .Using<object>((x, y) => x?.ToString() == y?.ToString()));
        }

        [Test]
        public void HandleError_ShouldReturnInternalServerError_WhenStatusIs500()
        {
            // Act
            var result = _controller.HandleError(StatusCodes.Status500InternalServerError) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.That(result.Value, Is.EqualTo(new { Message = "An unexpected error occurred." })
                                    .Using<object>((x, y) => x?.ToString() == y?.ToString()));
        }

        [Test]
        public void HandleError_ShouldReturnBadRequest_WhenStatusIs400()
        {
            // Act
            var result = _controller.HandleError(StatusCodes.Status400BadRequest) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(result.Value, Is.EqualTo(new { Message = "Bad request." })
                                    .Using<object>((x, y) => x?.ToString() == y?.ToString()));
        }

        [Test]
        public void HandleError_ShouldReturnCustomError_WhenStatusIsOther()
        {
            // Arrange
            var customStatus = 418; // I'm a teapot ☕😅

            // Act
            var result = _controller.HandleError(customStatus) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.StatusCode, Is.EqualTo(customStatus));
            Assert.That(result.Value, Is.EqualTo(new { Message = "An error occurred." })
                                    .Using<object>((x, y) => x?.ToString() == y?.ToString()));
        }
    }
}
