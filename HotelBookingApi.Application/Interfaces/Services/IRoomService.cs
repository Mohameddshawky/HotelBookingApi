using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Application.DTOs;

namespace HotelBookingApi.Application.Interfaces.Services;

public interface IRoomService
{
    Task<IQueryable<RoomDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RoomDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IQueryable<AvailableRoomDto>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(SaveRoomDto dto, CancellationToken cancellationToken = default);
    Task DeactivateAsync(Guid roomId, CancellationToken cancellationToken = default);
}
