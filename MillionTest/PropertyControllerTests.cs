using Microsoft.AspNetCore.Mvc;
using Million.API.Controllers;
using Million.Application.DTOs;
using Million.Application.Interfaces.Services;
using Million.Domain.Enitities;
using Moq;

namespace Million.Controllers.Tests
{
    [TestFixture]
    public class PropertyControllerTests
    {
        private Mock<IPropertyService> _mockService;
        private PropertyController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IPropertyService>();
            _controller = new PropertyController(_mockService.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkResult_WithListOfProperties()
        {
            // Arrange
            var properties = new List<Property>
            {
                new() { IdProperty = 1, Name = "Property1" },
                new() { IdProperty = 2, Name = "Property2" }
            };
            _mockService.Setup(service => service.GetAllPropertiesAsync()).ReturnsAsync(properties);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(properties));
        }

        [Test]
        public async Task GetById_ReturnsNotFound_WhenPropertyDoesNotExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetPropertyByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Property);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetById_ReturnsOkResult_WhenPropertyExists()
        {
            // Arrange
            var property = new Property { IdProperty = 1, Name = "Property1" };
            _mockService.Setup(service => service.GetPropertyByIdAsync(1)).ReturnsAsync(property);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(property));
        }

        [Test]
        public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(new PropertyDto());

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Create_ReturnsCreatedAtAction_WhenSuccessful()
        {
            // Arrange
            var propertyDto = new PropertyDto { Name = "Property1", IdOwner = 1 };
            _mockService.Setup(service => service.AddPropertyAsync(propertyDto, propertyDto.IdOwner)).ReturnsAsync(true);

            // Act
            var result = await _controller.Create(propertyDto);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.That(createdAtActionResult, Is.Not.Null);
            Assert.That(createdAtActionResult.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task Update_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var propertyDto = new PropertyDto { Name = "UpdatedProperty" };
            _mockService.Setup(service => service.UpdatePropertyAsync(propertyDto, 1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(propertyDto, 1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            _mockService.Setup(service => service.DeletePropertyAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Filter_ReturnsOkResult_WithFilteredProperties()
        {
            // Arrange
            var properties = new List<Property>
            {
                new() { IdProperty = 1, Name = "Property1" }
            };
            var filterDto = new PropertyFilterDto { MinPrice = 100000, MaxPrice = 200000 };
            _mockService.Setup(service => service.FilterPropertiesAsync(filterDto)).ReturnsAsync(properties);

            // Act
            var result = await _controller.Filter(filterDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(properties));
        }
    }
}
