using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Application.DTOs;

namespace HotelBookingApi.Application.Interfaces.Services;

public interface IAmenityService
{
    Task<IEnumerable<AmenityDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AmenityDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreateOrUpdateAmenityDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, CreateOrUpdateAmenityDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
