using Microsoft.AspNetCore.Mvc;
using Million.Application.DTOs;
using Million.Application.Interfaces.Services;
using Million.Application.Services;
using Million.Domain.Enitities;

namespace Million.API.Controllers
{
    /// <summary>
    /// API Controller for managing owner-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnerController"/> class.
        /// </summary>
        /// <param name="service">The service used to handle owner operations.</param>
        public OwnerController(IOwnerService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all owners.
        /// </summary>
        /// <returns>A list of all owners.</returns>
        /// <response code="200">Returns the list of owners.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetOwnersAsync());
        }

        /// <summary>
        /// Retrieves an owner by their ID.
        /// </summary>
        /// <param name="id">The ID of the owner to retrieve.</param>
        /// <returns>The requested owner if found, otherwise NotFound.</returns>
        /// <response code="200">Returns the owner with the specified ID.</response>
        /// <response code="404">If the owner is not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var owner = await _service.GetOwnerByIdAsync(id);
            if (owner == null)
                return NotFound();

            return Ok(owner);
        }

        /// <summary>
        /// Creates a new owner.
        /// </summary>
        /// <param name="ownerDto">The DTO containing the owner's information.</param>
        /// <param name="photo">The owner's photo as a file.</param>
        /// <returns>The newly created owner.</returns>
        /// <response code="201">The owner was successfully created.</response>
        /// <response code="400">If the provided data is invalid.</response>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] OwnerDto ownerDto, IFormFile photo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _service.AddOwnerAsync(ownerDto, photo);
            return CreatedAtAction(nameof(GetById), new { id = ownerDto.Name }, ownerDto);
        }

        /// <summary>
        /// Updates an existing owner.
        /// </summary>
        /// <param name="ownerDto">The DTO containing updated information for the owner.</param>
        /// <param name="id">The unique identifier of the owner to be updated.</param>
        /// <returns>
        /// No content if the update was successful; otherwise, a BadRequest response.
        /// </returns>
        /// <response code="204">The owner was successfully updated.</response>
        /// <response code="400">If the update operation failed due to invalid model state or other reasons.</response>
        [HttpPut]
        public async Task<IActionResult> Update(OwnerDto ownerDto, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _service.UpdateOwnerAsync(ownerDto, id);
            if (!success)
            {
                return BadRequest();
            }

            return NoContent();
        }


        /// <summary>
        /// Deletes an owner by their ID.
        /// </summary>
        /// <param name="id">The ID of the owner to delete.</param>
        /// <returns>No content if the deletion was successful, otherwise BadRequest.</returns>
        /// <response code="204">The owner was successfully deleted.</response>
        /// <response code="400">If the deletion operation failed.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteOwnerAsync(id);
            if (!success)
                return BadRequest();

            return NoContent();
        }
    }
}
