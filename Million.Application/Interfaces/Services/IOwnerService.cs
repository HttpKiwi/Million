using Microsoft.AspNetCore.Http;
using Million.Application.DTOs;
using Million.Domain.Enitities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Million.Application.Interfaces.Services
{
    public interface IOwnerService
    {
        Task<List<Owner>> GetOwnersAsync();
        Task<Owner> GetOwnerByIdAsync(int id);
        Task AddOwnerAsync(OwnerDto ownerDto, IFormFile photo);
        Task<bool> UpdateOwnerAsync(OwnerDto ownerDto, int id);
        Task<bool> DeleteOwnerAsync(int id);
    }
}
