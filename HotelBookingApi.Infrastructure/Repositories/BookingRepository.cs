using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Domain.Enums;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Infrastructure.Data;

namespace HotelBookingApi.Infrastructure.Repositories;

public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> HasOverlappingBookingAsync(Guid roomId, DateTime checkIn, DateTime checkOut, Guid? excludedBookingId = null)
    {
        var query = _dbSet.AsQueryable();
        
        if (excludedBookingId.HasValue)
        {
            query = query.Where(b => b.Id != excludedBookingId.Value);
        }

        return await query.AnyAsync(b => b.RoomId == roomId && 
                                         b.Status != BookingStatus.Cancelled &&
                                         b.CheckInDate < checkOut && 
                                         b.CheckOutDate > checkIn);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByGuestIdAsync(Guid guestId)
    {
        return await _dbSet
            .Include(b => b.Room)
            .ThenInclude(r => r!.RoomType)
            .AsNoTracking()
            .Where(b => b.GuestId == guestId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetActiveBookingsAsync()
    {
        return await _dbSet
            .AsNoTracking()
            .Where(b => b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.CheckedIn)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByRoomIdAsync(Guid roomId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(b => b.RoomId == roomId)
            .ToListAsync();
    }
}
