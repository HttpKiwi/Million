﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Million.Domain.Enitities
{
    /// <summary>
    /// Represents a property trace entity in the database.
    /// </summary>
    public class PropertyTrace
    {
        /// <summary>
        /// Gets or sets the unique identifier for the property trace.
        /// This value is auto-generated by the database.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPropertyTrace { get; set; }

        /// <summary>
        /// Gets or sets the date of the property sale.
        /// </summary>
        public DateTime DateSale { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the property trace.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of the property trace.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the tax associated with the property trace.
        /// </summary>
        public int Tax { get; set; }

        /// <summary>
        /// Gets or sets the ID of the associated property.
        /// This is a foreign key referencing the <see cref="Property"/> class.
        /// </summary>
        public int IdProperty { get; set; }

        /// <summary>
        /// Navigation property for the associated property.
        /// </summary>
        public virtual Property Property { get; set; }
    }
}
