using Microsoft.AspNetCore.Mvc;
using Million.API.Controllers;
using Million.Application.DTOs;
using Million.Domain.Enitities;
using Moq;
using Million.Application.Interfaces.Services;

namespace Million.Controllers.Tests
{
    [TestFixture]
    public class OwnerControllerTests
    {
        private Mock<IOwnerService> _mockService;
        private OwnerController _controller;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IOwnerService>();
            _controller = new OwnerController(_mockService.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkResult_WithListOfOwners()
        {
            // Arrange
            var owners = new List<Owner>
            {
                new() { IdOwner = 1, Name = "Owner1" },
                new() { IdOwner = 2, Name = "Owner2" }
            };
            _mockService.Setup(service => service.GetOwnersAsync()).ReturnsAsync(owners);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(owners));
        }

        [Test]
        public async Task GetById_ExistingId_ReturnsOkResult_WithOwner()
        {
            // Arrange
            var owner = new Owner { IdOwner = 1, Name = "Owner1" };
            _mockService.Setup(service => service.GetOwnerByIdAsync(1)).ReturnsAsync(owner);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(owner));
        }

        [Test]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(service => service.GetOwnerByIdAsync(1)).ReturnsAsync(null as Owner);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Create_ValidOwner_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var ownerDto = new OwnerDto { Name = "Owner1" };
            _mockService.Setup(service => service.AddOwnerAsync(ownerDto, null)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(ownerDto, null);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(createdResult.Value, Is.EqualTo(ownerDto));
        }

        [Test]
        public async Task Create_InvalidOwner_ReturnsBadRequest()
        {
            // Arrange
            var ownerDto = new OwnerDto(); // Invalid data
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(ownerDto, null);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public async Task Update_ExistingOwner_ReturnsNoContent()
        {
            // Arrange
            var ownerDto = new OwnerDto { Name = "Updated Owner" };
            _mockService.Setup(service => service.UpdateOwnerAsync(ownerDto, 1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(ownerDto, 1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Update_NonExistingOwner_ReturnsBadRequest()
        {
            // Arrange
            var ownerDto = new OwnerDto { Name = "Updated Owner" };
            _mockService.Setup(service => service.UpdateOwnerAsync(ownerDto, 1)).ReturnsAsync(false);

            // Act
            var result = await _controller.Update(ownerDto, 1);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task Delete_ExistingOwner_ReturnsNoContent()
        {
            // Arrange
            _mockService.Setup(service => service.DeleteOwnerAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_NonExistingOwner_ReturnsBadRequest()
        {
            // Arrange
            _mockService.Setup(service => service.DeleteOwnerAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }
    }
}
