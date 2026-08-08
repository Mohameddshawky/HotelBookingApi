using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelBookingApi.Domain.Entities;
using HotelBookingApi.Application.Interfaces.Repositories;
using HotelBookingApi.Infrastructure.Data;

namespace HotelBookingApi.Infrastructure.Repositories;

public class ReviewRepository : GenericRepository<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsForBookingAsync(Guid bookingId)
    {
        return await _dbSet.AnyAsync(r => r.BookingId == bookingId);
    }

    public async Task<IEnumerable<Review>> GetReviewsByRoomTypeIdAsync(Guid roomTypeId)
    {
        return await _dbSet
            .Include(r => r.Booking)
            .ThenInclude(b => b!.Room)
            .AsNoTracking()
            .Where(r => r.Booking != null && r.Booking.Room != null && r.Booking.Room.RoomTypeId == roomTypeId)
            .ToListAsync();
    }

    public async Task<Review?> GetByBookingIdAsync(Guid bookingId)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(r => r.BookingId == bookingId);
    }
}
