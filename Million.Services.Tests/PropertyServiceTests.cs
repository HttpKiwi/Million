using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Million.Application.DTOs;
using Million.Application.Interfaces.Services;
using Million.Application.Services;
using Million.Domain.Enitities;
using Million.Infrastructure.Data;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Million.Services.Tests
{
    [TestFixture]
    public class PropertyServiceTests : IDisposable
    {
        private DataContext _context;
        private PropertyService _propertyService;
        private IMapper _mapper;
        private Mock<IOwnerService> _mockOwnerService;

        [SetUp]
        public void Setup()
        {
            // Set up the In-Memory Database
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            // Set up AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PropertyDto, Property>();
            });

            _mapper = config.CreateMapper();

            // Set up the mocked OwnerService
            _mockOwnerService = new Mock<IOwnerService>();

            _propertyService = new PropertyService(_context, _mockOwnerService.Object, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }

        [Test]
        public async Task GetAllPropertiesAsync_ReturnsListOfProperties()
        {
            // Arrange
            var properties = new List<Property>
            {
                new() { IdProperty = 1, Name = "Property 1", Address = "Address 1", Price = 1000, Year = 2000 },
                new() { IdProperty = 2, Name = "Property 2", Address = "Address 2", Price = 2000, Year = 2010 }
            };
            await _context.Property.AddRangeAsync(properties);
            await _context.SaveChangesAsync();

            // Act
            var result = await _propertyService.GetAllPropertiesAsync();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("Property 1"));
            Assert.That(result[1].Name, Is.EqualTo("Property 2"));
        }

        [Test]
        public async Task GetPropertyByIdAsync_ReturnsProperty_WhenPropertyExists()
        {
            // Arrange
            var property = new Property { IdProperty = 1, Name = "Property 1", Address = "Address 1", Price = 1000, Year = 2000 };
            await _context.Property.AddAsync(property);
            await _context.SaveChangesAsync();

            // Act
            var result = await _propertyService.GetPropertyByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Property 1"));
        }

        [Test]
        public async Task GetPropertyByIdAsync_ReturnsNull_WhenPropertyDoesNotExist()
        {
            // Act
            var result = await _propertyService.GetPropertyByIdAsync(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task AddPropertyAsync_ReturnsTrue_WhenOwnerExists()
        {
            // Arrange
            var propertyDto = new PropertyDto { Name = "New Property", Address = "New Address", Price = 1000, Year = 2022, IdOwner = 1 };
            var owner = new Owner { IdOwner = 1, Name = "Owner 1" };

            _mockOwnerService.Setup(s => s.GetOwnerByIdAsync(1)).ReturnsAsync(owner);

            // Act
            var result = await _propertyService.AddPropertyAsync(propertyDto, 1);

            // Assert
            Assert.That(result, Is.True);
            var properties = await _context.Property.ToListAsync();
            Assert.That(properties, Has.Count.EqualTo(1));
            Assert.That(properties[0].Name, Is.EqualTo("New Property"));
        }

        [Test]
        public async Task AddPropertyAsync_ReturnsFalse_WhenOwnerDoesNotExist()
        {
            // Arrange
            var propertyDto = new PropertyDto { Name = "New Property", Address = "New Address", Price = 1000, Year = 2022, IdOwner = 1 };

            _mockOwnerService.Setup(s => s.GetOwnerByIdAsync(1)).ReturnsAsync(null as Owner);

            // Act
            var result = await _propertyService.AddPropertyAsync(propertyDto, 1);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task UpdatePropertyAsync_ReturnsTrue_WhenPropertyExists()
        {
            // Arrange
            var property = new Property { IdProperty = 1, Name = "Old Property", Address = "Old Address", Price = 500, Year = 1999 };
            await _context.Property.AddAsync(property);
            await _context.SaveChangesAsync();

            var propertyDto = new PropertyDto { Name = "Updated Property", Address = "Updated Address", Price = 2000, Year = 2020 };

            // Act
            var result = await _propertyService.UpdatePropertyAsync(propertyDto, 1);

            // Assert
            Assert.That(result, Is.True);
            var updatedProperty = await _context.Property.FindAsync(1);
            Assert.That(updatedProperty?.Name, Is.EqualTo("Updated Property"));
            Assert.That(updatedProperty.Price, Is.EqualTo(2000));
        }

        [Test]
        public async Task UpdatePropertyAsync_ReturnsFalse_WhenPropertyDoesNotExist()
        {
            // Arrange
            var propertyDto = new PropertyDto { Name = "Updated Property", Address = "Updated Address", Price = 2000, Year = 2020 };

            // Act
            var result = await _propertyService.UpdatePropertyAsync(propertyDto, 999);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeletePropertyAsync_ReturnsTrue_WhenPropertyDeleted()
        {
            // Arrange
            var property = new Property { IdProperty = 1, Name = "Property to Delete", Address = "Address to Delete", Price = 500 };
            await _context.Property.AddAsync(property);
            await _context.SaveChangesAsync();

            // Act
            var result = await _propertyService.DeletePropertyAsync(1);

            // Assert
            Assert.That(result, Is.True);
            var deletedProperty = await _context.Property.FindAsync(1);
            Assert.That(deletedProperty, Is.Null);
        }

        [Test]
        public async Task DeletePropertyAsync_ReturnsFalse_WhenPropertyDoesNotExist()
        {
            // Act
            var result = await _propertyService.DeletePropertyAsync(999);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task FilterPropertiesAsync_ReturnsFilteredProperties()
        {
            // Arrange
            var properties = new List<Property>
            {
                new() { IdProperty = 1, Name = "Property 1", Address = "Address 1", Price = 500, Year = 2015 },
                new() { IdProperty = 2, Name = "Property 2", Address = "Address 2", Price = 1500, Year = 2020 },
                new() { IdProperty = 3, Name = "Property 3", Address = "Address 3", Price = 2000, Year = 2020 }
            };
            await _context.Property.AddRangeAsync(properties);
            await _context.SaveChangesAsync();

            var filterDto = new PropertyFilterDto { MinPrice = 1000, Year = 2020 };

            // Act
            var result = await _propertyService.FilterPropertiesAsync(filterDto);

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("Property 2"));
            Assert.That(result[1].Name, Is.EqualTo("Property 3"));
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
