using Microsoft.AspNetCore.Mvc;
using Million.API.Controllers;
using Million.Application.DTOs;
using Million.Application.Interfaces.Services;
using Million.Application.Services;
using Million.Domain.Enitities;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Million.Controllers.Tests
{
    [TestFixture]
    public class PropertyTraceControllerTests
    {
        private Mock<IPropertyTraceService> _mockService;
        private PropertyTraceController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IPropertyTraceService>();
            _controller = new PropertyTraceController(_mockService.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkResult_WithListOfPropertyTraces()
        {
            // Arrange
            var propertyTraces = new List<PropertyTrace>
            {
                new() { IdPropertyTrace = 1, Name = "Trace 1", IdProperty = 1 },
                new() { IdPropertyTrace = 2, Name = "Trace 2", IdProperty = 2 }
            };
            _mockService.Setup(service => service.GetPropertyTraceAsync()).ReturnsAsync(propertyTraces);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(propertyTraces));
        }

        [Test]
        public async Task GetById_ReturnsNotFound_WhenPropertyTraceDoesNotExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetPropertyTraceByIdAsync(It.IsAny<int>())).ReturnsAsync(null as PropertyTrace);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task GetById_ReturnsOkResult_WhenPropertyTraceExists()
        {
            // Arrange
            var propertyTrace = new PropertyTrace { IdPropertyTrace = 1, Name = "Trace 1", IdProperty = 1 };
            _mockService.Setup(service => service.GetPropertyTraceByIdAsync(1)).ReturnsAsync(propertyTrace);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(propertyTrace));
        }

        [Test]
        public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(new PropertyTraceDto());

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Create_ReturnsCreatedAtAction_WhenSuccessful()
        {
            // Arrange
            var propertyTraceDto = new PropertyTraceDto { Name = "Trace 1", IdProperty = 1 };
            _mockService.Setup(service => service.AddPropertyTraceAsync(propertyTraceDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(propertyTraceDto);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.That(createdAtActionResult, Is.Not.Null);
            Assert.That(createdAtActionResult.StatusCode, Is.EqualTo(201));
            Assert.That(createdAtActionResult.Value, Is.EqualTo(propertyTraceDto));
        }

        [Test]
        public async Task Update_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var propertyTraceDto = new PropertyTraceDto { Name = "Updated Trace", IdProperty = 1 };
            _mockService.Setup(service => service.UpdatePropertyTraceAsync(propertyTraceDto, 1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(propertyTraceDto, 1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Update_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var propertyTraceDto = new PropertyTraceDto { Name = "Updated Trace", IdProperty = 1 };
            _mockService.Setup(service => service.UpdatePropertyTraceAsync(propertyTraceDto, 1)).ReturnsAsync(false);

            // Act
            var result = await _controller.Update(propertyTraceDto, 1);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task Delete_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            _mockService.Setup(service => service.DeletePropertyTraceAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_ReturnsBadRequest_WhenUnsuccessful()
        {
            // Arrange
            _mockService.Setup(service => service.DeletePropertyTraceAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }
    }
}
