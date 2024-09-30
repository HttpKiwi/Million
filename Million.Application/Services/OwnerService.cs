using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Million.Application.DTOs;
using Million.Application.Interfaces.Services;
using Million.Domain.Enitities;
using Million.Infrastructure.Data;

namespace Million.Application.Services
{
    /// <summary>
    /// Service for managing owner-related operations.
    /// </summary>
    public class OwnerService : IOwnerService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnerService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The AutoMapper instance for object mapping.</param>
        public OwnerService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of all owners.
        /// </summary>
        /// <returns>A list of <see cref="Owner"/> objects.</returns>
        public async Task<List<Owner>> GetOwnersAsync()
        {
            return await _context.Owner.ToListAsync();
        }

        /// <summary>
        /// Retrieves an owner by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the owner.</param>
        /// <returns>The <see cref="Owner"/> object if found; otherwise, null.</returns>
        public async Task<Owner> GetOwnerByIdAsync(int id)
        {
            return await _context.Owner.FindAsync(id);
        }

        /// <summary>
        /// Adds a new owner with the specified details and photo.
        /// </summary>
        /// <param name="ownerDto">The owner data transfer object.</param>
        /// <param name="photo">The owner's photo as a file.</param>
        public async Task AddOwnerAsync(OwnerDto ownerDto, IFormFile photo)
        {
            var owner = _mapper.Map<Owner>(ownerDto);
            using (var memoryStream = new MemoryStream())
            {
                await photo.CopyToAsync(memoryStream);
                owner.Photo = memoryStream.ToArray();
            }
            _context.Owner.Add(owner);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing owner's details.
        /// </summary>
        /// <param name="ownerDto">The DTO containing updated details for the owner.</param>
        /// <param name="id">The unique identifier of the owner to be updated.</param>
        /// <returns>
        /// <c>true</c> if the update was successful; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> UpdateOwnerAsync(OwnerDto ownerDto, int id)
        {
            var owner = _mapper.Map<Owner>(ownerDto);
            var currentOwner = await _context.Owner.FindAsync(id);
            if (currentOwner == null)
            {
                return false;
            }

            currentOwner.Name = owner.Name;
            currentOwner.Address = owner.Address;
            currentOwner.Photo = owner.Photo;

            await _context.SaveChangesAsync();
            return true;
        }


        /// <summary>
        /// Deletes an owner by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the owner to delete.</param>
        /// <returns>true if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> DeleteOwnerAsync(int id)
        {
            var currentOwner = await _context.Owner.FindAsync(id);
            if (currentOwner == null)
            {
                return false;
            }

            _context.Owner.Remove(currentOwner);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
