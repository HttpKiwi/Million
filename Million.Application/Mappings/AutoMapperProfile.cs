using AutoMapper;
using Million.Application.DTOs;
using Million.Domain.Enitities;

namespace Million.Application.Mappings
{
    /// <summary>
    /// Configures the mappings between Data Transfer Objects (DTOs) and domain models
    /// using AutoMapper.
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperProfile"/> class.
        /// </summary>
        public AutoMapperProfile()
        {
            // Create a mapping from OwnerDto to Owner
            CreateMap<OwnerDto, Owner>();

            // Create a mapping from PropertyDto to Property
            CreateMap<PropertyDto, Property>();

            // Create a mapping from PropertyImageDto to PropertyImage
            CreateMap<PropertyImageDto, PropertyImage>();

            // Create a mapping from PropertyTraceDto to PropertyTrace
            CreateMap<PropertyTraceDto, PropertyTrace>();

        }
    }
}
