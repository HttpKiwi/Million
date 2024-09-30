using Microsoft.AspNetCore.Mvc;
using Million.Application.DTOs;
using Million.Application.Interfaces.Services;

namespace Million.API.Controllers
{
    /// <summary>
    /// API Controller for managing Property Image operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyImageController : ControllerBase
    {
        private readonly IPropertyImageService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyImageController"/> class.
        /// </summary>
        /// <param name="service">The service used to handle property image operations.</param>
        public PropertyImageController(IPropertyImageService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all property images.
        /// </summary>
        /// <returns>A list of all property images.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetPropertyImageAsync());
        }

        /// <summary>
        /// Retrieves a property image by its ID.
        /// </summary>
        /// <param name="id">The ID of the property image to retrieve.</param>
        /// <returns>The requested property image if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetPropertyImageByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Creates a new property image.
        /// </summary>
        /// <param name="propertyImageDto">The DTO containing property image details.</param>
        /// <param name="photo">The image file for the property.</param>
        /// <returns>The newly created property image.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PropertyImageDto propertyImageDto, IFormFile photo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.AddPropertyImageAsync(propertyImageDto, photo);
            return CreatedAtAction(nameof(GetById), new { id = propertyImageDto.IdProperty }, propertyImageDto);
        }

        /// <summary>
        /// Updates an existing property image.
        /// </summary>
        /// <param name="propertyImageDto">The property image DTO containing updated information.</param>
        /// <param name="id">The unique identifier of the property image to be updated.</param>
        /// <returns>NoContent if the update was successful; otherwise, BadRequest.</returns>
        /// <response code="204">The property image was successfully updated.</response>
        /// <response code="400">If the update operation failed due to validation errors or if the specified property image was not found.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(PropertyImageDto propertyImageDto, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _service.UpdatePropertyImageAsync(propertyImageDto, id))
            {
                return BadRequest();
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a property image by its ID.
        /// </summary>
        /// <param name="id">The ID of the property image to delete.</param>
        /// <returns>NoContent if successful, otherwise BadRequest.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _service.DeletePropertyImageAsync(id))
                return BadRequest();

            return NoContent();
        }
    }
}
