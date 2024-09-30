using Microsoft.AspNetCore.Mvc;
using Million.Application.DTOs;
using Million.Application.Services;
using Million.Domain.Enitities;
using Million.Application.Interfaces.Services;

namespace Million.API.Controllers
{
    /// <summary>
    /// API Controller for managing Property Trace operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyTraceController : ControllerBase
    {
        private readonly IPropertyTraceService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTraceController"/> class.
        /// </summary>
        /// <param name="service">The service to manage Property Trace operations.</param>
        public PropertyTraceController(IPropertyTraceService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all property traces.
        /// </summary>
        /// <returns>A list of all property traces.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetPropertyTraceAsync());
        }

        /// <summary>
        /// Retrieves a property trace by its ID.
        /// </summary>
        /// <param name="id">The ID of the property trace to retrieve.</param>
        /// <returns>The requested property trace if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetPropertyTraceByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(await _service.GetPropertyTraceByIdAsync(id));
        }

        /// <summary>
        /// Creates a new property trace.
        /// </summary>
        /// <param name="PropertyTrace">The DTO containing details of the new property trace.</param>
        /// <returns>The newly created property trace.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(PropertyTraceDto PropertyTrace)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.AddPropertyTraceAsync(PropertyTrace);
            return CreatedAtAction(nameof(GetById), new { id = PropertyTrace.Name }, PropertyTrace);
        }

        /// <summary>
        /// Updates an existing property trace.
        /// </summary>
        /// <param name="propertyTraceDto">The property trace DTO containing updated information.</param>
        /// <param name="id">The unique identifier of the property trace to be updated.</param>
        /// <returns>NoContent if the update was successful; otherwise, BadRequest.</returns>
        /// <response code="204">The property trace was successfully updated.</response>
        /// <response code="400">If the update operation failed due to validation errors or if the specified property trace was not found.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(PropertyTraceDto propertyTraceDto, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _service.UpdatePropertyTraceAsync(propertyTraceDto, id))
            {
                return BadRequest();
            }

            return NoContent();
        }


        /// <summary>
        /// Deletes a property trace by its ID.
        /// </summary>
        /// <param name="id">The ID of the property trace to delete.</param>
        /// <returns>NoContent if successful, otherwise BadRequest.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _service.DeletePropertyTraceAsync(id))
                return BadRequest();

            return NoContent();
        }
    }
}
