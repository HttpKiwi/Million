using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Million.Application.DTOs;
using Million.Application.Interfaces.Services;
using Million.Domain.Enitities;
using Million.Infrastructure.Data;

namespace Million.Application.Services
{
    /// <summary>
    /// Service for managing property trace-related operations.
    /// </summary>
    public class PropertyTraceService : IPropertyTraceService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTraceService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The AutoMapper instance for object mapping.</param>
        public PropertyTraceService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of all property traces.
        /// </summary>
        /// <returns>A list of <see cref="PropertyTrace"/> objects.</returns>
        public async Task<List<PropertyTrace>> GetPropertyTraceAsync()
        {
            return await _context.PropertyTrace.ToListAsync();
        }

        /// <summary>
        /// Retrieves a property trace by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the property trace.</param>
        /// <returns>The <see cref="PropertyTrace"/> object if found; otherwise, null.</returns>
        public async Task<PropertyTrace> GetPropertyTraceByIdAsync(int id)
        {
            return await _context.PropertyTrace.FindAsync(id);
        }

        /// <summary>
        /// Adds a new property trace with the specified details.
        /// </summary>
        /// <param name="propertyTraceDto">The property trace data transfer object.</param>
        public async Task AddPropertyTraceAsync(PropertyTraceDto propertyTraceDto)
        {
            var propertyTrace = _mapper.Map<PropertyTrace>(propertyTraceDto);
            _context.PropertyTrace.Add(propertyTrace);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing property trace.
        /// </summary>
        /// <param name="propertyTraceDto">The DTO containing updated details for the property trace.</param>
        /// <param name="id">The ID of the property trace to update.</param>
        /// <returns>true if the update was successful; otherwise, false.</returns>
        public async Task<bool> UpdatePropertyTraceAsync(PropertyTraceDto propertyTraceDto, int id)
        {
            // Map the DTO to the PropertyTrace entity
            var propertyTrace = _mapper.Map<PropertyTrace>(propertyTraceDto);

            // Find the current property trace in the database
            var currentPropertyTrace = await _context.PropertyTrace.FindAsync(id);
            if (currentPropertyTrace == null)
            {
                return false; // Return false if the property trace does not exist
            }

            // Update the existing property trace with new values
            currentPropertyTrace.DateSale = propertyTrace.DateSale;
            currentPropertyTrace.Name = propertyTrace.Name;
            currentPropertyTrace.Value = propertyTrace.Value;
            currentPropertyTrace.Tax = propertyTrace.Tax;
            currentPropertyTrace.IdProperty = propertyTrace.IdProperty;

            // Save changes to the database
            await _context.SaveChangesAsync();
            return true; // Return true indicating the update was successful
        }

        /// <summary>
        /// Deletes a property trace by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the property trace to delete.</param>
        /// <returns>true if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> DeletePropertyTraceAsync(int id)
        {
            var currentPropertyTrace = await _context.PropertyTrace.FindAsync(id);
            if (currentPropertyTrace == null)
            {
                return false;
            }

            _context.PropertyTrace.Remove(currentPropertyTrace);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
