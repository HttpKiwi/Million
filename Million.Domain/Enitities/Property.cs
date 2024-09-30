using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Million.Domain.Enitities
{
    /// <summary>
    /// Entity class representing a property in the system.
    /// </summary>
    public class Property
    {
        /// <summary>
        /// The unique identifier for the property. This field is auto-generated.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProperty { get; set; }

        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The address of the property.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The price of the property.
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// The internal code for the property.
        /// </summary>
        public string CodeInternal { get; set; }

        /// <summary>
        /// The year the property was built.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// The ID of the owner associated with the property.
        /// </summary>
        public int IdOwner { get; set; }

        /// <summary>
        /// A collection of images associated with the property.
        /// </summary>
        public ICollection<PropertyImage> PropertyImages { get; set; }

        /// <summary>
        /// A collection of traces (price changes or transaction records) associated with the property.
        /// </summary>
        public ICollection<PropertyTrace> PropertyTraces { get; set; }
    }
}
