using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Million.Domain.Enitities
{
    /// <summary>
    /// Entity class representing an owner in the system.
    /// </summary>
    public class Owner
    {
        /// <summary>
        /// The unique identifier for the owner. This field is auto-generated.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdOwner { get; set; }

        /// <summary>
        /// The owner's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The owner's address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The owner's photo as a byte array.
        /// </summary>
        public byte[] Photo { get; set; }

        /// <summary>
        /// The owner's date of birth.
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// A collection of properties owned by the owner.
        /// </summary>
        public ICollection<Property> Properties { get; set; }
    }
}
