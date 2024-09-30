using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Million.Application.DTOs;
using Million.Application.Interfaces.Services;
using Million.Application.Services;
using Million.Domain.Enitities;
using Million.Infrastructure.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Million.Services.Tests
{
    [TestFixture]
    public class PropertyTraceServiceTests : IDisposable
    {
        private DataContext _context;
        private PropertyTraceService _propertyTraceService;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PropertyTraceDto, PropertyTrace>();
            });

            _mapper = config.CreateMapper();
            _propertyTraceService = new PropertyTraceService(_context, _mapper);
        }

        [TearDown] // This method is called after each test
        public void TearDown()
        {
            _context?.Dispose();
        }

        [Test]
        public async Task GetPropertyTraceAsync_ReturnsListOfPropertyTraces()
        {
            // Arrange
            var propertyTraces = new List<PropertyTrace>
            {
                new() { IdPropertyTrace = 1, Name = "Trace 1", Value = 100, Tax = 10, DateSale = DateTime.Now, IdProperty = 1 },
                new() { IdPropertyTrace = 2, Name = "Trace 2", Value = 200, Tax = 20, DateSale = DateTime.Now, IdProperty = 1 }
            };

            await _context.PropertyTrace.AddRangeAsync(propertyTraces);
            await _context.SaveChangesAsync();

            // Act
            var result = await _propertyTraceService.GetPropertyTraceAsync();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task GetPropertyTraceByIdAsync_ReturnsPropertyTrace_WhenExists()
        {
            // Arrange
            var propertyTrace = new PropertyTrace { IdPropertyTrace = 1, Name = "Trace 1", Value = 100, Tax = 10, DateSale = DateTime.Now, IdProperty = 1 };
            await _context.PropertyTrace.AddAsync(propertyTrace);
            await _context.SaveChangesAsync();

            // Act
            var result = await _propertyTraceService.GetPropertyTraceByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Trace 1"));
        }

        [Test]
        public async Task GetPropertyTraceByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Act
            var result = await _propertyTraceService.GetPropertyTraceByIdAsync(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task AddPropertyTraceAsync_AddsNewPropertyTrace()
        {
            // Arrange
            var propertyTraceDto = new PropertyTraceDto { Name = "New Trace", Value = 150, Tax = 15, DateSale = DateTime.Now, IdProperty = 1 };

            // Act
            await _propertyTraceService.AddPropertyTraceAsync(propertyTraceDto);

            // Assert
            var propertyTraces = await _context.PropertyTrace.ToListAsync();
            Assert.That(propertyTraces, Has.Count.EqualTo(1));
            Assert.That(propertyTraces[0].Name, Is.EqualTo("New Trace"));
        }

        [Test]
        public async Task UpdatePropertyTraceAsync_ReturnsTrue_WhenPropertyTraceExists()
        {
            // Arrange
            var existingTrace = new PropertyTrace { IdPropertyTrace = 1, Name = "Old Trace", Value = 100, Tax = 10, DateSale = DateTime.Now, IdProperty = 1 };
            await _context.PropertyTrace.AddAsync(existingTrace);
            await _context.SaveChangesAsync();

            var propertyTraceDto = new PropertyTraceDto { Name = "Updated Trace", Value = 200, Tax = 20, DateSale = DateTime.Now, IdProperty = 1 };

            // Act
            var result = await _propertyTraceService.UpdatePropertyTraceAsync(propertyTraceDto, existingTrace.IdPropertyTrace);

            // Assert
            Assert.That(result, Is.True);
            var updatedTrace = await _context.PropertyTrace.FindAsync(existingTrace.IdPropertyTrace);
            Assert.That(updatedTrace?.Name, Is.EqualTo("Updated Trace"));
        }

        [Test]
        public async Task UpdatePropertyTraceAsync_ReturnsFalse_WhenPropertyTraceDoesNotExist()
        {
            // Arrange
            var propertyTraceDto = new PropertyTraceDto { Name = "Updated Trace", Value = 200, Tax = 20, DateSale = DateTime.Now, IdProperty = 1 };

            // Act
            var result = await _propertyTraceService.UpdatePropertyTraceAsync(propertyTraceDto, 999);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeletePropertyTraceAsync_ReturnsTrue_WhenPropertyTraceDeleted()
        {
            // Arrange
            var existingTrace = new PropertyTrace { IdPropertyTrace = 1, Name = "Trace to Delete", Value = 100, Tax = 10, DateSale = DateTime.Now, IdProperty = 1 };
            await _context.PropertyTrace.AddAsync(existingTrace);
            await _context.SaveChangesAsync();

            // Act
            var result = await _propertyTraceService.DeletePropertyTraceAsync(1);

            // Assert
            Assert.That(result, Is.True);
            var deletedTrace = await _context.PropertyTrace.FindAsync(1);
            Assert.That(deletedTrace, Is.Null);
        }

        [Test]
        public async Task DeletePropertyTraceAsync_ReturnsFalse_WhenPropertyTraceNotFound()
        {
            // Arrange

            // Act
            var result = await _propertyTraceService.DeletePropertyTraceAsync(999);

            // Assert
            Assert.That(result, Is.False);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
