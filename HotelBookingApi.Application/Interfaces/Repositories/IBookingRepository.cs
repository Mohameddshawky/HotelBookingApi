using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Application.Interfaces.Repositories;

public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<bool> HasOverlappingBookingAsync(Guid roomId, DateTime checkIn, DateTime checkOut, Guid? excludedBookingId = null);
    Task<IEnumerable<Booking>> GetBookingsByGuestIdAsync(Guid guestId);
    Task<IEnumerable<Booking>> GetActiveBookingsAsync();
    Task<IEnumerable<Booking>> GetBookingsByRoomIdAsync(Guid roomId);
}
