using System.ComponentModel.DataAnnotations;

namespace Million.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for creating and updating property image details.
    /// </summary>
    public class PropertyImageDto
    {
        /// <summary>
        /// Gets or sets the ID of the property associated with the image.
        /// This field is required and must be a valid property ID.
        /// </summary>
        [Required]
        public int IdProperty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image is enabled.
        /// Default value is false, indicating that the image is not enabled.
        /// </summary>
        public bool Enabled { get; set; }
    }
}
