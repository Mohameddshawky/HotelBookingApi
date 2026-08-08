using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Application.DTOs;

namespace HotelBookingApi.Application.Interfaces.Services;

public interface IReportService
{
    Task<IQueryable<AvailableRoomDto>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken = default);
    Task<OccupancyReportDto> GetOccupancyReportAsync(CancellationToken cancellationToken = default);
    Task<IQueryable<RoomTypeRatingReportDto>> GetRoomTypeRatingsAsync(CancellationToken cancellationToken = default);
    Task<IQueryable<BookingHistoryDto>> GetGuestBookingHistoryAsync(Guid guestId, CancellationToken cancellationToken = default);
}
