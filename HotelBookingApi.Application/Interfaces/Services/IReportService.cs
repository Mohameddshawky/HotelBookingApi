using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelBookingApi.Application.DTOs;

namespace HotelBookingApi.Application.Interfaces.Services;

public interface IReportService
{
    Task<IEnumerable<AvailableRoomDto>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken = default);
    Task<OccupancyReportDto> GetOccupancyReportAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<RoomTypeRatingReportDto>> GetRoomTypeRatingsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingHistoryDto>> GetGuestBookingHistoryAsync(Guid guestId, CancellationToken cancellationToken = default);
}
