using System.ComponentModel.DataAnnotations;

namespace Million.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for creating and updating property trace details.
    /// </summary>
    public class PropertyTraceDto
    {
        /// <summary>
        /// Gets or sets the date of the property sale.
        /// This field is required.
        /// </summary>
        [Required]
        public DateTime DateSale { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the property trace.
        /// This field is required.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the property trace.
        /// This field is required and must be greater than zero.
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the tax associated with the property trace.
        /// This field is required and must be greater than zero.
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public int Tax { get; set; }

        /// <summary>
        /// Gets or sets the ID of the associated property.
        /// This field is required.
        /// </summary>
        [Required]
        public int IdProperty { get; set; }
    }
}
