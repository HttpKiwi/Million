using System.ComponentModel.DataAnnotations;

namespace Million.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for Owner creation and updates.
    /// </summary>
    public class OwnerDto
    {
        /// <summary>
        /// The owner's name. This field is required.
        /// </summary>
        [Required(ErrorMessage = "The owner's name is required.")]
        public string Name { get; set; }

        /// <summary>
        /// The owner's address. This field is required.
        /// </summary>
        [Required(ErrorMessage = "The owner's address is required.")]
        public string Address { get; set; }

        /// <summary>
        /// The owner's date of birth. This field is required.
        /// </summary>
        [Required(ErrorMessage = "The owner's birthday is required.")]
        public DateTime Birthday { get; set; }
    }
}
