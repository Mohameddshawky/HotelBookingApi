using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Application.DTOs;

namespace HotelBookingApi.Application.Interfaces.Services;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RoomDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AvailableRoomDto>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreateOrUpdateRoomDto dto, CancellationToken cancellationToken = default);
    Task DeactivateAsync(Guid roomId, CancellationToken cancellationToken = default);
}
