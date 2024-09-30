using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Million.Application.DTOs;
using Million.Application.Interfaces.Services;
using Million.Application.Services;
using Million.Domain.Enitities;
using Million.Infrastructure.Data;
using Moq;

namespace Million.Services.Tests
{
    [TestFixture]
    public class OwnerServiceTests : IDisposable // Implement IDisposable
    {
        private DataContext _context;
        private OwnerService _ownerService;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            var config = new MapperConfiguration(config =>
            {
                config.CreateMap<OwnerDto, Owner>();
            });

            _mapper = config.CreateMapper(); 
            _ownerService = new OwnerService(_context, _mapper);
        }

        [TearDown] // This method is called after each test
        public void TearDown()
        {
            // Dispose of the context to avoid memory leaks
            _context?.Dispose();
        }

        [Test]
        public async Task GetOwnersAsync_ReturnsListOfOwners()
        {
            // Arrange
            var owners = new List<Owner>
            {
                new() { IdOwner = 1, Name = "Owner 1" },
                new() { IdOwner = 2, Name = "Owner 2" }
            };

            await _context.Owner.AddRangeAsync(owners);
            await _context.SaveChangesAsync();

            // Act
            var result = await _ownerService.GetOwnersAsync();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task AddOwnerAsync_AddsNewOwner()
        {
            // Arrange
            var ownerDto = new OwnerDto { Name = "New Owner", Address = "123 Street" };
            var photo = new Mock<IFormFile>();
            var memoryStream = new MemoryStream();
            var photoData = new byte[] { 0x1, 0x2, 0x3 };
            memoryStream.Write(photoData, 0, photoData.Length);
            memoryStream.Position = 0;

            photo.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Callback<Stream, CancellationToken>((stream, token) => memoryStream.CopyTo(stream));

            // Act
            await _ownerService.AddOwnerAsync(ownerDto, photo.Object);

            // Assert
            var owners = await _context.Owner.ToListAsync();
            Assert.That(owners, Has.Count.EqualTo(1));
            Assert.That(owners[0].Name, Is.EqualTo("New Owner"));
        }

        [Test]
        public async Task GetOwnerByIdAsync_ReturnsNull_WhenNotFound()
        {
            // Arrange

            // Act
            var result = await _ownerService.GetOwnerByIdAsync(1);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateOwnerAsync_ReturnsTrue_WhenOwnerUpdated()
        {
            // Arrange
            var existingOwner = new Owner { IdOwner = 1, Name = "Old Owner" };
            await _context.Owner.AddAsync(existingOwner);
            await _context.SaveChangesAsync();

            var ownerDto = new OwnerDto { Name = "Updated Owner" };

            // Act
            var result = await _ownerService.UpdateOwnerAsync(ownerDto, existingOwner.IdOwner);

            // Assert
            Assert.That(result, Is.True);
            var updatedOwner = await _context.Owner.FindAsync(existingOwner.IdOwner);
            Assert.That(updatedOwner?.Name, Is.EqualTo("Updated Owner"));
        }

        [Test]
        public async Task UpdateOwnerAsync_ReturnsFalse_WhenOwnerNotFound()
        {
            // Arrange
            var ownerDto = new OwnerDto { Name = "Updated Owner" };

            // Act
            var result = await _ownerService.UpdateOwnerAsync(ownerDto, 1);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteOwnerAsync_ReturnsTrue_WhenOwnerDeleted()
        {
            // Arrange
            var existingOwner = new Owner { IdOwner = 1, Name = "Old Owner" };
            await _context.Owner.AddAsync(existingOwner);
            await _context.SaveChangesAsync();

            // Act
            var result = await _ownerService.DeleteOwnerAsync(existingOwner.IdOwner);

            // Assert
            Assert.That(result, Is.True);
            var deletedOwner = await _context.Owner.FindAsync(existingOwner.IdOwner);
            Assert.That(deletedOwner, Is.Null);
        }

        [Test]
        public async Task DeleteOwnerAsync_ReturnsFalse_WhenOwnerNotFound()
        {
            // Arrange

            // Act
            var result = await _ownerService.DeleteOwnerAsync(1);

            // Assert
            Assert.That(result, Is.False);
        }

        // Implement IDisposable
        public void Dispose()
        {
            // Dispose of any unmanaged resources here
            _context?.Dispose();
        }
    }
}
