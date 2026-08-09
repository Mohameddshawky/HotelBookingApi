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

public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    public RoomRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut)
    {
        return await _dbSet
            .Include(r => r.RoomType)
            .AsNoTracking()
            .Where(r => r.Status == RoomStatus.Available && 
                        !r.Bookings.Any(b => b.Status != BookingStatus.Cancelled &&
                                             b.CheckInDate < checkOut && b.CheckOutDate > checkIn))
            .ToListAsync();
    }

    public async Task<Room?> GetByRoomNumberAsync(string roomNumber)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(r => r.Number == roomNumber);
    }

    public async Task<IEnumerable<Room>> GetRoomsByRoomTypeIdAsync(Guid roomTypeId)
    {
        return await _dbSet.AsNoTracking().Where(r => r.RoomTypeId == roomTypeId).ToListAsync();
    }
}
