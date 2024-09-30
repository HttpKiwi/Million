using AutoMapper;
using Million.Application.DTOs;
using Million.Domain.Enitities;
using Million.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Interfaces.Services
{
    public interface IPropertyTraceService
    {
        Task<List<PropertyTrace>> GetPropertyTraceAsync();
        Task<PropertyTrace> GetPropertyTraceByIdAsync(int id);
        Task AddPropertyTraceAsync(PropertyTraceDto propertyTraceDto);
        Task<bool> UpdatePropertyTraceAsync(PropertyTraceDto propertyTraceDto, int id);
        Task<bool> DeletePropertyTraceAsync(int id);
    }
}
