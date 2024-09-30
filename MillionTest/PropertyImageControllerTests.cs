using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Million.API.Controllers;
using Million.Application.DTOs;
using Million.Application.Interfaces.Services;
using Million.Domain.Enitities;
using Moq;

namespace Million.Controllers.Tests
{
    [TestFixture]
    public class PropertyImageControllerTests
    {
        private Mock<IPropertyImageService> _mockService;
        private PropertyImageController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IPropertyImageService>();
            _controller = new PropertyImageController(_mockService.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkResult_WithListOfPropertyImages()
        {
            // Arrange
            var propertyImages = new List<PropertyImage>
            {
                new() { IdPropertyImage = 1, IdProperty = 1 },
                new() { IdPropertyImage = 2, IdProperty = 2 }
            };
            _mockService.Setup(service => service.GetPropertyImageAsync()).ReturnsAsync(propertyImages);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(propertyImages));
        }

        [Test]
        public async Task GetById_ReturnsNotFound_WhenPropertyImageDoesNotExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetPropertyImageByIdAsync(It.IsAny<int>())).ReturnsAsync(null as PropertyImage);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetById_ReturnsOkResult_WhenPropertyImageExists()
        {
            // Arrange
            var propertyImage = new PropertyImage { IdPropertyImage = 1, IdProperty = 1 };
            _mockService.Setup(service => service.GetPropertyImageByIdAsync(1)).ReturnsAsync(propertyImage);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(propertyImage));
        }

        [Test]
        public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("IdProperty", "Required");

            // Act
            var result = await _controller.Create(new PropertyImageDto(), null);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Create_ReturnsCreatedAtAction_WhenSuccessful()
        {
            // Arrange
            var propertyImageDto = new PropertyImageDto { IdProperty = 1 };
            var photo = new Mock<IFormFile>();
            _mockService.Setup(service => service.AddPropertyImageAsync(propertyImageDto, photo.Object)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(propertyImageDto, photo.Object);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.That(createdAtActionResult, Is.Not.Null);
            Assert.That(createdAtActionResult.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task Update_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var propertyImageDto = new PropertyImageDto { IdProperty = 1 };
            _mockService.Setup(service => service.UpdatePropertyImageAsync(propertyImageDto, 1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(propertyImageDto, 1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            _mockService.Setup(service => service.DeletePropertyImageAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_ReturnsBadRequest_WhenUnsuccessful()
        {
            // Arrange
            _mockService.Setup(service => service.DeletePropertyImageAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }
    }
}
