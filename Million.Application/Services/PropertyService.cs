using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Million.Application.DTOs;
using Million.Application.Interfaces.Services;
using Million.Domain.Enitities;
using Million.Infrastructure.Data;

namespace Million.Application.Services
{
    /// <summary>
    /// Service for managing property-related operations.
    /// </summary>
    public class PropertyService : IPropertyService
    {
        private readonly DataContext _context;
        private readonly IOwnerService _ownerService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="ownerService">The service for managing owners.</param>
        /// <param name="mapper">The AutoMapper instance for object mapping.</param>
        public PropertyService(DataContext context, IOwnerService ownerService, IMapper mapper)
        {
            _context = context;
            _ownerService = ownerService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of all properties.
        /// </summary>
        /// <returns>A list of <see cref="Property"/> objects.</returns>
        public async Task<List<Property>> GetAllPropertiesAsync()
        {
            return await _context.Property.ToListAsync();
        }

        /// <summary>
        /// Retrieves a property by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the property.</param>
        /// <returns>The <see cref="Property"/> object if found; otherwise, null.</returns>
        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            return await _context.Property.FindAsync(id);
        }

        /// <summary>
        /// Adds a new property with the specified details and owner ID.
        /// </summary>
        /// <param name="propertyDto">The property data transfer object.</param>
        /// <param name="id">The ID of the owner associated with the property.</param>
        /// <returns>true if the addition was successful; otherwise, false.</returns>
        public async Task<bool> AddPropertyAsync(PropertyDto propertyDto, int id)
        {
            var property = _mapper.Map<Property>(propertyDto);
            var owner = await _ownerService.GetOwnerByIdAsync(id);
            if (owner == null)
            {
                return false;
            }
            _context.Property.Add(property);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Updates an existing property.
        /// </summary>
        /// <param name="propertyDto">The DTO containing updated details for the property.</param>
        /// <param name="id">The unique identifier of the property to be updated.</param>
        /// <returns>
        /// True if the update was successful; otherwise, false.
        /// </returns>
        public async Task<bool> UpdatePropertyAsync(PropertyDto propertyDto, int id)
        {
            var property = _mapper.Map<Property>(propertyDto);
            var currentProperty = await _context.Property.FindAsync(id);
            if (currentProperty == null)
            {
                return false;
            }

            currentProperty.Address = property.Address;
            currentProperty.Price = property.Price;
            currentProperty.IdOwner = property.IdOwner;
            currentProperty.Year = property.Year;
            currentProperty.Name = property.Name;
            currentProperty.CodeInternal = property.CodeInternal;

            await _context.SaveChangesAsync();
            return true;
        }


        /// <summary>
        /// Deletes a property by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the property to delete.</param>
        /// <returns>true if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> DeletePropertyAsync(int id)
        {
            var currentProperty = await _context.Property.FindAsync(id);
            if (currentProperty == null)
            {
                return false;
            }

            _context.Property.Remove(currentProperty);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Filters properties based on the specified criteria.
        /// </summary>
        /// <param name="filterDto">The filtering criteria.</param>
        /// <returns>A list of filtered <see cref="Property"/> objects.</returns>
        public async Task<List<Property>> FilterPropertiesAsync(PropertyFilterDto filterDto)
        {
            var query = _context.Property.AsQueryable();

            if (filterDto.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filterDto.MinPrice.Value);
            }

            if (filterDto.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filterDto.MaxPrice.Value);
            }

            if (filterDto.Year.HasValue)
            {
                query = query.Where(p => p.Year == filterDto.Year.Value);
            }

            if (!string.IsNullOrWhiteSpace(filterDto.Name))
            {
                query = query.Where(p => p.Name.Contains(filterDto.Name));
            }

            return await query.ToListAsync();
        }
    }
}
