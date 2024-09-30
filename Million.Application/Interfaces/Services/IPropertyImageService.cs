using Microsoft.AspNetCore.Http;
using Million.Application.DTOs;
using Million.Domain.Enitities;

namespace Million.Application.Interfaces.Services
{
    public interface IPropertyImageService
    {
        Task<List<PropertyImage>> GetPropertyImageAsync();
        Task<PropertyImage> GetPropertyImageByIdAsync(int id);
        Task AddPropertyImageAsync(PropertyImageDto propertyImageDto, IFormFile photo);
        Task<bool> UpdatePropertyImageAsync(PropertyImageDto propertyImageDto, int id);
        Task<bool> DeletePropertyImageAsync(int id);
    }
}
