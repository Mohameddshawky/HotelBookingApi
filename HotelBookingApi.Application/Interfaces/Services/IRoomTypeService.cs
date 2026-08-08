using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Application.DTOs;

namespace HotelBookingApi.Application.Interfaces.Services;

public interface IRoomTypeService
{
    Task<IQueryable<RoomTypeDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RoomTypeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(SaveRoomTypeDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, SaveRoomTypeDto dto, CancellationToken cancellationToken = default);
}
