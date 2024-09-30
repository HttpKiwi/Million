using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Million.Application.DTOs;
using Million.Application.Services;
using Million.Domain.Enitities;
using Million.Infrastructure.Data;
using Moq;

namespace Million.Services.Tests
{
    [TestFixture]
    public class PropertyImageServiceTests : IDisposable
    {
        private DataContext _context;
        private PropertyImageService _propertyImageService;
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
                cfg.CreateMap<PropertyImageDto, PropertyImage>();
            });

            _mapper = config.CreateMapper();
            _propertyImageService = new PropertyImageService(_context, _mapper);
        }

        [TearDown] 
        public void TearDown()
        {
            // Dispose of the context to avoid memory leaks
            _context?.Dispose();
        }

        [Test]
        public async Task GetPropertyImageAsync_ReturnsListOfPropertyImages()
        {
            // Arrange
            var propertyImages = new List<PropertyImage>
            {
                new() { IdPropertyImage = 1, Enabled = true },
                new() { IdPropertyImage = 2, Enabled = true }
            };

            await _context.PropertyImage.AddRangeAsync(propertyImages);
            await _context.SaveChangesAsync();

            // Act
            var result = await _propertyImageService.GetPropertyImageAsync();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task AddPropertyImageAsync_AddsNewPropertyImage()
        {
            // Arrange
            var propertyImageDto = new PropertyImageDto { IdProperty = 1, Enabled = true };
            var photo = new Mock<IFormFile>();
            var memoryStream = new MemoryStream();
            var photoData = new byte[] { 0x1, 0x2, 0x3 };
            memoryStream.Write(photoData, 0, photoData.Length);
            memoryStream.Position = 0;

            photo.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Callback<Stream, CancellationToken>((stream, token) => memoryStream.CopyTo(stream));

            // Act
            await _propertyImageService.AddPropertyImageAsync(propertyImageDto, photo.Object);

            // Assert
            var propertyImages = await _context.PropertyImage.ToListAsync();
            Assert.That(propertyImages, Has.Count.EqualTo(1));
            Assert.That(propertyImages[0].Enabled, Is.True);
        }

        [Test]
        public async Task GetPropertyImageByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange

            // Act
            var result = await _propertyImageService.GetPropertyImageByIdAsync(1);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdatePropertyImageAsync_ReturnsTrue_WhenPropertyImageUpdated()
        {
            // Arrange
            var existingImage = new PropertyImage { IdPropertyImage = 1, Enabled = true };
            await _context.PropertyImage.AddAsync(existingImage);
            await _context.SaveChangesAsync();

            var propertyImageDto = new PropertyImageDto { IdProperty = existingImage.IdProperty, Enabled = false };

            // Act
            var result = await _propertyImageService.UpdatePropertyImageAsync(propertyImageDto, existingImage.IdPropertyImage);

            // Assert
            Assert.That(result, Is.True);
            var updatedImage = await _context.PropertyImage.FindAsync(existingImage.IdPropertyImage);
            Assert.That(updatedImage?.Enabled, Is.False);
        }

        [Test]
        public async Task UpdatePropertyImageAsync_ReturnsFalse_WhenPropertyImageNotFound()
        {
            // Arrange
            var propertyImageDto = new PropertyImageDto { IdProperty = 1, Enabled = false };

            // Act
            var result = await _propertyImageService.UpdatePropertyImageAsync(propertyImageDto, 999);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeletePropertyImageAsync_ReturnsTrue_WhenPropertyImageDeleted()
        {
            // Arrange
            var existingImage = new PropertyImage { IdPropertyImage = 1, Enabled = true };
            await _context.PropertyImage.AddAsync(existingImage);
            await _context.SaveChangesAsync();

            // Act
            var result = await _propertyImageService.DeletePropertyImageAsync(1);

            // Assert
            Assert.That(result, Is.True);
            var deletedImage = await _context.PropertyImage.FindAsync(1);
            Assert.That(deletedImage, Is.Null);
        }

        [Test]
        public async Task DeletePropertyImageAsync_ReturnsFalse_WhenPropertyImageNotFound()
        {
            // Arrange
            // Do Nothing

            // Act
            var result = await _propertyImageService.DeletePropertyImageAsync(999);

            // Assert
            Assert.That(result, Is.False);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
