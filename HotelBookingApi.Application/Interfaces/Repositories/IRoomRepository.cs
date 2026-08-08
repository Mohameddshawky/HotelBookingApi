using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApi.Domain.Entities;

namespace HotelBookingApi.Application.Interfaces.Repositories;

public interface IRoomRepository : IGenericRepository<Room>
{
    Task<IQueryable<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut);
    Task<Room?> GetByRoomNumberAsync(string roomNumber);
    Task<IQueryable<Room>> GetRoomsByRoomTypeIdAsync(Guid roomTypeId);
}
