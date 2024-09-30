namespace Million.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) for filtering properties.
    /// </summary>
    public class PropertyFilterDto
    {
        /// <summary>
        /// The minimum price for filtering properties.
        /// </summary>
        public int? MinPrice { get; set; }

        /// <summary>
        /// The maximum price for filtering properties.
        /// </summary>
        public int? MaxPrice { get; set; }

        /// <summary>
        /// The year the property was built for filtering.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// The name of the property for filtering.
        /// </summary>
        public string Name { get; set; }
    }
}
