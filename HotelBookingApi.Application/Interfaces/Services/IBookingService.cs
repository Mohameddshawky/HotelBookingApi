using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Application.DTOs;

namespace HotelBookingApi.Application.Interfaces.Services;

public interface IBookingService
{
    Task<BookingDetailsDto> CreateAsync(SaveBookingDto dto, CancellationToken cancellationToken = default);
    Task<BookingDetailsDto?> GetByIdAsync(Guid bookingId, CancellationToken cancellationToken = default);
    Task<IQueryable<BookingHistoryDto>> GetGuestBookingsAsync(Guid guestId, CancellationToken cancellationToken = default);
    Task ConfirmAsync(Guid bookingId, CancellationToken cancellationToken = default);
    Task CancelAsync(Guid bookingId, CancellationToken cancellationToken = default);
    Task CheckInAsync(Guid bookingId, CancellationToken cancellationToken = default);
    Task CheckOutAsync(Guid bookingId, CancellationToken cancellationToken = default);
}
