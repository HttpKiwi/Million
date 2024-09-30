using Million.Application.DTOs;
using Million.Domain.Enitities;

namespace Million.Application.Interfaces.Services
{
    public interface IPropertyService
    {
        Task<List<Property>> GetAllPropertiesAsync();
        Task<Property> GetPropertyByIdAsync(int id);
        Task<bool> AddPropertyAsync(PropertyDto propertyDto, int id);
        Task<bool> UpdatePropertyAsync(PropertyDto propertyDto, int id);
        Task<bool> DeletePropertyAsync(int id);
        Task<List<Property>> FilterPropertiesAsync(PropertyFilterDto filterDto);
    }
}
