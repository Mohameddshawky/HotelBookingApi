using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Application.DTOs;

namespace HotelBookingApi.Application.Interfaces.Services;

public interface IGuestService
{
    Task<GuestDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<GuestDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(SaveGuestDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, SaveGuestDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingHistoryDto>> GetBookingHistoryAsync(Guid guestId, CancellationToken cancellationToken = default);
}
