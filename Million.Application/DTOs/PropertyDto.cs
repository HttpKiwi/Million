using System.ComponentModel.DataAnnotations;

namespace Million.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for creating and updating property details.
    /// </summary>
    public class PropertyDto
    {
        /// <summary>
        /// The name of the property. This field is required.
        /// </summary>
        [Required(ErrorMessage = "The property's name is required.")]
        public string Name { get; set; }

        /// <summary>
        /// The address of the property. This field is required.
        /// </summary>
        [Required(ErrorMessage = "The property's address is required.")]
        public string Address { get; set; }

        /// <summary>
        /// The price of the property, which must be greater than zero.
        /// </summary>
        [Required(ErrorMessage = "The property's price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public int Price { get; set; }

        /// <summary>
        /// The internal code for the property. This field is required.
        /// </summary>
        [Required(ErrorMessage = "The property's internal code is required.")]
        public string CodeInternal { get; set; }

        /// <summary>
        /// The year the property was built. The year must be between 1800 and 2024.
        /// </summary>
        [Required(ErrorMessage = "The property's year is required.")]
        [Range(1800, 2024, ErrorMessage = "Year must be between 1800 and 2024.")]
        public int Year { get; set; }

        /// <summary>
        /// The ID of the owner associated with the property.
        /// </summary>
        [Required(ErrorMessage = "The owner's ID is required.")]
        public int IdOwner { get; set; }
    }
}
