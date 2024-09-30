using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Million.Application.DTOs;
using Million.Application.Interfaces.Services;
using Million.Domain.Enitities;
using Million.Infrastructure.Data;

namespace Million.Application.Services
{
    /// <summary>
    /// Service for managing property image-related operations.
    /// </summary>
    public class PropertyImageService : IPropertyImageService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyImageService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The AutoMapper instance for object mapping.</param>
        public PropertyImageService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of all property images.
        /// </summary>
        /// <returns>A list of <see cref="PropertyImage"/> objects.</returns>
        public async Task<List<PropertyImage>> GetPropertyImageAsync()
        {
            return await _context.PropertyImage.ToListAsync();
        }

        /// <summary>
        /// Retrieves a property image by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the property image.</param>
        /// <returns>The <see cref="PropertyImage"/> object if found; otherwise, null.</returns>
        public async Task<PropertyImage> GetPropertyImageByIdAsync(int id)
        {
            return await _context.PropertyImage.FindAsync(id);
        }

        /// <summary>
        /// Adds a new property image with the specified details and photo.
        /// </summary>
        /// <param name="propertyImageDto">The property image data transfer object.</param>
        /// <param name="photo">The property image as a file.</param>
        public async Task AddPropertyImageAsync(PropertyImageDto propertyImageDto, IFormFile photo)
        {
            var propertyImage = _mapper.Map<PropertyImage>(propertyImageDto);
            using (var memoryStream = new MemoryStream())
            {
                await photo.CopyToAsync(memoryStream);
                propertyImage.File = memoryStream.ToArray();
            }
            _context.PropertyImage.Add(propertyImage);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing property image.
        /// </summary>
        /// <param name="propertyImageDto">The DTO containing updated details for the property image.</param>
        /// <param name="id">The ID of the property image to update.</param>
        /// <returns>true if the update was successful; otherwise, false.</returns>
        public async Task<bool> UpdatePropertyImageAsync(PropertyImageDto propertyImageDto, int id)
        {

            var propertyImage = _mapper.Map<PropertyImage>(propertyImageDto);

            var currentPropertyImage = await _context.PropertyImage.FindAsync(id);
            if (currentPropertyImage == null)
            {
                return false;
            }

            currentPropertyImage.Enabled = propertyImage.Enabled;

            await _context.SaveChangesAsync();
            return true;
        }


        /// <summary>
        /// Deletes a property image by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the property image to delete.</param>
        /// <returns>true if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> DeletePropertyImageAsync(int id)
        {
            var currentPropertyImage = await _context.PropertyImage.FindAsync(id);
            if (currentPropertyImage == null)
            {
                return false;
            }

            _context.PropertyImage.Remove(currentPropertyImage);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
