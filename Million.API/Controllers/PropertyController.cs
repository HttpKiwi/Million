using Microsoft.AspNetCore.Mvc;
using Million.Application.DTOs;
using Million.Application.Interfaces.Services;
using Million.Application.Services;
using Million.Domain.Enitities;

namespace Million.API.Controllers
{
    /// <summary>
    /// API Controller for managing Property operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyController"/> class.
        /// </summary>
        /// <param name="service">The service used to handle property operations.</param>
        public PropertyController(IPropertyService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all properties.
        /// </summary>
        /// <returns>A list of all properties.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllPropertiesAsync());
        }

        /// <summary>
        /// Retrieves a property by its ID.
        /// </summary>
        /// <param name="id">The ID of the property to retrieve.</param>
        /// <returns>The requested property if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var property = await _service.GetPropertyByIdAsync(id);
            if (property == null)
                return NotFound();

            return Ok(property);
        }

        /// <summary>
        /// Creates a new property.
        /// </summary>
        /// <param name="propertyDto">The DTO containing the property details.</param>
        /// <returns>The newly created property.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(PropertyDto propertyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _service.AddPropertyAsync(propertyDto, propertyDto.IdOwner);
            if (!success)
            {
                return BadRequest("Failed to create property");
            }

            return CreatedAtAction(nameof(GetById), new { id = propertyDto.Name }, propertyDto);
        }

        /// <summary>
        /// Updates an existing property.
        /// </summary>
        /// <param name="propertyDto">The DTO containing updated information for the property.</param>
        /// <param name="id">The unique identifier of the property to be updated.</param>
        /// <returns>
        /// NoContent if the update was successful; otherwise, BadRequest with an error message.
        /// </returns>
        /// <response code="204">The property was successfully updated.</response>
        /// <response code="400">If the update operation failed.</response>
        [HttpPut]
        public async Task<IActionResult> Update(PropertyDto propertyDto, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _service.UpdatePropertyAsync(propertyDto, id);
            if (!success)
            {
                return BadRequest("Failed to update property");
            }

            return NoContent();
        }


        /// <summary>
        /// Deletes a property by its ID.
        /// </summary>
        /// <param name="id">The ID of the property to delete.</param>
        /// <returns>NoContent if successful, otherwise BadRequest.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeletePropertyAsync(id);
            if (!success)
            {
                return BadRequest("Failed to delete property");
            }

            return NoContent();
        }

        /// <summary>
        /// Filters properties based on specified criteria.
        /// </summary>
        /// <param name="filterDto">The filtering criteria.</param>
        /// <returns>A list of filtered properties.</returns>
        /// <response code="200">Returns the filtered list of properties.</response>
        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] PropertyFilterDto filterDto)
        {
            var properties = await _service.FilterPropertiesAsync(filterDto);
            return Ok(properties);
        }

    }
}
